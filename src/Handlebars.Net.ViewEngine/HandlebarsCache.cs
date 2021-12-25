namespace HandlebarsDotNet.ViewEngine
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using HandlebarsDotNet;
    using HandlebarsDotNet.Helpers;

    using static HandlebarsConstants;

    public static class HandlebarsCache
    {
        private static readonly IDictionary<ViewDefinition, HandlebarsTemplate<object, string>> _views = new Dictionary<ViewDefinition, HandlebarsTemplate<object, string>>();
        private static readonly object _locker = new();
        private static readonly IHandlebars _handlebarsContext;

        static HandlebarsCache()
        {
            _handlebarsContext = Handlebars.Create();

            // Register all partials
            var partials = new List<string>();

            // Match all directories under "Views" with "Partials" in the name
            foreach (var directory in Directory.EnumerateDirectories(ViewsFolder, PartialsFolder, SearchOption.AllDirectories))
            {
                // Register all hbs files within the directory
                foreach (var path in Directory.EnumerateFiles(directory, "*" + ViewExtension, SearchOption.TopDirectoryOnly))
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    if (partials.Exists(p => p.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new Exception($"Duplicate partial view: '{name}' ({path}). All partial views must be uniquely named regardless of folder");
                    }
                    partials.Add(path);

                    var partialTemplate = File.ReadAllText(path);
                    _handlebarsContext.RegisterTemplate(name, partialTemplate);
                }
            }

            HandlebarsHelpers.Register(_handlebarsContext);
        }

        public static HandlebarsTemplate<object, string> GetTemplate(string layoutPath, string viewPath)
        {
            lock (_locker)
            {
                var viewDef = new ViewDefinition(layoutPath, viewPath);

                if (!_views.ContainsKey(viewDef))
                {
                    // Get layout template (e.g. Views/Shared/layout.hbs)
                    var layout = File.ReadAllText(layoutPath);
                    
                    // Get view template (e.g. Views/Home/Index.hbs)
                    var source = File.ReadAllText(viewPath);

                    // Create page (insert view into layout)
                    var page = layout.Replace("{{{body}}}", source);

                    // Compile template
                    //var template = Handlebars.Compile(page);
                    var template = _handlebarsContext.Compile(page);

                    _views.Add(viewDef, template);
                    return template;
                }
                else
                {
                    return _views[viewDef];
                }                           
            }
        }

        public static void RegisterHelper(string helperName, HandlebarsHelper helper)
        {
            _handlebarsContext.RegisterHelper(helperName, helper);
        }

        public static void RegisterBlockHelper(string helperName, HandlebarsBlockHelper helper)
        {
            _handlebarsContext.RegisterHelper(helperName, helper);
        }
    }

    internal class ViewDefinition : IEquatable<ViewDefinition>
    {
        public string LayoutPath { get; set; }
        public string ViewPath { get; set; }

        public ViewDefinition(string layoutPath, string viewPath)
        {
            if (string.IsNullOrWhiteSpace(layoutPath))
                throw new ArgumentException(nameof(layoutPath) + " is null or whitespace", nameof(layoutPath));

            if (string.IsNullOrWhiteSpace(viewPath))
                throw new ArgumentException(nameof(viewPath) + " is null or whitespace", nameof(viewPath));
        }

        public bool Equals(ViewDefinition other)
        {
            return LayoutPath.Equals(other.LayoutPath, StringComparison.OrdinalIgnoreCase) &&
                ViewPath.Equals(other.ViewPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}