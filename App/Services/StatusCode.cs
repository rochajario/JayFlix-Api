using Microsoft.AspNetCore.Mvc;

namespace plataforma_videos_api.Services
{
    internal class StatusCode : ObjectResult
    {
        public StatusCode(object value) : base(value)
        {
        }
    }
}