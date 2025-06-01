/// <summary>
/// DTO que representa una nueva entrada para el leaderboard.
/// </summary>
[System.Serializable]
public class LeaderboardCreateDTO
{
    public int usuarioId;
    public int puntuacion;
    public string nivel;
    public string fecha;

    public LeaderboardCreateDTO(int usuarioId, int puntuacion, string nivel)
    {
        this.usuarioId = usuarioId;
        this.puntuacion = puntuacion;
        this.nivel = nivel;
        this.fecha = System.DateTime.Now.ToString("yyyy-MM-dd");
    }
}