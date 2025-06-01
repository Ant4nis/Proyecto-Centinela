using System.Collections;
using Player;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Gestiona el proceso tras la muerte del jugador: mensaje, bloqueo de controles, reinicio de escena y restauración de stats.
    /// </summary>
    public class DeathHandler : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Texto que se muestra al morir el jugador.")]
        [SerializeField] private TMP_Text deathMessage;

        [Tooltip("Segundos que se espera antes de reiniciar la escena.")]
        [SerializeField] private float restartDelay = 3f;

        [Tooltip("Referencia al ScriptableObject de configuración del jugador.")]
        [SerializeField] private PlayerConfiguration playerConfig;

        [Tooltip("Componente PlayerInput que se desactiva al morir.")]
        [SerializeField] private PlayerInputReader playerInput;
        [SerializeField] private Rigidbody2D rb;

        [Tooltip("Nombre o índice de la escena inicial.")]
        [SerializeField] private string sceneToLoad = "MainMenu";

        /// <summary>
        /// Llama este método cuando el jugador muere.
        /// </summary>
        public void HandleDeath()
        {
            StartCoroutine(IEHandleDeath());
        }

        private IEnumerator IEHandleDeath()
        {
            if (deathMessage != null)
            {
                deathMessage.gameObject.SetActive(true);
                yield return StartCoroutine(FadeInText(deathMessage, 1f)); // ← velocidad del fade
            }

            // Desactivar controles
            if (playerInput != null)
                playerInput.enabled = false;
            if (rb != null)
                rb.linearVelocity = Vector2.zero;


            // Esperar
            yield return new WaitForSeconds(restartDelay);

            // Restaurar stats
            if (playerConfig != null)
            {
                playerConfig.CurrentHealth = playerConfig.MaxHealth;
                playerConfig.CurrentArmor = playerConfig.MaxArmor;
                playerConfig.CurrentAmmo = playerConfig.MaxAmmo;
            }

            // Volver al inicio
            SceneManager.LoadScene(sceneToLoad);
        }
        
        private IEnumerator FadeInText(TMP_Text text, float duration)
        {
            Color originalColor = text.color;
            float timer = 0f;

            // Forzar alpha = 0 al empezar
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

            while (timer < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, timer / duration);
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }

            // Asegura que acabe completamente visible
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }

    }
}
