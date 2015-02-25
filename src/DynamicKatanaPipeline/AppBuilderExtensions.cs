namespace DynamicKatanaPipeline
{
    using Owin;

    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseDynamicKatanaMiddleware(this IAppBuilder app, string configurationFilePath)
        {
            return app.Use<DynamicKatanaMiddleware>(configurationFilePath);
        }
    }
}