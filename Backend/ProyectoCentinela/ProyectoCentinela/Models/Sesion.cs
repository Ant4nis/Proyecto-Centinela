using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCentinela.Models
{
    /// <summary>
    /// Representa una sesión de inicio de sesión de un usuario.
    /// Almacena información como fecha, IP y dispositivo.
    /// </summary>
    [Table("sesion")]
    public class Sesion
    {
        /// <summary>
        /// Identificador único de la sesión (clave primaria).
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// ID del usuario asociado a la sesión (clave foránea).
        /// </summary>
        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Última conexión registrada por el usuario.
        /// </summary>
        [Required]
        [Column("ultima_conexion")]
        public DateTime UltimaConexion { get; set; }

        /// <summary>
        /// Dirección IP desde donde se conectó el usuario.
        /// </summary>
        [MaxLength(45)]
        [Column("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// Relación con el usuario para navegación.
        /// </summary>
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
    }
}