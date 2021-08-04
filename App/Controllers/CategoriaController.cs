using plataforma_videos_api.Models.Request;
using Microsoft.AspNetCore.Mvc;
using plataforma_videos_api.Models;
using plataforma_videos_api.Services;
using System;
using Microsoft.AspNetCore.Authorization;

namespace plataforma_videos_api.Controllers
{
    [Authorize]
    [Route("categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(IService<Categoria> categoriaService)
        {
            _categoriaService = (CategoriaService)categoriaService;
        }

        [HttpGet]
        public IActionResult GetPagination([FromQuery] int page = default)
        {
            return _categoriaService.ObterItensComPaginacao(page);
        }

        [HttpGet("{id}")]
        public IActionResult GetSpecific([FromRoute] int id)
        {
            return _categoriaService.ObterItens(id);
        }

        [HttpGet("{id}/videos")]
        public IActionResult GetVideosPorCategoria([FromRoute] int id)
        {
            
            return _categoriaService.ObterItensPorQuery(Convert.ToString(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoriaRequest value)
        {
            return _categoriaService.CriaItem(value);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] CategoriaRequest request)
        {
            return _categoriaService.AtualizaItem(id, request);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _categoriaService.RemoveItem(id);
        }
    }
}
