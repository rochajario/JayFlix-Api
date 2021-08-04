using Microsoft.AspNetCore.Mvc;

namespace plataforma_videos_api.Interfaces
{
    public interface IServiceExtended
    {
        public ObjectResult ObterItensPorQuery(string search = null);
        public ObjectResult ObterItensComPaginacao(int numeroPagina = default);
    }
}
