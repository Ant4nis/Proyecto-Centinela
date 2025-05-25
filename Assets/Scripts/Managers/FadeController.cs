using System.Collections;
using Extra;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Controlador centralizado para hacer transiciones de opacidad con fade in/out.
    /// 1. Ejecuta automáticamente un fade in al cargar la escena.
    /// 2. Permite ejecutar un fade out y cargar una escena nueva.
    /// 3. Usa Helper.IEFade para interpolar el CanvasGroup.
    /// </summary>
    public class FadeController : MonoBehaviour
    {
        [Header("CanvasGroup que controla la opacidad de la pantalla")]
        [Tooltip("Debe estar en un objeto de UI que cubra toda la pantalla.")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Duración del fade")]
        [Tooltip("Tiempo en segundos que tarda en hacer fade in/out.")]
        [SerializeField] private float fadeDuration = 1f;

        /// <summary>
        /// Instancia global (Singleton) para facilitar el acceso desde otros scripts.
        /// </summary>
        public static FadeController Instance { get; private set; }

        private void Awake()
        {
            // Configurar singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Asegurarse de que el canvas comienza visible (fade in)
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }

        private void Start()
        {
            if (canvasGroup != null)
            {
                StartCoroutine(Helper.IEFade(canvasGroup, 0f, fadeDuration));
            }
        }

        /// <summary>
        /// Ejecuta un fade out y, al terminar, carga la escena indicada.
        /// </summary>
        /// <param name="sceneName">Nombre de la escena a cargar</param>
        public void FadeOutAndLoadScene(string sceneName)
        {
            if (canvasGroup != null)
            {
                StartCoroutine(FadeOutAndChangeSceneCoroutine(sceneName));
            }
        }

        /// <summary>
        /// Corrutina interna para fade out + cambio de escena.
        /// </summary>
        private IEnumerator FadeOutAndChangeSceneCoroutine(string sceneName)
        {
            yield return Helper.IEFade(canvasGroup, 1f, fadeDuration);
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Ejecuta manualmente un fade in.
        /// </summary>
        public void FadeIn()
        {
            if (canvasGroup != null)
            {
                StartCoroutine(Helper.IEFade(canvasGroup, 0f, fadeDuration));
            }
        }

        /// <summary>
        /// Ejecuta manualmente un fade out.
        /// </summary>
        public void FadeOut()
        {
            if (canvasGroup != null)
            {
                StartCoroutine(Helper.IEFade(canvasGroup, 1f, fadeDuration));
            }
        }
    }
}
