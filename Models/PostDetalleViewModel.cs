using System.Collections.Generic;

namespace PlataformaNoticias.Models
{
    public class PostDetalleViewModel
    {
        public Post Post { get; set; } = new();
        public User? Autor { get; set; }
        public List<Comment> Comentarios { get; set; } = new();
    }
}
