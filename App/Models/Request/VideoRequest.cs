using System.ComponentModel.DataAnnotations;

namespace plataforma_videos_api.Models.Request
{
    public class VideoRequest
    {
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }
        [Required]
        public string NomeCategoria { get; set; }
    }
}
