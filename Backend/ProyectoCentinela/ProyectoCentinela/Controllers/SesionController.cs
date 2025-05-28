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
        /// Devuelve todas las sesiones, incluyendo usuarios que nunca iniciaron sesión.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllSesiones()
        {
            // ✅ Intentamos obtener el ID del usuario actual desde cabecera, pero sin petar si no existe
            string idStr = Request.Headers["UsuarioActualId"];
            int idUsuarioActual = 0;
            if (!string.IsNullOrEmpty(idStr))
                int.TryParse(idStr, out idUsuarioActual);

            // ✅ Cargamos todos los usuarios junto con sus sesiones (puedan tener 0 o más)
            var sesiones = await _context.Usuarios
                .Include(u => u.Sesiones)
                .AsNoTracking()
                .Select(u => new
                {
                    Id = u.Sesiones.OrderByDescending(s => s.UltimaConexion).Select(s => s.Id).FirstOrDefault(),
                    UsuarioId = u.Id,
                    Usuario = u.NombreUsuario,
                    UltimaConexion = u.Sesiones.Any()
                        ? u.Sesiones.OrderByDescending(s => s.UltimaConexion).First().UltimaConexion.ToString("dd/MM/yyyy HH:mm")
                        : "-",
                    Ip = u.Sesiones.Any()
                        ? u.Sesiones.OrderByDescending(s => s.UltimaConexion).First().Ip
                        : "-",
                    EsUsuarioActual = u.Id == idUsuarioActual,
                    TieneSesion = u.Sesiones.Any()
                })
                .ToListAsync();

            //  Orden final:
            // 1. Usuario actual
            // 2. Usuarios con sesión
            // 3. Usuarios sin sesión
            // Todo ordenado alfabéticamente dentro de cada grupo
            var ordenados = sesiones
                .OrderByDescending(s => s.EsUsuarioActual)
                .ThenByDescending(s => s.TieneSesion)
                .ThenBy(s => s.Usuario)
                .ToList();

            return Ok(ordenados);
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