
﻿using System.ComponentModel.DataAnnotations;

﻿namespace plataforma_videos_api.Models.Request
{
    public class CategoriaRequest
    {
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Cor { get; set; }
    }
}
