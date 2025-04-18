using System;
using Dungeon;
using Dungeon.Lists;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
        /// <summary>
    /// Gestor central del nivel que proporciona acceso global a las plantillas de generación de salas.
    /// 
    /// Funcionalidades:
    /// 1. Almacena la plantilla de habitaciones generales (TemplatesRoom).
    /// 2. Almacena la plantilla específica para habitaciones tipo Puzzle (PuzzleRoomTemplate).
    /// 3. Almacena referencias a librerías de mazmorras (DungeonLibrary) usadas para generación de puertas.
    /// 4. Expone dichas plantillas mediante propiedades públicas para que otros sistemas (como Room) las usen.
    /// 5. Implementa patrón Singleton para acceso global desde otras clases.
    /// 6. Responde a eventos cuando el jugador entra a una habitación.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Instancia global del LevelManager accesible desde cualquier parte del juego.
        /// </summary>
        public static LevelManager Instance;
        
        [Header("Plantillas Generales")]
        [Tooltip("Plantilla general para la generación de habitaciones no-puzzle.")]
        [SerializeField] private TemplatesRoom templatesRoom;

        [Header("Plantilla de Puzzle")]
        [Tooltip("ScriptableObject específico para generar salas de tipo puzzle.")]
        [SerializeField] private PuzzleRoomTemplate puzzleRoomTemplate;

        [Header("Puertas")]
        [Tooltip("Librería con referencias para instanciar puertas dentro del dungeon.")]
        [SerializeField] private DungeonLibrary dungeonLibrary;
        
        /// <summary>
        /// Acceso público a la plantilla general de habitaciones.
        /// </summary>
        public TemplatesRoom TemplatesRoom => templatesRoom;

        /// <summary>
        /// Acceso público a la plantilla específica para habitaciones de tipo puzzle.
        /// </summary>
        public PuzzleRoomTemplate PuzzleRoomTemplate => puzzleRoomTemplate;

        /// <summary>
        /// Acceso público a la librería de mazmorra que contiene referencias a elementos como puertas.
        /// </summary>
        public DungeonLibrary DungeonLibrary => dungeonLibrary;
        
        /// <summary>
        /// Referencia a la sala en la que se encuentra actualmente el jugador.
        /// </summary>
        private Room _currentRoom;
        
        /// <summary>
        /// Inicializa la instancia Singleton al cargar la escena.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Maneja la lógica cuando el jugador entra a una sala.
        /// Cierra las puertas si la sala no ha sido completada aún.
        /// </summary>
        /// <param name="room">Sala a la que el jugador ha entrado.</param>
        private void PlayerInRoomResponse(Room room)
        {
            _currentRoom = room;

            if (_currentRoom.RoomFinished == false)
            {
                _currentRoom.CloseDoors();
            }
        }

        /// <summary>
        /// Se suscribe al evento que detecta que el jugador ha entrado en una sala.
        /// </summary>
        private void OnEnable()
        {
            Room.PlayerInRoomEvent += PlayerInRoomResponse;
        }

        /// <summary>
        /// Se desuscribe del evento al desactivarse este componente.
        /// </summary>
        private void OnDisable()
        {
            Room.PlayerInRoomEvent -= PlayerInRoomResponse;
        }
    }
}