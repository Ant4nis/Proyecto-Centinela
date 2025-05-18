namespace ProyectoCentinela.DTOs
{
    /// <summary>
    /// DTO para la transferencia de datos de Sesion
    /// </summary>
    public class SesionDTO
    {
        public int Id { get; set; }
        public DateTime UltimaConexion { get; set; }
        public string Ip { get; set; }
    }
}