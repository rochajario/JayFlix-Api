using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api.Interfaces;
using plataforma_videos_api_test.InMemoryDatabses;
using ServiceStack.Host;
using System.Linq;
using Xunit;

namespace plataforma_videos_api_test
{
    public class RepositoryVideoTest
    {

        [Fact]
        public void BuscarTodosOsVideos()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("BuscarTodosOsVideos");
            Assert.Equal(4, repo.BuscarTodos().Length);
            foreach (var video in repo.BuscarTodos())
            {
                Assert.IsType<Video>(video);
            }
        }

        [Fact]
        public void BuscarVideoPorId()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("BuscarVideoPorId");
            Assert.IsType<Video>(repo.BuscarPorId(1));
            Assert.Equal("Titulo 01", repo.BuscarPorId(1).Titulo);
        }

        [Fact]
        public void BuscarVideoPorQuery()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("BuscarVideoPorQuery");
            var video = repo.BuscaAvancada(v => v.Titulo == "Titulo 04").FirstOrDefault();
            Assert.NotNull(video);
        }

        [Fact]
        public void AtualizaVideo()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("AtualizaVideo");
            var videoAtualizado = new Video
            {
                Id = 1,
                Titulo = "Alterando Título",
                Descricao = "Alterando Descricao",
                Url = "http://urlalterada.com"
            };

            var video = repo.Atualiza(1, videoAtualizado);

            Assert.Equal("Alterando Título", video.Titulo);
        }

        [Fact]
        public void DeletaVideo()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("DeletaVideo");
            repo.Deleta(1);
            Assert.Equal(3, repo.BuscarTodos().Length);
        }

        [Fact]
        public void DeletaVideo_FalhaAoTentarDeletarVideoInexistente()
        {
            using var repo = InMemoryDatabaseBuilder.VideoRepository("DeletaVideo_FalhaAoTentarDeletarVideoInexistente");
            var exception = Assert.Throws<HttpException>(() => repo.Deleta(100));

            Assert.Throws<HttpException>(() => repo.Deleta(100));
            Assert.Equal(Http.NotFound, exception.StatusCode);
            Assert.Equal(ErrorMessages.SEM_RESULTADOS, exception.StatusDescription);
        }
    }
}
