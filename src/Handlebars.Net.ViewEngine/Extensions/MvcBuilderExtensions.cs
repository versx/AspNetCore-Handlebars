namespace HandlebarsDotNet.ViewEngine
{
    using System;

    using HandlebarsDotNet;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddHandlebars(this IMvcBuilder builder, Action<HandlebarsViewEngineOptions> setupAction = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddOptions()
                            .AddTransient<IConfigureOptions<HandlebarsViewEngineOptions>, HandlebarsViewEngineOptionsSetup>();

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            /*
            var options = builder.Services.BuildServiceProvider().GetService<IOptions<HandlebarsViewEngineOptions>>()
                .Value;

            if (options.RegisterHelpers != null)
            {
                var helpers = new HelperList();
                options.RegisterHelpers.Invoke(helpers);
                var typeBaseHelper = typeof(HandlebarsHelper);
                foreach (var helper in helpers)
                {
                    if (helper.IsSubclassOf(typeBaseHelper))
                    {
                        builder.Services.AddScoped(helper);
                    }
                    else
                    {
                        throw new TypeMismatchException(helper, typeBaseHelper);
                    }
                }
            }
            */

            builder.Services
                .AddTransient<IConfigureOptions<MvcViewOptions>, HandlebarsMvcViewOptionsSetup>()
                .AddSingleton<IHandlebarsViewEngine, HandlebarsViewEngine>();

            return builder;
        }
    }

    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(Type subType, Type parentType) : base(
            $"Type {subType} doesn't implement interface {parentType}")
        {
        }
    }
}