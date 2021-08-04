using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plataforma_videos_api.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public Categoria Categoria { get; set; }
    }
}
