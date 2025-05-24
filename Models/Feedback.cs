using System;
using System.ComponentModel.DataAnnotations;

namespace PlataformaNoticias.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        [RegularExpression("like|dislike", ErrorMessage = "Sentimiento debe ser 'like' o 'dislike'")]
        public string Sentimiento { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; }
    }
}
//cambio3
