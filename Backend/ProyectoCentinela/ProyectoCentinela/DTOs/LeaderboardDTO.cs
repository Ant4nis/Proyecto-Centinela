namespace ProyectoCentinela.DTOs
{
    /// <summary>
    /// DTO para la transferencia de datos de Leaderboard
    /// </summary>
    public class LeaderboardDTO
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public string Nivel { get; set; }
        public DateTime Fecha { get; set; }
    }
}