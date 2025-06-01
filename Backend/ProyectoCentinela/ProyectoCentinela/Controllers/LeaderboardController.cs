using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
using Microsoft.EntityFrameworkCore;
using ProyectoCentinela.DTOs;

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

        /// <summary>
        /// Devuelve una entrada por usuario (aunque no tenga puntuación registrada).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLeaderboardCompleto()
        {
            var leaderboard = await _context.Usuarios
                .Include(u => u.Rol)
                .GroupJoin(
                    _context.Leaderboards,
                    usuario => usuario.Id,
                    lb => lb.UsuarioId,
                    (usuario, puntuaciones) => new
                    {
                        Usuario = usuario,
                        Entrada = puntuaciones.OrderByDescending(p => p.Fecha).FirstOrDefault()
                    }
                )
                .Select(x => new
                {
                    Id = x.Usuario.Id,
                    NombreUsuario = x.Usuario.NombreUsuario,
                    Rol = x.Usuario.Rol.Nombre,
                    Puntuacion = x.Entrada != null ? x.Entrada.Puntuacion : 0,
                    Nivel = x.Entrada != null ? x.Entrada.Nivel : "Sin datos",
                    Fecha = x.Entrada != null ? x.Entrada.Fecha : DateTime.MinValue
                })
                .ToListAsync();

            return Ok(leaderboard);
        }

        /// <summary>
        /// Elimina una entrada de leaderboard por ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaderboard(int id)
        {
            var leaderboard = await _context.Leaderboards.FindAsync(id);

            if (leaderboard == null)
                return NotFound(new { mensaje = "Registro en Leaderboard no encontrado" });

            _context.Leaderboards.Remove(leaderboard);
            await _context.SaveChangesAsync();

            var maxId = await _context.Leaderboards.MaxAsync(l => (int?)l.Id) ?? 0;
            maxId++;
            await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE leaderboard AUTO_INCREMENT = {maxId}");

            return Ok(new { mensaje = "Registro en Leaderboard eliminado correctamente" });
        }
        
        /// <summary>
        /// Crea una nueva entrada en el leaderboard.
        /// </summary>
        /// <summary>
        /// Crea una nueva entrada en el leaderboard.
        /// Suma la puntuación a la última registrada para ese usuario.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostLeaderboard([FromBody] LeaderboardDTO entradaDTO)
        {
            var usuario = await _context.Usuarios.FindAsync(entradaDTO.UsuarioId);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            var ultimaEntrada = await _context.Leaderboards
                .Where(l => l.UsuarioId == entradaDTO.UsuarioId)
                .OrderByDescending(l => l.Fecha)
                .FirstOrDefaultAsync();

            int puntuacionTotal = entradaDTO.Puntuacion;
            if (ultimaEntrada != null)
            {
                puntuacionTotal += ultimaEntrada.Puntuacion;
            }

            var nuevaEntrada = new Leaderboard
            {
                UsuarioId = entradaDTO.UsuarioId,
                Puntuacion = puntuacionTotal,
                Nivel = entradaDTO.Nivel,
                Fecha = entradaDTO.Fecha
            };

            _context.Leaderboards.Add(nuevaEntrada);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Puntuación añadida correctamente", entrada = nuevaEntrada });
        }

    }
}
