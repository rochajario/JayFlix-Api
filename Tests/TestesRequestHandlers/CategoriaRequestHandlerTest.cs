using plataforma_videos_api.Models;
using plataforma_videos_api.Models.Request;
using plataforma_videos_api.Services;
using ServiceStack.Host;
using System;
using Xunit;

namespace plataforma_videos_tests
{

    public static class CategoriaRequestHandlerTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("teste", null)]
        [InlineData("teste", "")]
        [InlineData("", "#asd")]
        [InlineData(null, "#asd")]
        public static void CategoriaModelHandler_DeveriaLancarHttpException_ParametrosInvalidos(string titulo, string cor)
        {
            Assert.Throws<HttpException>(() => CategoriaRequestHandler.ValidaRequest(
                new CategoriaRequest
                {
                    Titulo = titulo,
                    Cor = cor
                })
            );
        }

        [Fact]
        public static void CategoriaRequestHandler_DeveriaLancarHttpException_UrlIncompleta()
        {
            var request = new CategoriaRequest
            {
                Titulo = "titulo",
                Cor = "cor"
            };
            Assert.Throws<HttpException>(() => CategoriaRequestHandler.ValidaRequest(request));
        }

        [Fact]
        public static  void CategoriaRequestHandler_ValidaRequest_DeveriaLancarExcecaoRegex()
        {
            var request = new CategoriaRequest
            {
                Titulo = "titulo",
                Cor = "#cor"
            };
            Assert.Throws<HttpException>(() => CategoriaRequestHandler.ValidaRequest(request));
        }

        [Fact]
        public static void CategoriaRequestHandler_ValidaRequest_DeveriaValidarUpdate()
        {
            var update = new CategoriaRequest
            {
                Cor = "#008F00",
                Titulo = "teste"
            };

            Assert.IsType<Categoria>(CategoriaRequestHandler.ValidaRequest(update));
            Assert.Equal("#008F00", CategoriaRequestHandler.ValidaRequest(update).Cor);
            Assert.Equal("TESTE", CategoriaRequestHandler.ValidaRequest(update).Titulo);
        }

        [Fact]
        public static void CategoriaRequestHandler_ValidaRequest_DeveriaLancarExcecaoAoReceberVideoRequestNulo()
        {
            Assert.Throws<HttpException>(() => CategoriaRequestHandler.ValidaRequest(null));
            Assert.Throws<HttpException>(() => CategoriaRequestHandler.ValidaRequest(new CategoriaRequest{ Titulo = "" }));
        }
    }
}

