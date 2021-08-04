using Microsoft.AspNetCore.Mvc;

namespace plataforma_videos_api.Services
{
    public interface IService<T>
    {
        public ObjectResult ObterItens(int idItemEspecifico = int.MinValue);
        public ObjectResult CriaItem(object item);
        public ObjectResult AtualizaItem(int id, object item);
        public ObjectResult RemoveItem(int id);
    }
}