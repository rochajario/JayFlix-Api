using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;

namespace plataforma_videos_api.Interfaces
{
    public class RepositorioVideo : IRepositorio<Video>
    {
        private ApplicationContext _app;

        public RepositorioVideo(ApplicationContext app)
        {
            _app = app;
        }

        public void Cadastrar(Video item)
        {
            _app.Videos.Add(item);
            _app.SaveChanges();
        }

        public Video Atualiza(int id, Video request)
        {
            var video = BuscarPorId(id);
            video.Titulo = request.Titulo;
            video.Url = request.Url;
            video.Categoria = request.Categoria;

            _app.Update(video);
            return _app.SaveChanges() != 0 ? video : throw new HttpException(Http.UnprocessableEntity, ErrorMessages.FALHA_AO_ATUALIZAR);
        }

        public void Deleta(int id)
        {
            var video = BuscarPorId(id);
            if(video == null)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            _app.Videos.Remove(video);
            _app.SaveChanges();
        }

        public Video BuscarPorId(int id)
        {
            var response = _app.Videos.Include(v => v.Categoria).FirstOrDefault(v => v.Id == id);
            if (response == null)
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            return response;
        }

        public Video[] BuscarTodos()
        {
            var response = _app.Videos.Include(v => v.Categoria).ToArray();
            if (response.Length.Equals(0))
            {
                throw new HttpException(Http.NotFound, ErrorMessages.SEM_RESULTADOS);
            }
            return response;
        }

        public List<Video> BuscaAvancada(Func<Video, bool> filtro, bool lancaExcecao = true)
        {
            var resultadoQuery = _app.Videos.Include(v => v.Categoria).Where(filtro).ToList();
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
