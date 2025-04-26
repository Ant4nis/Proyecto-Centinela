using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponDirectionConfig", menuName = "Weapons/Direction Config")]
    public class WeaponDirectionConfig : ScriptableObject
    {
        [System.Serializable]
        public class DirectionSettings
        {
            public Direction8 direction;
            public int sortingOrder = 0;        // Orden en la capa
        }

        public DirectionSettings[] settings;

        public DirectionSettings GetSettingsFor(Direction8 dir)
        {
            foreach (var s in settings)
            {
                if (s.direction == dir)
                    return s;
            }

            Debug.LogWarning($"No se encontró configuración para {dir}");
            return null;
        }
    }
}