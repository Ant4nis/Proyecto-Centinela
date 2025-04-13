using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// ScriptableObject que almacena la configuración y estadísticas del jugador.
    /// 
    /// Funciones:
    /// 1. Define datos básicos del jugador (nombre e icono).
    /// 2. Establece el nivel del jugador.
    /// 3. Almacena las estadísticas esenciales, incluyendo salud, armadura, munición y parámetros críticos.
    /// </summary>
    [CreateAssetMenu(fileName = "Player_", menuName = "Player/Configuración")]
    public class PlayerConfiguration : ScriptableObject
    {
        [Header("Datos")] 
        [Tooltip("Nombre del jugador")]
        public string Name;
        [Tooltip("Icono representativo del jugador")]
        public Sprite Icon;
        
        [Header("Nivel")]
        [Tooltip("Nivel actual del jugador")]
        public int Level;
        
        [Header("Stats")]
        [Tooltip("Salud actual del jugador")]
        public float CurrentHealth;
        [Tooltip("Salud máxima del jugador")]
        public float MaxHealth;
        [Tooltip("Armadura actual del jugador")]
        public float CurrentArmor;
        [Tooltip("Armadura máxima del jugador")]
        public float MaxArmor;
        [Tooltip("Munición actual del jugador")]
        public float CurrentAmmo;
        [Tooltip("Munición máxima del jugador")]
        public float MaxAmmo;
        [Tooltip("Probabilidad de realizar un golpe crítico")]
        public float CriticalChance;
        [Tooltip("Daño causado por un golpe crítico")]
        public float CriticalDamage;
    }
}