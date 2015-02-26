namespace DynamicKatanaPipeline
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.Owin.Builder;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;
    using Owin;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class DynamicKatanaMiddleware
    {
        private AppFunc _dynamicAppFunc;
        private readonly FileSystemWatcher _fileSystemWatcher;

        public DynamicKatanaMiddleware(AppFunc next, string configurationFilePath)
        {
            _fileSystemWatcher = new FileSystemWatcher(Environment.CurrentDirectory)
            {
                EnableRaisingEvents = true
            };
            _fileSystemWatcher.Changed += (_, __) =>
            {
                ConfigurePipeline(next, configurationFilePath);
            };
            ConfigurePipeline(next, configurationFilePath);
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            return _dynamicAppFunc(environment);
        }

        private void ConfigurePipeline(AppFunc next, string fullPath)
        {
            Console.WriteLine("Configuring pipeline...");
            try
            {
                string index = string.Format(
                    "<HTML><HEAD><TITLE>Dynamic Middleware</TITLE></HEAD><BODY><p>Edit {0} and refresh to see below links change.</p>",
                    fullPath);
                var allLines = File.ReadAllLines(fullPath);

                var app = new AppBuilder();
                foreach(var line in allLines)
                {
                    var paths = line.Split(';');
                    app.UseDirectoryBrowser(new DirectoryBrowserOptions
                    {
                        RequestPath = new PathString(paths[0]),
                        FileSystem = new PhysicalFileSystem(paths[1])
                    });
                    index += string.Format("<a href='{0}'>{0}</a><br>", paths[0]);
                    Console.WriteLine("Directory {0} on {1}", paths[1], paths[0]);
                }
                app.Use(async (ctx, next2) =>
                {
                    if(ctx.Request.Path.Value == "/")
                    {
                        await ctx.Response.WriteAsync(index);
                    }
                    await next2();
                });
                app.Run(ctx => next(ctx.Environment));
                _dynamicAppFunc = app.Build();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                _dynamicAppFunc = async env =>
                {
                    var context = new OwinContext(env);
                    context.Response.StatusCode = 500;
                    context.Response.ReasonPhrase = "Internal server error";
                    await context.Response.WriteAsync(ex.Message);
                };
            }
        }
    }
}
