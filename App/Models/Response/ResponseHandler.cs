using plataforma_videos_api.Constants;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Host;
using System;

namespace plataforma_videos_api.Models.Response
{
    public static class ResponseHandler
    {
        public static ObjectResult Wrap(this ControllerBase controller,int httpStatusCode, object command, string message=null)
        {
            try
            {
                if (message != null)
                {
                    return controller.StatusCode(httpStatusCode, message);
                }
                return controller.StatusCode(httpStatusCode, command);
            }
            catch (HttpException ex)
            {
                return controller.StatusCode(ex.StatusCode, ex.StatusDescription);
            }
            catch (Exception)
            {
                return controller.StatusCode(Http.InternalServerError, ErrorMessages.ERRO_INESPERADO);
            }
        }
    }
}
