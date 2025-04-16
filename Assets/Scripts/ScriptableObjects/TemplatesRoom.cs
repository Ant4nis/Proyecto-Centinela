using System;
using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// ScriptableObject que define una plantilla de habitación para el sistema de generación procedural de niveles.
    /// 
    /// Este asset permite almacenar:
    /// 1. Una lista de texturas que actúan como "mapas" o guías para generar habitaciones (templates).
    /// 2. Un conjunto de props (objetos decorativos o interactivos) con sus respectivos colores y prefabs, que se relacionan
    ///    con los colores definidos en las texturas para saber qué instanciar.
    /// </summary>
    [CreateAssetMenu(fileName = "Plantilla", menuName = "Dungeon/Plantillas")]
    public class TemplatesRoom : ScriptableObject
    {
        [Header("Plantillas")]
        [Tooltip("Texturas utilizadas como guía para generar habitaciones.\nCada píxel puede representar un tipo de objeto, piso o pared.")]
        [SerializeField] private Texture2D[] templates;

        [Header("Props")]
        [Tooltip("Listado de props definidos por color, nombre y prefab.\nEstos colores deben coincidir con los usados en los templates.")]
        [SerializeField] private PropsRoom[] prop;

        /// <summary>
        /// Devuelve las texturas de plantillas.
        /// </summary>
        public Texture2D[] Templates => templates;

        /// <summary>
        /// Devuelve la lista de props asignados.
        /// </summary>
        public PropsRoom[] Props => prop;
    }

    /// <summary>
    /// Clase serializable que representa un objeto decorativo o funcional que puede ser instanciado dentro de una habitación.
    /// Se vincula mediante el color usado en la textura de plantilla.
    /// </summary>
    [Serializable]
    public class PropsRoom
    {
        [Tooltip("Nombre identificador del prop (solo informativo).")]
        public string propName;

        [Tooltip("Color que representa este prop en la textura de plantilla.")]
        public Color propColor;

        [Tooltip("Prefab que se instanciará cuando se detecte este color en la plantilla.")]
        public GameObject propPrefab;
    }
}
