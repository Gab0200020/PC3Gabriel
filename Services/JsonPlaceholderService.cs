using PlataformaNoticias.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlataformaNoticias.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        // Caché en memoria para evitar llamadas repetidas
        private List<Post>? _cachePosts;
        private readonly Dictionary<int, Post> _cachePostById = new();
        private readonly Dictionary<int, User> _cacheUsuarios = new();
        private readonly Dictionary<int, List<Comment>> _cacheComentarios = new();

        public JsonPlaceholderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Post>?> ObtenerPostsAsync()
        {
            if (_cachePosts != null)
                return _cachePosts;

            try
            {
                var response = await _httpClient.GetAsync("posts");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                _cachePosts = JsonSerializer.Deserialize<List<Post>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return _cachePosts;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Post?> ObtenerPostPorIdAsync(int id)
        {
            if (_cachePostById.ContainsKey(id))
                return _cachePostById[id];

            try
            {
                var response = await _httpClient.GetAsync($"posts/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var post = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (post != null)
                    _cachePostById[id] = post;
                return post;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> ObtenerUsuarioPorIdAsync(int userId)
        {
            if (_cacheUsuarios.ContainsKey(userId))
                return _cacheUsuarios[userId];

            try
            {
                var response = await _httpClient.GetAsync($"users/{userId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (user != null)
                    _cacheUsuarios[userId] = user;
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Comment>?> ObtenerComentariosPorPostIdAsync(int postId)
        {
            if (_cacheComentarios.ContainsKey(postId))
                return _cacheComentarios[postId];

            try
            {
                var response = await _httpClient.GetAsync($"comments?postId={postId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var comentarios = JsonSerializer.Deserialize<List<Comment>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (comentarios != null)
                    _cacheComentarios[postId] = comentarios;
                return comentarios;
            }
            catch
            {
                return null;
            }
        }
    }
}
