namespace HandlebarsViewEngine
{
    using System.Collections.Generic;

    using HandlebarsDotNet;

    public class HandlebarsViewEngineOptions
    {
        public IList<string> ViewLocationFormats { get; } = new List<string>();

        public IReadOnlyDictionary<string, HandlebarsHelper> RegisterHelpers = new Dictionary<string, HandlebarsHelper>();

        public string DefaultLayout { get; set; }
    }
}