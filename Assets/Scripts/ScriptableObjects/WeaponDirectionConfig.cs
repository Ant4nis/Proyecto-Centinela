using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// ScriptableObject que configura el orden de renderizado del arma según la dirección del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Define un conjunto de configuraciones por dirección cardinal.
    /// 2. Proporciona la configuración adecuada a partir de una dirección solicitada.
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponDirectionConfig", menuName = "Weapons/Direction Config")]
    public class WeaponDirectionConfig : ScriptableObject
    {
        /// <summary>
        /// Configuración individual para una dirección específica.
        /// </summary>
        [System.Serializable]
        public class DirectionSettings
        {
            [Tooltip("Dirección cardinal asociada.")]
            public Direction8 direction;

            [Tooltip("Orden de renderizado para esta dirección.")]
            public int sortingOrder = 0;
        }

        [Tooltip("Lista de configuraciones para las distintas direcciones posibles.")]
        public DirectionSettings[] settings;

        /// <summary>
        /// Devuelve la configuración de orden de renderizado correspondiente a una dirección específica.
        /// </summary>
        /// <param name="dir">Dirección cardinal solicitada.</param>
        /// <returns>Configuración de dirección encontrada o null si no existe.</returns>
        public DirectionSettings GetSettingsFor(Direction8 dir)
        {
            foreach (var s in settings)
            {
                if (s.direction == dir)
                    return s;
            }

        #if UNITY_EDITOR
            Debug.LogWarning($"No se encontró configuración para la dirección {dir}");
        #endif
            return null;
        }
    }
}