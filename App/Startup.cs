using plataforma_videos_api.Models;
using plataforma_videos_api.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using plataforma_videos_api.Services;

namespace plataforma_videos_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFirebaseAuthentication(Configuration);

            services.AddCors(options => options.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            }));

            services.AddControllers(options => options.Filters.Add(typeof(HttpExcepitonGlobalFilter)));
            services.AddSwaggerDocumentation(Configuration);
            services.AddDbContext<ApplicationContext>();
            services.AddScoped<IRepositorio<Video>, RepositorioVideo>();
            services.AddScoped<IRepositorio<Categoria>, RepositorioCategoria>();
            services.AddScoped<IService<Categoria>, CategoriaService>();
            services.AddScoped<IService<Video>, VideoService>();
        }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwaggerDocumentation(Configuration);
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAny");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
}
