using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api_test.InMemoryDatabses;
using ServiceStack.Host;
using System.Linq;
using Xunit;

namespace plataforma_videos_api_test
{
    public static class RepositoryCategoriaTest
    {

        [Fact]
        public static void BuscarTodasCategorias()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscarTodasCategorias");
            Assert.Equal(5, repo.BuscarTodos().Length);
        }

        [Fact]
        public static void BuscarCategoriaPorId()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscarCategoriaPorId");
            Assert.Equal("LIVRE", repo.BuscarPorId(1).Titulo);
        }

        [Fact]
        public static void BuscarCategoriaPorFiltro()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscarCategoriaPorFiltro");
            Assert.Equal("Titulo 03", repo.BuscaAvancada(c => c.Titulo == "Titulo 03").FirstOrDefault().Titulo);
        }

        [Fact]
        public static void CadastrarNovaCategoria()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("CadastrarNovaCategoria");
            repo.Cadastrar(new Categoria() { Id = 6, Titulo = "Comédia", Cor = "" });
            Assert.Equal(6, repo.BuscarTodos().Length);
            Assert.Equal("Comédia", repo.BuscarPorId(6).Titulo);
        }

        [Fact]
        public static void AtualizarCategoria()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("AtualizarCategoria");
            repo.Atualiza(1, new Categoria() { Titulo = "Terror", Cor = "#asd" });
            Assert.Equal("Terror", repo.BuscarPorId(1).Titulo);
        }

        [Fact]
        public static void RemoverCategoria()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("RemoverCategoria");
            repo.Deleta(2);
            Assert.Equal(4, repo.BuscarTodos().Length);
        }

        [Fact]
        public static void BuscarTodasCategorias_LancaErro404()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscarTodasCategorias_LancaErro404");
            foreach (var item in repo.BuscarTodos())
            {
                repo.Deleta(item.Id);
            }
            var exception = Assert.Throws<HttpException>(() => repo.BuscarTodos());
            Assert.Equal(Http.NotFound, exception.StatusCode);
            Assert.Equal(ErrorMessages.SEM_RESULTADOS, exception.StatusDescription);
        }

        [Fact]
        public static void BuscarCategoriaPorId_LancaErro404()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscarCategoriaPorId_LancaErro404");
            Assert.Throws<HttpException>(() => repo.BuscarPorId(100));
        }

        [Fact]
        public static void BuscaCategoriaAvancada_LancaErro404()
        {
            using var repo = InMemoryDatabaseBuilder.CategoriaRepository("BuscaCategoriaAvancada_LancaErro404");
            var exception = Assert.Throws<HttpException>(() => repo.BuscaAvancada(c => c.Titulo == "teste", true));
            Assert.Equal(Http.NotFound, exception.StatusCode);
            Assert.Equal(ErrorMessages.SEM_RESULTADOS, exception.StatusDescription);
        }
    }
}
