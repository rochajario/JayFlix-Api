using plataforma_videos_api.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceStack.Host;
using System.Net;

namespace plataforma_videos_api
{
    public class HttpExcepitonGlobalFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not HttpException)
            {
                context.Result = new ObjectResult(ErrorMessages.ERRO_INESPERADO) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
            else
            {
                HttpException ex = (HttpException)context.Exception;
                context.Result = new ObjectResult(ex.StatusDescription) { StatusCode = ex.StatusCode };
            }
            context.ExceptionHandled = true;
        }
    }
}
