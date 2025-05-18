using System;
using System.Collections;
using Dungeon;
using Dungeon.Lists;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    /// <summary>
    /// Gestor central del nivel que:
    /// 1. Implementa el patrón Singleton para acceso global.
    /// 2. Crea y destruye instancias de mazmorras según el nivel e índice actuales.
    /// 3. Controla la transición entre mazmorras con un efecto de fade.
    /// 4. Teletransporta al jugador a la sala de entrada tras generar una nueva mazmorra.
    /// 5. Cierra las puertas de la sala si no está completada al entrar el jugador.
    /// 6. Se suscribe y desuscribe a eventos de entrada en sala y activación de portal.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Instancia global del LevelManager accesible desde cualquier parte del juego.
        /// </summary>
        public static LevelManager Instance;
        
        [Header("TEMPORAL")]
        [Tooltip("Referencia al Transform del jugador para teletransportarlo.")]
        [SerializeField] private Transform player;
        
        [Header("Plantillas")]
        [Tooltip("Plantilla general para la generación de habitaciones no-puzzle.")]
        [SerializeField] private TemplatesRoom templatesRoom;
        [Tooltip("ScriptableObject específico para generar salas de tipo puzzle.")]
        [SerializeField] private PuzzleRoomTemplate puzzleRoomTemplate;

        [Header("Puertas")]
        [Tooltip("Librería con referencias para instanciar puertas dentro del dungeon.")]
        [SerializeField] private DungeonLibrary dungeonLibrary;
        
        /// <summary>Acceso público a la plantilla general de habitaciones.</summary>
        public TemplatesRoom TemplatesRoom => templatesRoom;

        /// <summary>Acceso público a la plantilla específica para habitaciones de tipo puzzle.</summary>
        public PuzzleRoomTemplate PuzzleRoomTemplate => puzzleRoomTemplate;

        /// <summary>Acceso público a la librería de mazmorra con prefabs de puertas y niveles.</summary>
        public DungeonLibrary DungeonLibrary => dungeonLibrary;
        
        public Transform Player => player;
        
        private int _currentLevelIndex;
        private int _currentDungeonIndex;
        private GameObject _currentDungeonGO;
        
        /// <summary>
        /// Sala en la que se encuentra actualmente el jugador.
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
        /// Al iniciar, crea la primera mazmorra.
        /// </summary>
        private void Start()
        {
            CreateDungeon();
        }

        /// <summary>
        /// Instancia la mazmorra correspondiente al nivel e índice actuales.
        /// </summary>
        private void CreateDungeon()
        {
           _currentDungeonGO = Instantiate(
               dungeonLibrary.Levels[_currentLevelIndex].Dungeons[_currentDungeonIndex],
               transform
           );
        }

        /// <summary>
        /// Avanza al siguiente dungeon; si supera el último, pasa al siguiente nivel.
        /// Destruye la instancia actual, crea la nueva y teletransporta al jugador.
        /// </summary>
        private void NextDungeon()
        {
            _currentDungeonIndex++;

            if (_currentDungeonIndex > dungeonLibrary.Levels[_currentLevelIndex].Dungeons.Length - 1)
            {
                _currentDungeonIndex = 0;
                _currentLevelIndex++;
            }
            
            Destroy(_currentDungeonGO);
            CreateDungeon();
            PlayerTeleport();
        }

        /// <summary>
        /// Encuentra la sala de entrada en la mazmorra recién generada y mueve al jugador a ella.
        /// </summary>
        private void PlayerTeleport()
        {
            Room[] newDungeonRooms = _currentDungeonGO.GetComponentsInChildren<Room>();
            Room entranceRoom = null;

            for (int i = 0; i < newDungeonRooms.Length; i++)
            {
                if (newDungeonRooms[i].RoomType == RoomType.EntranceRoom)
                {
                    entranceRoom = newDungeonRooms[i];
                }
            }

            if (entranceRoom != null && player != null)
            {
                player.transform.position = entranceRoom.transform.position;
            }
        }

        /// <summary>
        /// Coroutine que aplica el fade de salida, espera la duración,
        /// cambia de mazmorra y aplica el fade de entrada.
        /// </summary>
        private IEnumerator IEFadeDungeon()
        {
            UIManager.Instance.NewDungeonFade(1f);
            yield return new WaitForSeconds(1.5f);
            NextDungeon();
            UIManager.Instance.NewDungeonFade(0f);
        }
        
        /// <summary>
        /// Maneja el evento de entrada del jugador en una sala:
        /// cierra las puertas si la sala no está completada.
        /// </summary>
        /// <param name="room">Sala a la que ha entrado el jugador.</param>
        private void PlayerInRoomResponse(Room room)
        {
            _currentRoom = room;

            if (_currentRoom.RoomFinished == false)
            {
                _currentRoom.CloseDoors();
            }
        }

        /// <summary>
        /// Maneja el evento de activación del portal (escalera hacia abajo),
        /// iniciando la transición entre mazmorras.
        /// </summary>
        private void EventPortalResponse()
        {
            StartCoroutine(IEFadeDungeon());
        }
        
        /// <summary>
        /// Suscribe los manejadores a los eventos al habilitar este componente.
        /// </summary>
        private void OnEnable()
        {
            Room.PlayerInRoomEvent  += PlayerInRoomResponse;
            StairsDown.PortalEvent  += EventPortalResponse;
        }

        /// <summary>
        /// Desuscribe los manejadores de los eventos al deshabilitar este componente.
        /// </summary>
        private void OnDisable()
        {
            Room.PlayerInRoomEvent  -= PlayerInRoomResponse;
            StairsDown.PortalEvent  -= EventPortalResponse;
        }
    }
}
