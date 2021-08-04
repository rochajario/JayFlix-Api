using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using plataforma_videos_api.Interfaces;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;

namespace plataforma_videos_api.Services
{
    public static class VideoRequestHandler
    {

        public static Video ValidaRequest(Categoria categoria, VideoRequest request)
        {
            if (request is null or default(VideoRequest))
            {
                throw new HttpException(Http.BadRequest, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "Dados Nulos"));
            }
            if (String.IsNullOrEmpty(request.Descricao))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "descricao"));
            }
            if (String.IsNullOrEmpty(request.Titulo))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "titulo"));
            }
            if (String.IsNullOrEmpty(request.Url))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "url"));
            }
            if (!request.Url.Contains("http"))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INCOMPLETO, "url", "adicione ao link o protocolo (http ou https)"));
            }
            if (!request.Url.Contains("."))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INCOMPLETO, "url", "adicione separadores de domínio (Ex. www.xpto.com)"));
            }

            return ConvertToVideo(request, categoria);
        }

        private static Video ConvertToVideo(VideoRequest request, Categoria categoria)
        {
            return new Video
            {
                Descricao = request.Descricao,
                Titulo = request.Titulo,
                Url = request.Url,
                Categoria = categoria
            };
        }

        public static List<Video> ObtemVideosPorCategoria(this IRepositorio<Video> repo, int categoriaId)
        {
            using (repo)
            {
                return repo.BuscaAvancada(v => v.Categoria.Id == categoriaId).ToList();
            }
        }

        public static Video IdentificaColisaoDeTitulo(this IRepositorio<Video> repo, Video video)
        {
            var colisao = repo.BuscaAvancada(v => v.Titulo == video.Titulo, lancaExcecao: false).FirstOrDefault();
            if (colisao != null)
            {
                throw new HttpException(Http.Conflict, String.Format(ErrorMessages.VIDEO_PRE_CADASTRADO, video.Titulo));
            }
            return video;
        }

        public static Video ValidaAtualizacaoDeVideo(this IRepositorio<Video> repo, Video video)
        {
            using (repo)
            {
                var videos = repo.BuscaAvancada(v => v.Titulo == video.Titulo && v.Id != video.Id, lancaExcecao: false);
                if (videos.Count > 0)
                {
                    throw new HttpException(Http.Conflict, ErrorMessages.VIDEO_PRE_CADASTRADO);
                }
            }
            return video;
        }

        public static List<Video> BuscaAvancada(this IRepositorio<Video> repo, string search)
        {
            return repo.BuscaAvancada(v => v.Titulo.Contains(search));
        }
    }
}
