using plataforma_videos_api.Models;
using plataforma_videos_api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace plataforma_videos_tests.InMemoryDatabses
{
    public static class InMemoryDatabaseBuilder
    {
        public static IRepositorio<Video> VideoRepository(string testName)
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(testName)
                .Options;
            var contexto = new ApplicationContext(options);
            var repo = new RepositorioVideo(contexto);

            for (int i = 0; i < 4; i++)
            {
                var cont = Convert.ToString(i + 1);
                repo.Cadastrar(
                    new Video
                    {
                        Titulo = "Titulo 0" + cont,
                        Descricao = "Desc 0" + cont,
                        Url = "https://url" + cont + ".com"
                    });
            }

            return repo;
        }

        public static IRepositorio<Categoria> CategoriaRepository(string testName)
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(testName)
                .Options;
            var contexto = new ApplicationContext(options);
            var repo = new RepositorioCategoria(contexto);

           repo.Cadastrar(
                    new Categoria
                    {
                        Id = (1),
                        Titulo = "LIVRE",
                        Cor = "#FFFFFF"
                    });

            for (int i = 1; i < 5; i++)
            {
                var cont = Convert.ToString(i + 1);
                repo.Cadastrar(
                    new Categoria
                    {
                        Id = (i + 1),
                        Titulo = "Titulo 0" + cont,
                        Cor = "#FFFFFF" + cont
                    });
            }

            return repo;
        }
    }
}
