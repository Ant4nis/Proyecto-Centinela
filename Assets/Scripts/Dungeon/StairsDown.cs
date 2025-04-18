using System;
using UnityEngine;

namespace Dungeon
{
    /// <summary>
    /// Componente que detecta si el jugador entra en el portal de bajada (escalera).
    /// 
    /// Funcionalidades:
    /// 1. Detecta colisiones con el jugador mediante trigger.
    /// 2. Lanza un evento global para notificar que se ha activado el portal.
    /// </summary>
    public class StairsDown : MonoBehaviour
    {
        /// <summary>
        /// Evento estático que se lanza cuando el jugador entra en el trigger de la escalera.
        /// Otros sistemas (como LevelManager) pueden suscribirse para reaccionar al cambio de nivel.
        /// </summary>
        public static event Action PortalEvent;

        /// <summary>
        /// Detecta la entrada de un collider. Si es el jugador, se lanza el evento del portal.
        /// </summary>
        /// <param name="other">Collider que entra al trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PortalEvent?.Invoke();
            }
        }
    }
}