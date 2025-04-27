using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    /// <summary>
    /// Enum que define el nivel de dificultad del puzzle.
    /// Funcionalidades:
    /// 1. Facil: Plantilla sencilla.
    /// 2. Media: Plantilla intermedia.
    /// 3. Dificil: Plantilla compleja.
    /// </summary>
    public enum PuzzleDifficulty
    {
        Facil,
        Media,
        Dificil
    }

    /// <summary>
    /// ScriptableObject que define la configuración específica para salas de puzzles.
    /// 
    /// Funcionalidades:
    /// 1. Almacena una o varias texturas que actúan como guías para la disposición del puzzle.
    /// 2. Define la dificultad del puzzle seleccionable mediante un enum (Fácil, Media o Difícil).
    /// 3. Define una lista de elementos interactuables que se vinculan a colores específicos de la plantilla.
    /// 4. Permite configurar aspectos adicionales del puzzle, como un tiempo límite para su resolución.
    /// </summary>
    [CreateAssetMenu(fileName = "PuzzleRoomTemplate", menuName = "Dungeon/Puzzle Room Template")]
    public class PuzzleRoomTemplate : ScriptableObject
    {
        [Header("Plantilla del Puzzle")]
        [Tooltip("Texturas que actúan como guía para la disposición del puzzle.\nCada píxel puede representar un elemento interactuable.")]
        [SerializeField] private Texture2D[] puzzleTemplate;

        [Header("Dificultad del Puzzle")]
        [Tooltip("Selecciona la dificultad del puzzle.\nFacil: Plantilla sencilla. Media: Plantilla intermedia. Dificil: Plantilla compleja.")]
        [SerializeField] private PuzzleDifficulty difficulty;

        [Header("Configuración Adicional")]
        [Tooltip("Tiempo límite para resolver el puzzle, en segundos (0 si no aplica).")]
        [SerializeField] private float timeLimit;
        
        /// <summary>Devuelve las texturas de plantilla del puzzle.</summary>
        public Texture2D[] PuzzleTemplate => puzzleTemplate;

        /// <summary>Devuelve el nivel de dificultad seleccionado para el puzzle.</summary>
        public PuzzleDifficulty Difficulty => difficulty;
        
        /// <summary>Devuelve el tiempo límite para el puzzle.</summary>
        public float TimeLimit => timeLimit;
    }
    
}