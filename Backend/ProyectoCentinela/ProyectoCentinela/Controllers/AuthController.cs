using Microsoft.AspNetCore.Mvc;
using ProyectoCentinela.Data;
using ProyectoCentinela.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProyectoCentinela.DTOs;

namespace ProyectoCentinela.Controllers
{
    /// <summary>
    /// Controlador para registro y autenticación de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra un nuevo usuario si no existe previamente.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRegisterDTO nuevoUsuario, [FromQuery] int rolId)
        {
            // Comprobar si el nombre de usuario o email ya existen
            bool nombreExiste = await _context.Usuarios.AnyAsync(u => u.NombreUsuario == nuevoUsuario.NombreUsuario);
            bool emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == nuevoUsuario.Email);
        
            if (nombreExiste || emailExiste)
            {
                return BadRequest(new { mensaje = "El nombre de usuario o el email ya están en uso." });
            }
        
            // ✅ Asignación del rol basado en el ID
            var rol = await _context.Roles.FindAsync(rolId);
        
            if (rol == null)
            {
                return BadRequest(new { mensaje = "El rol seleccionado no existe en la base de datos." });
            }
        
            // ✅ Crear la instancia del usuario
            var usuario = new Usuario
            {
                NombreUsuario = nuevoUsuario.NombreUsuario,
                Email = nuevoUsuario.Email,
                ContrasenaHash = nuevoUsuario.ContrasenaHash,
                FechaRegistro = DateTime.Now,
                RolId = rol.Id
            };
        
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
        
                    if (rol.Nombre.ToLower() == "jugador")
                    {
                        var entradaLeaderboard = new Leaderboard
                        {
                            UsuarioId = usuario.Id,
                            Puntuacion = 0,
                            Nivel = string.Empty,
                            Fecha = DateTime.Now
                        };
        
                        _context.Leaderboards.Add(entradaLeaderboard);
                        await _context.SaveChangesAsync();
                    }
        
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { mensaje = "Error en el registro del usuario", detalle = ex.Message });
                }
            }
        
            return Ok(new { mensaje = "Usuario registrado correctamente.", rol = rol.Nombre });
        }

    }
}
