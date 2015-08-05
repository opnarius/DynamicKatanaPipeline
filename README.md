# DynamicKatanaPipeline

[Example Katana style middleware](https://github.com/damianh/DynamicKatanaPipeline/blob/master/src/DynamicKatanaPipeline/DynamicKatanaMiddleware.cs) which contains one or more `Microsoft.Owin.StaticFiles` middleware that reconfigures itself based on changes to a config file triggered from a filesystem watcher.

Run the host project, edit the `config.txt` file (should be obvious) and refresh the page.

The key trick is to [new up your own](https://github.com/damianh/DynamicKatanaPipeline/blob/master/src/DynamicKatanaPipeline/DynamicKatanaMiddleware.cs#L47) `IAppBuilder` instance _within_ your middleware and [direct subsequent requests down the new pipeline](https://github.com/damianh/DynamicKatanaPipeline/blob/master/src/DynamicKatanaPipeline/DynamicKatanaMiddleware.cs#L68). 

You can use this infront of any pipeline that you want to rebuild at runtime i.e. multi-tenent scenarios, or changing settings for security middleware.

Note: this is just an example and isn't robust by any means.
