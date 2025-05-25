using UnityEngine;

namespace UIForms
{
    /// <summary>
    /// Acciones del menú principal que requieren transición de escena o salida del juego.
    /// Aplica un fade out antes de realizar la acción correspondiente.
    ///
    /// FUNCIONALIDADES:
    /// 1. Iniciar la escena del juego desde botón "Jugar".
    /// 2. Iniciar la escena de modo competitivo.
    /// 3. Salir del juego con fade out visual.
    /// </summary>
    public class MainMenuActions : MonoBehaviour
    {
        [Header("Nombres de escenas")]
        [Tooltip("Nombre exacto de la escena principal del juego (se carga al pulsar Jugar)")]
        [SerializeField] private string gameSceneName;

        [Tooltip("Nombre exacto de la escena competitiva (se carga al pulsar Competitivo)")]
        [SerializeField] private string competitiveSceneName;

        [Header("Duración antes de salir")]
        [Tooltip("Tiempo en segundos tras el fade antes de cerrar el juego")]
        [SerializeField] private float quitDelay = 1.2f;


        /// <summary>
        /// Inicia el fade out y carga la escena del juego.
        /// </summary>
        public void PlayGame()
        {
            if (!string.IsNullOrEmpty(gameSceneName))
            {
                Managers.FadeController.Instance.FadeOutAndLoadScene(gameSceneName);
            }
            else
            {
                Debug.LogWarning("No se ha asignado el nombre de la escena del juego.");
            }
        }

        /// <summary>
        /// Inicia el fade out y carga la escena del modo competitivo.
        /// </summary>
        public void PlayCompetitive()
        {
            if (!string.IsNullOrEmpty(competitiveSceneName))
            {
                Managers.FadeController.Instance.FadeOutAndLoadScene(competitiveSceneName);
            }
            else
            {
                Debug.LogWarning("No se ha asignado el nombre de la escena competitiva.");
            }
        }

        

        /// <summary>
        /// Inicia un fade out y después cierra el juego.
        /// </summary>
        public void QuitGame()
        {
            StartCoroutine(QuitWithFade());
        }

        /// <summary>
        /// Corrutina que espera tras el fade antes de salir.
        /// </summary>
        private System.Collections.IEnumerator QuitWithFade()
        {
            Managers.FadeController.Instance.FadeOut();
            yield return new WaitForSeconds(quitDelay);
            Application.Quit();
            Debug.Log("Aplicación cerrada.");
        }
    }
}
