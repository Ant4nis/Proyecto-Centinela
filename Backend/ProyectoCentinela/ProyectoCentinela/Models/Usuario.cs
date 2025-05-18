using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCentinela.Models
{
    /// <summary>
    /// Representa a un usuario registrado en el sistema.
    /// Incluye sus datos de acceso, rol y relación con las sesiones iniciadas.
    /// </summary>
    [Table("usuarios")] // Especifica el nombre real de la tabla en la base de datos.
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// Clave primaria con incremento automático.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de usuario elegido por el cliente.
        /// Es único y obligatorio, con un máximo de 50 caracteres.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Dirección de correo electrónico asociada al usuario.
        /// Campo obligatorio y único, máximo 100 caracteres.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        /// <summary>
        /// Contraseña encriptada mediante algoritmo hash.
        /// Campo sensible, obligatorio, hasta 255 caracteres.
        /// </summary>
        [Required]
        [MaxLength(255)]
        [Column("contraseña_hash")]
        public string ContrasenaHash { get; set; }

        /// <summary>
        /// Fecha en la que se registró el usuario.
        /// </summary>
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        /// <summary>
        /// Identificador del rol del usuario (Clave foránea).
        /// </summary>
        [Required]
        [Column("rol_id")]
        public int RolId { get; set; }

        /// <summary>
        /// Relación con la entidad Rol.
        /// </summary>
        [ForeignKey("RolId")]
        public virtual Rol Rol { get; set; }

        /// <summary>
        /// Relación con la entidad Sesión.
        /// </summary>
        public ICollection<Sesion> Sesiones { get; set; }

        /// <summary>
        /// Relación con la entidad Leaderboard.
        /// </summary>
        public ICollection<Leaderboard> Leaderboards { get; set; }
    
    }
}
