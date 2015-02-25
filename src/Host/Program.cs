namespace Host
{
    using System;
    using DynamicKatanaPipeline;
    using Microsoft.Owin.Hosting;
    using Owin;

    internal class Program
    {
        private static void Main(string[] args)
        {
            using(WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.WriteLine("Listenting on http://localhost:12345");
                Console.WriteLine("Edit {0}\\config.txt to reconfigure pipeline", Environment.CurrentDirectory);
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
