using UnityEngine;
using ScriptableObjects;
using Interfaces;

namespace Player
{
    /// <summary>
    /// Componente responsable de manejar la salud del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Aplicar daño al jugador, reduciendo primero la armadura y luego la salud.
    /// 2. Restaurar la salud hasta alcanzar el valor máximo configurado.
    /// 3. Restaurar la armadura hasta alcanzar el valor máximo configurado.
    /// 4. Ejecutar la lógica de manejo de la muerte del jugador.
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [Header("Configuración del Jugador (Scriptable Object)"), Tooltip("Configuración del jugador que contiene los valores de salud y armadura.")]
        [SerializeField] private PlayerConfiguration playerConfig;

        /// <summary>
        /// Método de prueba para simular la recepción de daño y la restauración de salud y armadura.
        /// NOTA: Se utiliza el sistema de input legacy para propósitos de testing.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                TakeDamage(1f);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                RestoreHealth(1f);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                RestoreArmor(1f);
            }
        }

        /// <summary>
        /// Restaura la salud del jugador hasta alcanzar el máximo permitido.
        /// </summary>
        /// <param name="health">Cantidad de salud a restaurar.</param>
        public void RestoreHealth(float health)
        {
            playerConfig.CurrentHealth += health;
            if (playerConfig.CurrentHealth > playerConfig.MaxHealth)
            {
                playerConfig.CurrentHealth = playerConfig.MaxHealth;
            }
        }

        /// <summary>
        /// Restaura la armadura del jugador hasta alcanzar el valor máximo permitido.
        /// </summary>
        /// <param name="armor">Cantidad de armadura a restaurar.</param>
        public void RestoreArmor(float armor)
        {
            playerConfig.CurrentArmor += armor;
            if (playerConfig.CurrentArmor > playerConfig.MaxArmor)
            {
                playerConfig.CurrentArmor = playerConfig.MaxArmor;
            }
        }
        
        /// <summary>
        /// Aplica daño al jugador, reduciendo primero la armadura y, si es necesario, la salud.
        /// Si la salud llega a cero, se ejecuta la lógica de muerte del jugador.
        /// </summary>
        /// <param name="damage">Cantidad de daño a aplicar.</param>
        public void TakeDamage(float damage)
        {
            if (playerConfig.CurrentArmor > 0)
            {
                float remainingDamage = damage - playerConfig.CurrentArmor;
                playerConfig.CurrentArmor = Mathf.Max(playerConfig.CurrentArmor - damage, 0);

                if (remainingDamage > 0)
                {
                    playerConfig.CurrentHealth = Mathf.Max(playerConfig.CurrentHealth - remainingDamage, 0);
                }
            }
            else
            {
                playerConfig.CurrentHealth = Mathf.Max(playerConfig.CurrentHealth - damage, 0);
            }

            if (playerConfig.CurrentHealth <= 0)
            {
                HandlePlayerDeath();
            }
        }
        
        /// <summary>
        /// Maneja la lógica a ejecutar cuando la salud del jugador llega a cero.
        /// Por ejemplo: activar animaciones de muerte, notificar al GameManager, mostrar la pantalla de Game Over, etc.
        /// </summary>
        private void HandlePlayerDeath()
        {
            // TODO: Implementar la lógica de muerte del jugador (activar animación, notificar GameManager, etc.)
        }
    }
}
