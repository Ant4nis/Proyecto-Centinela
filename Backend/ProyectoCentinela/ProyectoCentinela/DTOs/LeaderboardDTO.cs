namespace ProyectoCentinela.DTOs
{
    /// <summary>
    /// DTO para crear nuevas entradas en el leaderboard.
    /// </summary>
    public class LeaderboardDTO
    {
        public int UsuarioId { get; set; }
        public int Puntuacion { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}