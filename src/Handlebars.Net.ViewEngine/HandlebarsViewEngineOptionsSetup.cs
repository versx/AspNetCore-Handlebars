namespace HandlebarsViewEngine
{
    using Microsoft.Extensions.Options;

    using static HandlebarsConstants;

    public class HandlebarsViewEngineOptionsSetup : ConfigureOptions<HandlebarsViewEngineOptions>
    {
        public HandlebarsViewEngineOptionsSetup() : base(Configure) { }

        private new static void Configure(HandlebarsViewEngineOptions options)
        {
            options.ViewLocationFormats.Add(ViewsFolder + "/{1}/{0}" + ViewExtension);
            options.ViewLocationFormats.Add(ViewsFolder + "/Shared/{0}" + ViewExtension);
            options.DefaultLayout = ViewsFolder + "/Shared/layout.hbs";
        }
    }
}