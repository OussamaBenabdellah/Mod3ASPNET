using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mod3ASPNET.Doc
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo
                {
                    Title = "Mon Api",
                    Version = desc.ApiVersion.ToString()
                };
                options.SwaggerDoc(desc.GroupName, info);
            }
        }
    }
}
