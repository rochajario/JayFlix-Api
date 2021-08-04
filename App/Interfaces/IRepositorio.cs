using System;
using System.Collections.Generic;

namespace plataforma_videos_api.Interfaces
{
    public interface IRepositorio<T>: IDisposable
    {
        void Cadastrar(T item);
        T BuscarPorId(int id);
        T[] BuscarTodos();
        public List<T> BuscaAvancada(Func<T, bool> filtro, bool lancaExcecao = true);
        public T Atualiza(int id, T request);
        void Deleta(int id);
    }
}