using HandlebarsDotNet.ViewEngine.Helpers;

namespace HandlebarsDotNet.ViewEngine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.Extensions.Options;

    using static HandlebarsConstants;

    public class HandlebarsViewEngine : IHandlebarsViewEngine
    {
        private readonly HandlebarsViewEngineOptions _options;

        public HandlebarsViewEngine(IOptions<HandlebarsViewEngineOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));

            if (_options.RegisterHelpers != null && _options.RegisterHelpers.Count > 0)
            {
                foreach (var (name, helper) in _options.RegisterHelpers)
                {
                    HandlebarsCache.RegisterHelper(name, helper);
                }
            }
        }

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            var controllerName = context.GetNormalizedRouteValue(ControllerKey);
            var layoutPath = _options.DefaultLayout;

            var checkedLocations = new List<string>();
            foreach (var location in _options.ViewLocationFormats)
            {
                var viewPath = string.Format(location, viewName, controllerName);
                if (File.Exists(viewPath))
                {
                    return ViewEngineResult.Found(DefaultLayout, new HandlebarsView(layoutPath, viewPath));
                }
                checkedLocations.Add(viewPath);
            }

            return ViewEngineResult.NotFound(viewName, checkedLocations);
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            var applicationRelativePath = PathHelper.GetAbsolutePath(executingFilePath, viewPath);

            if (!PathHelper.IsAbsolutePath(viewPath))
            {
                // Not a path this method can handle.
                return ViewEngineResult.NotFound(applicationRelativePath, Enumerable.Empty<string>());
            }

            var layoutPath = _options.DefaultLayout;
            return ViewEngineResult.Found(DefaultLayout, new HandlebarsView(layoutPath, applicationRelativePath));
        }
    }
}