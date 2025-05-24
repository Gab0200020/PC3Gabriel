using Microsoft.AspNetCore.Mvc;
using PlataformaNoticias.Models;
using PlataformaNoticias.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PlataformaNoticias.Controllers
{
    public class PostMvcController : Controller
    {
        private readonly IJsonPlaceholderService _jsonService;
        private readonly HttpClient _localApiClient;

        public PostMvcController(IJsonPlaceholderService jsonService, IHttpClientFactory httpClientFactory)
        {
            _jsonService = jsonService;
            _localApiClient = httpClientFactory.CreateClient("LocalApi");
        }

        // GET: /PostMvc/Listar
        public async Task<IActionResult> Listar()
        {
            var posts = await _jsonService.ObtenerPostsAsync();
            if (posts == null)
            {
                ViewBag.Error = "No se pudieron cargar los posts. Intenta más tarde.";
                posts = new List<Post>();
            }
            return View(posts);
        }

        // GET: /PostMvc/Detalle/{id}
        public async Task<IActionResult> Detalle(int id)
        {
            var post = await _jsonService.ObtenerPostPorIdAsync(id);
            if (post == null) return NotFound();

            var autor = await _jsonService.ObtenerUsuarioPorIdAsync(post.UserId);
            var comentarios = await _jsonService.ObtenerComentariosPorPostIdAsync(post.Id);

            var modelo = new PostDetalleViewModel
            {
                Post = post,
                Autor = autor,
                Comentarios = comentarios ?? new List<Comment>()
            };

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(modelo);
        }

        // POST: /PostMvc/EnviarFeedback
        [HttpPost]
        public async Task<IActionResult> EnviarFeedback(int postId, string sentimiento)
        {
            if (sentimiento != "like" && sentimiento != "dislike")
                return BadRequest("Sentimiento inválido.");

            var feedback = new Feedback
            {
                PostId = postId,
                Sentimiento = sentimiento,
                Fecha = System.DateTime.UtcNow
            };

            var response = await _localApiClient.PostAsJsonAsync("api/feedback", feedback);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Detalle", new { id = postId });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                TempData["Error"] = "Ya has enviado feedback para este post.";
                return RedirectToAction("Detalle", new { id = postId });
            }
            else
            {
                TempData["Error"] = "Error al enviar feedback.";
                return RedirectToAction("Detalle", new { id = postId });
            }
        }
    }
}
//MVC