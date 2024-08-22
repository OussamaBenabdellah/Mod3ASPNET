
namespace Mod3ASPNET.Filter
{
    public class LoggingFilter : IEndpointFilter
    {
        private ILogger<LoggingFilter> logger;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
                                                    EndpointFilterDelegate next)
        {
            {
                ///ce qui est ecrit avant le next s'excute avant le endpoint 
                ///ce qui est apres c=s'excute apres 
                 logger.LogInformation("on va commencer l'endpoint file");
                var result = await next(context);
                logger.LogInformation("on va terminer l'endpoint file");
                return result;
            }
        }
    }
}
