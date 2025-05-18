using ScriptableObjects;
using UnityEngine;
using System.Collections;

namespace Player
{
    /// <summary>
    /// Componente encargado de gestionar la munición del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Controla el gasto de munición al disparar.
    /// 2. Permite la recuperación de munición.
    /// 3. Proporciona información sobre si el jugador tiene munición disponible.
    /// 4. Regenera munición de forma progresiva con el tiempo.
    /// </summary>
    public class PlayerAmmo : MonoBehaviour
    {
        [Header("Configuración de Munición")]
        [Tooltip("Configuración que almacena los valores actuales y máximos de munición del jugador.")]
        [SerializeField] private PlayerConfiguration playerConfiguration;

        [Header("Regeneración de Munición")]
        [Tooltip("Cantidad de munición que se regenera por segundo.")]
        [SerializeField] private float regenerationAmount = 1f;
        
        [Tooltip("Intervalo de tiempo (en segundos) entre cada regeneración.")]
        [SerializeField] private float regenerationInterval = 0.5f;

        private Coroutine _regenerationRoutine;

        /// <summary>Indica si el jugador dispone de munición actualmente.</summary>
        public bool HaveAmmo => playerConfiguration.CurrentAmmo > 0f;

        private void OnEnable()
        {
            StartRegeneration();
        }

        private void OnDisable()
        {
            StopRegeneration();
        }

        /// <summary>
        /// Inicia la regeneración progresiva de munición.
        /// </summary>
        public void StartRegeneration()
        {
            if (_regenerationRoutine == null)
            {
                _regenerationRoutine = StartCoroutine(RegenerateAmmo());
            }
        }

        /// <summary>
        /// Detiene la regeneración progresiva de munición.
        /// </summary>
        public void StopRegeneration()
        {
            if (_regenerationRoutine != null)
            {
                StopCoroutine(_regenerationRoutine);
                _regenerationRoutine = null;
            }
        }

        /// <summary>
        /// Corrutina que regenera munición de manera constante.
        /// </summary>
        private IEnumerator RegenerateAmmo()
        {
            while (true)
            {
                yield return new WaitForSeconds(regenerationInterval);

                if (playerConfiguration.CurrentAmmo < playerConfiguration.MaxAmmo)
                {
                    RecoverAmmo(regenerationAmount);
                }
            }
        }

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
