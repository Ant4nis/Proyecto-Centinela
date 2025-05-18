using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCentinela.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaderboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaderboard(int id)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(id);

            if (leaderboard == null)
                return NotFound(new { mensaje = "Registro en Leaderboard no encontrado" });

            // ✅ Eliminar el registro de leaderboard
            _context.Leaderboards.Remove(leaderboard);
            await _context.SaveChangesAsync();

            // ✅ Ajustar el valor del AUTO_INCREMENT
            var maxId = await _context.Leaderboards.MaxAsync(l => (int?)l.Id) ?? 0;
            maxId++; // Siguiente ID disponible
            await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE leaderboard AUTO_INCREMENT = {maxId}");

            return Ok(new { mensaje = "Registro en Leaderboard eliminado correctamente" });
        }
    }
}