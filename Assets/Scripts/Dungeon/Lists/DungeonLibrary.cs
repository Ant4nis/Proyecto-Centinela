using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dungeon.Lists
{
    /// <summary>
    /// ScriptableObject que actúa como librería central de mazmorras.
    /// 
    /// Funcionalidades:
    /// 1. Almacena los niveles del juego, cada uno con múltiples prefabs de mazmorra.
    /// 2. Contiene referencias a los prefabs de puertas por dirección cardinal.
    /// </summary>
    [CreateAssetMenu(fileName = "DungeonLibrary", menuName = "Dungeon/Librería Dungeon")]
    public class DungeonLibrary : ScriptableObject
    {
        [Header("Levels")]
        [Tooltip("Lista de niveles, cada uno con su colección de prefabs de mazmorras.")]
        public Level[] Levels;

        [Header("Puertas")]
        [Tooltip("Prefab de puerta orientada al norte.")]
        public GameObject doorN;

        [Tooltip("Prefab de puerta orientada al sur.")]
        public GameObject doorS;

        [Tooltip("Prefab de puerta orientada al este.")]
        public GameObject doorE;

        [Tooltip("Prefab de puerta orientada al oeste.")]
        public GameObject doorW;
    }

    /// <summary>
    /// Representa un nivel del juego, que contiene múltiples prefabs de mazmorras.
    /// </summary>
    [Serializable]
    public class Level
    {
        [Tooltip("Nombre del nivel (solo informativo).")]
        public string Nombre;

        [Tooltip("Prefabs de mazmorras disponibles en este nivel.")]
        public GameObject[] Dungeons;
    }
}