using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Asigna la configuración de debug en tiempo de ejecución.
    /// Debe existir en una escena inicial o persistente.
    /// </summary>
    public class DebugInitializer : MonoBehaviour
    {
        [Tooltip("ScriptableObject que contiene las categorías activas de depuración.")]
        [SerializeField] private DebugConfig debugConfig;

        private void Awake()
        {
            DebugLogger.SetConfig(debugConfig);
        }
    }
}