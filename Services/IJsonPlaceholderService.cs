using PlataformaNoticias.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaNoticias.Services
{
    public interface IJsonPlaceholderService
    {
        Task<List<Post>?> ObtenerPostsAsync();
        Task<Post?> ObtenerPostPorIdAsync(int id);
        Task<User?> ObtenerUsuarioPorIdAsync(int userId);
        Task<List<Comment>?> ObtenerComentariosPorPostIdAsync(int postId);
    }
}
