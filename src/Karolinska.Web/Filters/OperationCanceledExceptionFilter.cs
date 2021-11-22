using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Karolinska.Web.Filters
{
    public class OperationCanceledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public OperationCanceledExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OperationCanceledExceptionFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(400);
            }
        }
    }
}