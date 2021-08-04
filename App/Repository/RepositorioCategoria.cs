using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;

namespace plataforma_videos_api.Interfaces
{
    public class RepositorioCategoria : IRepositorio<Categoria>
    {
        private ApplicationContext _app;

        public RepositorioCategoria(ApplicationContext app)
        {
            _app = app;
        }

        public Categoria Atualiza(int id, Categoria request)
        {
            var categoria = BuscarPorId(id);
            categoria.Titulo = request.Titulo;
            categoria.Cor = request.Cor;
            _app.Update(categoria);
            return _app.SaveChanges() != 0 ? categoria : throw new HttpException(Http.UnprocessableEntity, ErrorMessages.FALHA_AO_ATUALIZAR);
        }

        public List<Categoria> BuscaAvancada(Func<Categoria, bool> filtro, bool lancaExcecao = true)
        {
            var resultadoQuery = _app.Categorias.Where(filtro).ToList();
            if (!lancaExcecao)
            {
                return resultadoQuery;
            }

            if (resultadoQuery.Count == 0)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            return resultadoQuery;
        }

        public Categoria BuscarPorId(int id)
        {
            var response = _app.Categorias.FirstOrDefault(v => v.Id == id);
            if (response == null)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            return response;
        }

        public Categoria[] BuscarTodos()
        {
            var response = _app.Categorias.ToArray();
            if (response.Length == 0)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            return response;
        }

        public void Cadastrar(Categoria item)
        {
            _app.Categorias.Add(item);
            _app.SaveChanges();
        }

        public void Deleta(int id)
        {
            var categoria = BuscarPorId(id);
            if (categoria == null)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            _app.Categorias.Remove(categoria);
            _app.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool v)
        {
            _app = default;
        }
    }
}
