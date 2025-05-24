using Microsoft.EntityFrameworkCore;
using PlataformaNoticias.Models;

namespace PlataformaNoticias.Data
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext(DbContextOptions<FeedbackContext> options) : base(options) { }

        public DbSet<Feedback> Feedbacks { get; set; } = null!;
    }
}
