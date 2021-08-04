using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace plataforma_videos_api.Interfaces.Extensions
{
    public static class RepositorioVideoExtension
    {
        public static List<Video> ObtemVideosPorCategoria(this IRepositorio<Video> repo, int categoriaId)
        {
            return repo.BuscaAvancada(v => v.Categoria.Id == categoriaId).ToList();
        }

        public static Video IdentificaColisaoDeTitulo(this IRepositorio<Video> repo, Video video)
        {
            var colisao = repo.BuscaAvancada(v => v.Titulo == video.Titulo, lancaExcecao: false).FirstOrDefault();
            if (colisao != null)
            {
                throw new HttpException((int)HttpStatusCode.Conflict, String.Format(ErrorMessages.VIDEO_PRE_CADASTRADO, video.Titulo));
            }
            return video;
        }

        public static Video ValidaAtualizacaoDeVideo(this IRepositorio<Video> repo, Video video)
        {
            var videos = repo.BuscaAvancada(v => v.Titulo == video.Titulo && v.Id != video.Id, lancaExcecao: false);
            if (videos.Count > 0)
            {
                throw new HttpException(Http.Conflict, ErrorMessages.VIDEO_PRE_CADASTRADO);
            }
            return video;
        }

        public static List<Video> BuscaAvancada(this IRepositorio<Video> repo, string search)
        {
            return repo.BuscaAvancada(v => v.Titulo.Contains(search));
        }
    }
}
