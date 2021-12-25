namespace HandlebarsViewEngine
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    public class HandlebarsView : IView
    {
        private readonly string _layoutPath;
        private readonly string _viewPath;

        public HandlebarsView(string layoutPath, string viewPath)
        {
            _layoutPath = layoutPath;
            _viewPath = viewPath;
        }

        public string Path => _viewPath;

        public async Task RenderAsync(ViewContext context)
        {
            var result = await Task.Run(() => {
                var template = HandlebarsCache.GetTemplate(_layoutPath, _viewPath);
                return template(context.ViewData.Model);
            });

            context.Writer.Write(result);
        }
    }
}