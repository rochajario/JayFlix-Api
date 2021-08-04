using plataforma_videos_api.Models;
using plataforma_videos_api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using plataforma_videos_api.Models.Request;
using ServiceStack.Host;
using plataforma_videos_api.Constants;
using System;
using System.Collections.Generic;

namespace plataforma_videos_api.Services
{
    public class CategoriaService : IService<Categoria>, IServiceExtended
    {
        private IRepositorio<Categoria> _categRepo;
        private IRepositorio<Video> _videoRepo;

        public CategoriaService(IRepositorio<Categoria> categoria, IRepositorio<Video> video)
        {
            _categRepo = categoria;
            _videoRepo = video;

        }

        public ObjectResult AtualizaItem(int id, object item)
        {
            using (_categRepo)
            {
                var request = (CategoriaRequest)item;
                var categoriaAtualizada = CategoriaRequestHandler.ValidaRequest(request);
                return new ObjectResult(_categRepo.Atualiza(id, categoriaAtualizada)) { StatusCode = (int)HttpStatusCode.OK };
            }
        }

        public ObjectResult CriaItem(object item)
        {
            using (_categRepo)
            {
                CategoriaRequest request = (CategoriaRequest)item;
                var newCategory = _categRepo.IdentificaColisaoDeTitulo(CategoriaRequestHandler.ValidaRequest(request));
                _categRepo.Cadastrar(newCategory);
                var response = _categRepo.BuscaAvancada(v => v.Titulo == request.Titulo.ToUpper()).FirstOrDefault();
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.Created };
            }
        }

        public ObjectResult ObterItens(int idItemEspecifico = int.MinValue)
        {
            using (_categRepo)
            {
                if (idItemEspecifico != int.MinValue)
                {
                    return new ObjectResult(_categRepo.BuscarPorId(idItemEspecifico)) { StatusCode = (int)HttpStatusCode.OK };
                }
                return new ObjectResult(_categRepo.BuscarTodos()) { StatusCode = (int)HttpStatusCode.OK };
            }
        }

        public ObjectResult ObterItensComPaginacao(int numeroPagina = default)
        {
            var itens = _categRepo.BuscarTodos().ToArray();

            if (numeroPagina == default || itens.Length <= 5)
            {
                return ObterItens();
            }

            return new ObjectResult(CriaPaginaDeCategorias(numeroPagina, itens)) { StatusCode = (int)HttpStatusCode.OK };
        }

        private List<Categoria> CriaPaginaDeCategorias(int numeroPagina, Categoria[] itens)
        {
            var itemFinal = numeroPagina * 5;
            var primeiroItem = itemFinal - 5;
            var retorno = new List<Categoria>();

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
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, String.Format(ErrorMessages.ARGUMENTO_INVALIDO, "id"));
            }
            var id = Convert.ToInt32(search);
            return new ObjectResult(_videoRepo.BuscaAvancada(v => v.Categoria.Id == id)) { StatusCode = (int)HttpStatusCode.OK };
        }

        public ObjectResult RemoveItem(int id)
        {
            using (_categRepo)
            {
                _categRepo.Deleta(id);
                return new ObjectResult("Categoria Deletada com sucesso!") { StatusCode = (int)HttpStatusCode.OK };
            }
        }
    }
}
