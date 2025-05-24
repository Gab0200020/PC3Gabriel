using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaNoticias.Data;
using PlataformaNoticias.Models;
using System.Threading.Tasks;

namespace PlataformaNoticias.Controllers.Api
{
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackContext _context;

        public FeedbackController(FeedbackContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CrearFeedback([FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool existe = await _context.Feedbacks.AnyAsync(f => f.PostId == feedback.PostId);
            if (existe)
                return Conflict("Ya existe feedback para este post.");

            feedback.Fecha = System.DateTime.UtcNow;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(feedback);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFeedbacks()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();
            return Ok(feedbacks);
        }
    }
}
//cambios