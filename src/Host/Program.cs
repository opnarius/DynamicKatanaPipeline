namespace Host
{
    using System;
    using System.Diagnostics;
    using DynamicKatanaPipeline;
    using Microsoft.Owin.Hosting;
    using Owin;

    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:12345";
            using(WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Listenting on {0}", url);
                Console.WriteLine("Edit {0}\\config.txt to reconfigure pipeline", Environment.CurrentDirectory);
                Process.Start(url);
                Console.ReadLine();
            }
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseDynamicKatanaMiddleware("config.txt");
            }
        }
    }
}
