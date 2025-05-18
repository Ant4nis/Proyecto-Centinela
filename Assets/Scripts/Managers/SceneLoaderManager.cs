using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Gestor centralizado para cargar escenas en cualquier parte del juego.
    /// Puede usarse desde botones o desde código.
    ///
    /// Ejemplo: SceneLoader.Instance.LoadScene("NombreDeLaEscena");
    /// </summary>
    public class SceneLoaderManager : MonoBehaviour
    {
        // Singleton para acceso global 
        public static SceneLoaderManager Instance { get; private set; }

        private void Awake()
        {
            // Aseguramos que solo haya una instancia si lo usamos como Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Carga una escena por su nombre.
        /// </summary>
        /// <param name="sceneName">Nombre de la escena a cargar</param>
        public void LoadScene(string sceneName)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Nombre de la escena no especificado.");
            }
        }

        /// <summary>
        /// Carga la siguiente escena en el índice de build (Si se usa por orden).
        /// </summary>
        public void LoadNextScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(currentIndex + 1);
            }
            else
            {
                Debug.LogWarning("No hay más escenas en el índice.");
            }
        }
    }
}