using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
using ProyectoCentinela.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCentinela.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SesionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SesionController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve todas las sesiones con el nombre del usuario asociado.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSesiones()
        {
            var sesiones = await _context.Sesiones
                .Include(s => s.Usuario)
                .AsNoTracking() // IMPORTANTE: evita cacheo en memoria
                .OrderByDescending(s => s.UltimaConexion)
                .Select(s => new
                {
                    s.Id,
                    UsuarioId = s.UsuarioId,
                    Usuario = s.Usuario.NombreUsuario,
                    s.UltimaConexion,
                    s.Ip
                })
                .ToListAsync();

            return Ok(sesiones);
        }


        /// <summary>
        /// Elimina una sesión por su ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSesion(int id)
        {
            var sesion = await _context.Sesiones.FindAsync(id);

            if (sesion == null)
                return NotFound(new { mensaje = "Sesión no encontrada" });

            _context.Sesiones.Remove(sesion);
            await _context.SaveChangesAsync();

            var maxId = await _context.Sesiones.MaxAsync(s => (int?)s.Id) ?? 0;
            maxId++;
            await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE sesion AUTO_INCREMENT = {maxId}");

            return Ok(new { mensaje = "Sesión eliminada correctamente" });
        }
    }
}