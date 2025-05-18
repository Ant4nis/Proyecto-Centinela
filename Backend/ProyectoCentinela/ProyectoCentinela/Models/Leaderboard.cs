using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoCentinela.Models
{
    /// <summary>
    /// Representa una entrada en el leaderboard del juego.
    /// </summary>
    [Table("leaderboard")]
    public class Leaderboard
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required]
        [Column("puntuacion")]
        public int Puntuacion { get; set; }

        [Column("nivel")]
        [MaxLength(50)]
        public string Nivel { get; set; } = string.Empty;

        [Required]
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}