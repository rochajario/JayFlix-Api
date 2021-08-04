using plataforma_videos_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace plataforma_videos_api.Interfaces
{
    public class ApplicationContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Video> Videos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public ApplicationContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Video>()
                .ToTable("videos")
                .HasKey(v => v.Id);

            modelBuilder.Entity<Categoria>()
                .ToTable("categorias")
                .HasKey(c => c.Id);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var version = new Version(
                    _configuration.GetValue<int>("DatabaseConfig:MajorVersion"), 
                    _configuration.GetValue<int>("DatabaseConfig:MinorVersion"));

                optionsBuilder
                .UseMySql(
                    _configuration.GetValue<string>("DatabaseConfig:ConnectionString"), 
                    new MySqlServerVersion(version))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            }
        }
    }
}
