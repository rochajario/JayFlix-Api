using Microsoft.AspNetCore.Authorization;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using plataforma_videos_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace plataforma_videos_api.Controllers
{
    [Authorize]
    [Route("videos")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly VideoService _service;

        public VideoController(IService<Video> service)
        {
            _service = (VideoService)service;
        }

        [AllowAnonymous]
        [HttpGet("free")]
        public ActionResult ObterVideosGratuitos()
        {
            return _service.ObterTodosItens();
        }

        [HttpGet]
        public ActionResult ObterTodosOsVideos([FromQuery] string search = null, [FromQuery] int page = default)
        {
            return _service.ObterTodosItens(search, page);
        }

        [HttpGet("{id}")]
        public ActionResult ObterVideoPorId([FromRoute] int id)
        {
            return _service.ObterItens(id);
        }

        [HttpPost]
        public ActionResult Post([FromBody] VideoRequest request)
        {
            return _service.CriaItem(request);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizarInfoVideo([FromRoute] int id, [FromBody] VideoRequest request)
        {
            return _service.AtualizaItem(id, request);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletarVideo([FromRoute] int id)
        {
            return _service.RemoveItem(id);
        }
    }
}
