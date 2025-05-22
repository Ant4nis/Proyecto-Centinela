using System.ComponentModel.DataAnnotations;

namespace ProyectoCentinela.DTOs
{
    /// <summary>
    /// DTO para el login de usuario (email y contraseña).
    /// </summary>
    public class UsuarioLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
}