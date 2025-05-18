using System.ComponentModel.DataAnnotations;

namespace ProyectoCentinela.DTOs
{
    public class UsuarioRegisterDTO
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ContrasenaHash { get; set; }
    }
}