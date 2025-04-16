using System;
using System.Collections.Generic;
using Tests;
using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// ScriptableObject que permite configurar la activación de mensajes de depuración por categoría.
    /// </summary>
    [CreateAssetMenu(fileName = "ConfigTest", menuName = "/Debug/Debug Config")]
    public class DebugConfig : ScriptableObject
    {
        [Serializable]
        public class DebugFlag
        {
            public DebugCategory category;
            public bool enabled;
        }

        [Tooltip("Activa o desactiva los mensajes de depuración por categoría.")]
        [SerializeField] private List<DebugFlag> flags = new List<DebugFlag>();

        /// <summary>
        /// Consulta si una categoría está habilitada.
        /// </summary>
        public bool IsEnabled(DebugCategory category)
        {
            foreach (var flag in flags)
            {
                if (flag.category == category)
                    return flag.enabled;
            }
            return false;
        }
    }
}