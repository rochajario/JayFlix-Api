using plataforma_videos_api.Constants;
using plataforma_videos_api.Interfaces;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Host;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace plataforma_videos_api.Services
{
    public class VideoService : IService<Video>, IServiceExtended
    {
        private IRepositorio<Video> _videoRepo;
        private IRepositorio<Categoria> _cateRepo;

        public VideoService(IRepositorio<Video> video, IRepositorio<Categoria> categoria)
        {
            _videoRepo = video;
            _cateRepo = categoria;
        }
        public ObjectResult AtualizaItem(int id, object item)
        {
            var request = (VideoRequest)item;
            var categoria = CategoriaRequestHandler.ObtemCategoria(_cateRepo, request);
            var videoAtualizado = VideoRequestHandler.ValidaRequest(categoria, request);
            return new ObjectResult(_videoRepo.Atualiza(id, videoAtualizado)) { StatusCode = (int)HttpStatusCode.OK };
        }

        public ObjectResult CriaItem(object item)
        {
            var request = (VideoRequest)item;
            var categoria = CategoriaRequestHandler.ObtemCategoria(_cateRepo, request);
            var novoVideo = VideoRequestHandler.IdentificaColisaoDeTitulo(_videoRepo, VideoRequestHandler.ValidaRequest(categoria, request));
            _videoRepo.Cadastrar(novoVideo);
            return new ObjectResult(_videoRepo.BuscaAvancada(v => v.Titulo == request.Titulo).FirstOrDefault()) { StatusCode = (int)HttpStatusCode.Created };
        }

        public ObjectResult ObterItens(int idItemEspecifico = int.MinValue)
        {
            using (_videoRepo)
            {
                if (idItemEspecifico != int.MinValue)
                {
                    return new ObjectResult(_videoRepo.BuscarPorId(idItemEspecifico)) { StatusCode = (int)HttpStatusCode.OK };
                }
                return new ObjectResult(_videoRepo.BuscarTodos()) { StatusCode = (int)HttpStatusCode.OK };
            }
        }

        public ObjectResult ObterItensComPaginacao(int numeroPagina = default)
        {
            var itens = _videoRepo.BuscarTodos().ToArray();

            if (numeroPagina == default || itens.Length <= 5)
            {
                return ObterItens();
            }

            return new ObjectResult(CriaPaginaDeVideos(numeroPagina, itens)) { StatusCode = (int)HttpStatusCode.OK };
        }

        private List<Video> CriaPaginaDeVideos(int numeroPagina, Video[] itens)
        {
            var itemFinal = numeroPagina * 5;
            var primeiroItem = itemFinal - 5;
            var retorno = new List<Video>();

            if (primeiroItem > itens.Length)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, ErrorMessages.SEM_RESULTADOS);
            }

            if (itens.Length < itemFinal)
            {
                itemFinal = itens.Length;
            }

            for (int i = primeiroItem; i < itemFinal; i++)
            {
                retorno.Add(itens[i]);
            }
            return retorno;
        }

        public ObjectResult ObterItensPorQuery(string search = null)
        {
            return new ObjectResult(_videoRepo.BuscaAvancada(search)) { StatusCode = (int)HttpStatusCode.OK };
        }

        public ObjectResult ObterTodosItens(string search = null, int numeroPagina = default)
        {
            if (search != null)
            {
                return ObterItensPorQuery(search);
            }
            if (numeroPagina != default)
            {
                return ObterItensComPaginacao(numeroPagina);
            }

            return ObterItens();
        }

        public ObjectResult RemoveItem(int id)
        {
            _videoRepo.Deleta(id);
            return new ObjectResult("Vídeo Deletado com sucesso!") { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
