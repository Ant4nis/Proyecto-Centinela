using UnityEngine;

namespace Dungeon.Lists
{
    [CreateAssetMenu(fileName = "DungeonLibrary",menuName = "Dungeon/Librería Dungeon")]
    public class DungeonLibrary : ScriptableObject
    {
        [Header("Puertas")]
        [Tooltip("Norte")]
        public GameObject doorN;
        [Tooltip("Sur")]
        public GameObject doorS;
        [Tooltip("Este")]
        public GameObject doorE;
        [Tooltip("Oeste")]
        public GameObject doorW;
    }
}