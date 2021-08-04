using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using plataforma_videos_api.Interfaces;
using ServiceStack.Host;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace plataforma_videos_api.Services
{
    public static class CategoriaRequestHandler
    {
        public static Categoria ValidaRequest(CategoriaRequest request)
        {
            Regex regexHexadecimal = new("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
            if(request == null || request == default)
            {
                throw new HttpException(Http.BadRequest, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "Dados Nulos"));
            }
            if (String.IsNullOrWhiteSpace(request.Titulo))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "Título"));
            }
            if (String.IsNullOrWhiteSpace(request.Cor))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "Cor"));
            }

            if (!regexHexadecimal.IsMatch(request.Cor))
            {
                throw new HttpException(Http.UnprocessableEntity, String.Format(ErrorMessages.ARGUMENTO_INCOMPLETO, "Cor", "A cor deve seguir o padrão Hexadecimal. Ex. #FFFFFF (Branco) "));
            }

            return ConvertToCategoria(request);
        }

        public static Categoria ObtemCategoria(IRepositorio<Categoria> repo, VideoRequest request)
        {
            using (repo)
            {
                if (String.IsNullOrWhiteSpace(request.NomeCategoria))
                {
                    request.NomeCategoria = "LIVRE";
                }

                request.NomeCategoria = request.NomeCategoria.ToUpper().Trim();

                var categoria = repo.BuscaAvancada(c => c.Titulo == request.NomeCategoria, false).FirstOrDefault();
                if (categoria == default)
                {
                    throw new HttpException(Http.PreconditionFailed, ErrorMessages.CATEGORIA_NECESSARIA);
                }

                return categoria;
            }
        }

        private static Categoria ConvertToCategoria(CategoriaRequest request)
        {
            return new Categoria
            {
                Titulo = request.Titulo.ToUpper().Trim(),
                Cor = request.Cor
            };
        }

        

        public static Categoria ValidaAtualizacaoDeCategoria(this IRepositorio<Categoria> repo, Categoria categoria)
        {
            using (repo)
            {
                var categorias = repo.BuscaAvancada(v => v.Titulo == categoria.Titulo && v.Id != categoria.Id, lancaExcecao: false);
                if (categorias.Count > 0)
                {
                    throw new HttpException(Http.Conflict, ErrorMessages.CATEGORIA_PRE_CADASTRADA);
                }
            }
            return categoria;
        }
    }
}
