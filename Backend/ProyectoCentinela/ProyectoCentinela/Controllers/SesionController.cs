using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSesion(int id)
        {
            var sesion = await _context.Sesiones.FindAsync(id);

            if (sesion == null)
                return NotFound(new { mensaje = "Sesión no encontrada" });

            // ✅ Eliminar la sesión
            _context.Sesiones.Remove(sesion);
            await _context.SaveChangesAsync();

            // ✅ Ajustar el valor del AUTO_INCREMENT
            var maxId = await _context.Sesiones.MaxAsync(s => (int?)s.Id) ?? 0;
            maxId++; // Siguiente ID disponible
            await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE sesion AUTO_INCREMENT = {maxId}");

            return Ok(new { mensaje = "Sesión eliminada correctamente" });
        }
    }
}