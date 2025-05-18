using UnityEngine;

/// <summary>
/// Se coloca sobre un GameObject raíz para que no se destruya entre escenas.
/// Todos los hijos permanecerán automáticamente al estar dentro de él.
/// </summary>
public class PersistAcrossScenes : MonoBehaviour
{
    private void Awake()
    {
        // Evita duplicados si ya existe uno con el mismo nombre
        if (GameObject.Find(gameObject.name) != gameObject)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}