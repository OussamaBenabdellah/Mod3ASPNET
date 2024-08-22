
using System.Net.Mime;
using System.Text;

namespace Mod3ASPNET
{
    public class HtmlResult : IResult
    {
        private readonly string html;

        public HtmlResult (string html)
        {
            this.html = html; 
        }
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(html);
            httpContext.Response.ContentType = MediaTypeNames.Text.Html;
            await httpContext.Response.WriteAsync(html);
            
        }
    }
}
