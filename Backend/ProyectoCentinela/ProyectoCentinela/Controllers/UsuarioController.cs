using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
using Microsoft.EntityFrameworkCore;
using ProyectoCentinela.DTOs;

namespace ProyectoCentinela.Controllers
{
    /// <summary>
    /// Controlador para gestionar los usuarios del sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtener todos los usuarios.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sesiones)
                .Include(u => u.Leaderboards)
                .Select(u => new
                {
                    u.Id,
                    u.NombreUsuario,
                    u.Email,
                    u.FechaRegistro,
                    u.RolId,
                    Rol = new
                    {
                        u.Rol.Id,
                        u.Rol.Nombre
                    },
                    Sesiones = u.Sesiones.Select(s => new
                    {
                        s.Id,
                        s.UltimaConexion,
                        s.Ip
                    }),
                    Leaderboards = u.Leaderboards.Select(l => new
                    {
                        l.Id,
                        l.Puntuacion,
                        l.Nivel,
                        l.Fecha
                    })
                })
                .ToListAsync();

            return Ok(usuarios);
        }


        /// <summary>
        /// Obtener un usuario por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sesiones)
                .Include(u => u.Leaderboards)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }

        /// <summary>
        /// Crear un nuevo usuario.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario nuevoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }
        
        /// <summary>
        /// Actualizar un usuario existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] DTOs.UsuarioUpdateDTO usuarioActualizado)
        {
            if (id != usuarioActualizado.Id)
            {
                return BadRequest(new { mensaje = "El ID del usuario no coincide." });
            }

            //  Obtenemos el usuario existente
            var usuarioExistente = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Sesiones)
                .Include(u => u.Leaderboards)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuarioExistente == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            //  Actualizamos los datos básicos
            usuarioExistente.NombreUsuario = usuarioActualizado.NombreUsuario;
            usuarioExistente.Email = usuarioActualizado.Email;
            usuarioExistente.ContrasenaHash = usuarioActualizado.ContrasenaHash;
            usuarioExistente.RolId = usuarioActualizado.RolId;

            // las relaciones para que Swagger no se queje
            usuarioExistente.Sesiones = new List<Sesion>();
            usuarioExistente.Leaderboards = new List<Leaderboard>();

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Usuario actualizado correctamente." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                throw;
            }
        }


        /// <summary>
        /// Eliminar un usuario por su ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // ✅ 1️⃣ Eliminar el usuario
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            // ✅ 2️⃣ Ajustar el valor del AUTO_INCREMENT
            // Esto actualiza el contador para que el próximo ID sea el correcto.
            var maxId = await _context.Usuarios.MaxAsync(u => (int?)u.Id) ?? 0;
            maxId++; // Siguiente ID disponible
            await _context.Database.ExecuteSqlRawAsync($"ALTER TABLE usuario AUTO_INCREMENT = {maxId}");

            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }
    }
}
