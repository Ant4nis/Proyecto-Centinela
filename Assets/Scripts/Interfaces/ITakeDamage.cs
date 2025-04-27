namespace Interfaces
{
    /// <summary>
    /// Interfaz para entidades que pueden recibir daño o curarse.
    /// Cualquier GO que la implemente puede recibirlo y s ser destruido.
    ///
    /// ¿Qué ofrece de especial?
    ///  Si por ejemplo un proyectil golpea algo. No necesita saber qué
    ///  ha golpeado, solo observa si lo que golpea implementa la interfaz, 
    ///  si la implementa aplica el daño.
    /// 
    /// Implementa:
    /// 1. Tomar daño con TakeDamage.
    /// 2. Restaurar salud con RestoreHealth.
    /// </summary>
    public interface ITakeDamage
    {
        /// <summary>
        /// Aplica daño a la entidad.
        /// </summary>
        /// <param name="damage">Cantidad de daño a aplicar.</param>
        public void TakeDamage(float damage);

        /// <summary>
        /// Restaura salud a la entidad.
        /// </summary>
        /// <param name="health">Cantidad de salud a restaurar.</param>
        public void RestoreHealth(float health);
    }
}