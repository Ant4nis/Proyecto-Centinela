using System.Diagnostics;
using UnityEngine;

namespace Tests
{
    /// <summary>
    /// Sistema de log personalizado para mostrar mensajes según configuración de categorías.
    /// Excluye los mensajes del build final automáticamente.
    /// </summary>
    public class DebugLogger
    {
        private static DebugConfig config;

        /// <summary>
        /// Asigna la configuración de depuración activa. Llamar desde un inicializador o componente global.
        /// </summary>
        public static void SetConfig(DebugConfig debugConfig)
        {
            config = debugConfig;
        }

        /// <summary>
        /// Muestra un mensaje en consola si la categoría está activada.
        /// Este método solo compila en el editor.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void Log(string message, DebugCategory category)
        {
            if (config != null && config.IsEnabled(category))
                UnityEngine.Debug.Log($"[{category}] {message}");
        }
    }
}