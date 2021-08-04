using plataforma_videos_api.Constants;
using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using plataforma_videos_api.Services;
using plataforma_videos_api_test.InMemoryDatabses;
using ServiceStack.Host;
using System.Collections.Generic;
using Xunit;

namespace plataforma_videos_api_test
{
    public class VideoRequestHandlerTest
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", "")]
        [InlineData("teste", "teste", null)]
        [InlineData("teste", "teste", "")]
        [InlineData("teste", null, "http://teste.com")]
        [InlineData("teste", "", "http://teste.com")]
        [InlineData(null, "teste", "http://teste.com")]
        [InlineData("", "teste", "http://teste.com")]
        public void VideoModelHandler_DeveriaLancarHttpException_ParametrosInvalidos(string titulo, string descricao, string url)
        {
            Assert.Throws<HttpException>(() => VideoRequestHandler.ValidaRequest(
                new Categoria { Id = 0, Titulo = "teste", Cor = "abc" },
                new VideoRequest
                {
                    Titulo = titulo,
                    Descricao = descricao,
                    Url = url
                })
            );
        }

        [Fact]
        public void VideoRequestHandler_DeveriaLancarHttpException_UrlIncompleta()
        {
            var request = new VideoRequest
            {
                Titulo = "titulo",
                Descricao = "descricao",
                Url = "url"
            };
            Assert.Throws<HttpException>(() => VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, request));
        }

        [Fact]
        public void VideoRequestHandler_DeveriaValidarObjeto()
        {
            var request = new VideoRequest
            {
                Titulo = "titulo",
                Descricao = "descricao",
                Url = "http://url.com"
            };
            Assert.IsType<Video>(VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, request));
        }

        [Fact]
        public void VideoRequestHandler_ValidaRequest_DeveriaValidarUpdate()
        {
            var update = new VideoRequest
            {
                Titulo = "Teste",
                Descricao = "Teste",
                Url = "http://teste.com"
            };

            Assert.IsType<Video>(VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, update));
            Assert.Equal("Teste", VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, update).Titulo);
        }

        [Fact]
        public void VideoRequestHandler_ValidaRequest_DeveriaLancarExcecaoAoReceberVideoRequestNulo()
        {
            Assert.Throws<HttpException>(() => VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, null));
            Assert.Throws<HttpException>(() => VideoRequestHandler.ValidaRequest(new Categoria { Id = 0, Titulo = "teste", Cor = "abc" }, new VideoRequest { Titulo = "abc", Descricao = "", Url = null }));
        }

        [Fact]
        public void VideoRequestHandler_BuscaAvancada_DeveriaLancarExcecao()
        {
            var exception = Assert.Throws<HttpException>(() => VideoRequestHandler.BuscaAvancada(InMemoryDatabaseBuilder.VideoRepository("VideoRequestHandler_BuscaAvancada_DeveriaLancarExcecao"), "Teste"));
            Assert.Equal(Http.NotFound, exception.StatusCode);
            Assert.Equal(ErrorMessages.SEM_RESULTADOS, exception.StatusDescription);
        }

        [Fact]
        public void VideoRequestHandler_BuscaAvancada_DeveriaRetornarVideos()
        {
            var assert = VideoRequestHandler.BuscaAvancada(InMemoryDatabaseBuilder.VideoRepository("VideoRequestHandler_BuscaAvancada_DeveriaRetornarVideos"), "Titulo");
            Assert.IsType<List<Video>>(assert);
            Assert.Equal(4, assert.Count);
        }

        //TODO - Adicionar testes sobre Busca de Videos por Categoria
    }
}
