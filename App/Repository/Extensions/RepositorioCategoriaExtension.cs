using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace plataforma_videos_api.Interfaces
{
    public static class RepositorioCategoriaExtension
    {
        public static Categoria ObtemCategoria(IRepositorio<Categoria> repo, VideoRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.NomeCategoria))
            {
                request.NomeCategoria = "LIVRE";
            }
            request.NomeCategoria = request.NomeCategoria.ToUpper().Trim();
            var categoria = repo.BuscaAvancada(c => c.Titulo == request.NomeCategoria, false).FirstOrDefault();
            if (categoria == default)
            {
                throw new HttpException((int)HttpStatusCode.PreconditionFailed, ErrorMessages.CATEGORIA_NECESSARIA);
            }
            return categoria;
        }

        public static Categoria IdentificaColisaoDeTitulo(this IRepositorio<Categoria> repo, Categoria categoria)
        {
            var ocorrencias = repo.BuscaAvancada(v => v.Titulo == categoria.Titulo, false).Count;
            if (ocorrencias > 0)
            {
                throw new HttpException((int)HttpStatusCode.Conflict, String.Format(ErrorMessages.CATEGORIA_PRE_CADASTRADA, categoria.Titulo));
            }
            return categoria;
        }

        public static Categoria ValidaAtualizacaoDeCategoria(this IRepositorio<Categoria> repo, Categoria categoria)
        {
            var categorias = repo.BuscaAvancada(v => v.Titulo == categoria.Titulo && v.Id != categoria.Id, lancaExcecao: false);
            if (categorias.Count > 0)
            {
                throw new HttpException(Http.Conflict, ErrorMessages.CATEGORIA_PRE_CADASTRADA);
            }
            return categoria;
        }
    }
}
