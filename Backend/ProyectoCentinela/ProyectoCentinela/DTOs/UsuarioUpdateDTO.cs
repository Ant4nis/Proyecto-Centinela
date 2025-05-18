using System.ComponentModel.DataAnnotations;

namespace ProyectoCentinela.DTOs
{
    /// <summary>
    /// DTO para la actualización de un usuario.
    /// </summary>
    public class UsuarioUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContrasenaHash { get; set; }

        [Required]
        public int RolId { get; set; }
    }
}