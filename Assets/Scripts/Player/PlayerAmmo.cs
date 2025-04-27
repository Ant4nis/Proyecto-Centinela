using ScriptableObjects;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Componente encargado de gestionar la munición del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Controla el gasto de munición al disparar.
    /// 2. Permite la recuperación de munición.
    /// 3. Proporciona información sobre si el jugador tiene munición disponible.
    /// </summary>
    public class PlayerAmmo : MonoBehaviour
    {
        [Header("Configuración de Munición")]
        [Tooltip("Configuración que almacena los valores actuales y máximos de munición del jugador.")]
        [SerializeField] private PlayerConfiguration playerConfiguration;

        /// <summary>Indica si el jugador dispone de munición actualmente.</summary>
        public bool HaveAmmo => playerConfiguration.CurrentAmmo > 0f;

        /// <summary>
        /// Reduce la cantidad de munición del jugador en la cantidad indicada.
        /// Si el valor resultante es negativo, se ajusta automáticamente a cero.
        /// </summary>
        /// <param name="ammo">Cantidad de munición a consumir.</param>
        public void SpendAmmo(float ammo)
        {
            playerConfiguration.CurrentAmmo -= ammo;

            if (playerConfiguration.CurrentAmmo < 0f)
            {
                playerConfiguration.CurrentAmmo = 0f;
            }
        }

        /// <summary>
        /// Aumenta la cantidad de munición del jugador en la cantidad indicada.
        /// Si la cantidad supera el máximo permitido, se ajusta automáticamente al máximo.
        /// </summary>
        /// <param name="ammo">Cantidad de munición a recuperar.</param>
        public void RecoverAmmo(float ammo)
        {
            playerConfiguration.CurrentAmmo += ammo;

            if (playerConfiguration.CurrentAmmo > playerConfiguration.MaxAmmo)
            {
                playerConfiguration.CurrentAmmo = playerConfiguration.MaxAmmo;
            }
        }
    }
}
