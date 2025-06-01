using UnityEngine;
using UnityEngine.SceneManagement;

namespace Text
{
    public class CreditScroller : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Texto que se desplazará.")]
        [SerializeField] private RectTransform creditText;

        [Tooltip("Velocidad del scroll (unidades por segundo).")]
        [SerializeField] private float scrollSpeed = 30f;

        [Tooltip("Tiempo antes de cambiar de escena (segundos).")]
        [SerializeField] private float duration = 20f;

        [Tooltip("Escena a la que volver tras los créditos.")]
        [SerializeField] private string sceneToLoad = "MainMenuScene";

        private float timer;

        private void Update()
        {
            if (creditText == null) return;

            // Mover el texto hacia arriba
            creditText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            if (timer >= duration)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}