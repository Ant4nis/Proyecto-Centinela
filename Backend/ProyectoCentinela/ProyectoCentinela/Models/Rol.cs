using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCentinela.Models
{
    /// <summary>
    /// Representa un rol en el sistema (administrador o jugador).
    /// </summary>
    [Table("rol")]
    public class Rol
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        /// <summary>
        /// Relación con los usuarios que tienen este rol.
        /// </summary>
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}