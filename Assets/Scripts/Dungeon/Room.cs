using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using Tests;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Dungeon
{
    public enum RoomType
    {
        FreeRoom,
        EntranceRoom,
        PuzzleRoom,
        EnemyRoom,
        BossRoom,
    }

    /// <summary>
    /// Representa una sala del dungeon. Se encarga de:
    /// 1. Asignar el tipo de sala (entrada, puzzle, combate, etc.).
    /// 2. Detectar y almacenar las posiciones válidas de tiles en el tilemap asociado.
    /// 3. Generar una sala basada en una plantilla visual (normal o de puzzle).
    /// 4. Instanciar puertas según la configuración de la sala.
    /// 5. Detectar cuándo el jugador entra y cerrar puertas si corresponde.
    /// 6. Visualizar los tiles activos mediante Gizmos en el editor.
    /// </summary>
    public class Room : MonoBehaviour
    {
        /// <summary>
        /// Evento que se lanza cuando el jugador entra en esta sala.
        /// </summary>
        public static event Action<Room> PlayerInRoomEvent;

        [Header("Configuración de Room")]
        [Tooltip("Tipo de habitación para esta sala (entrada, puzzle, combate, etc).")]
        [SerializeField] private RoomType roomType;

        [Header(nameof(Grid))]
        [Tooltip("Tilemap asociado a esta habitación, usado para detectar los tiles ocupados.")]
        [SerializeField] private Tilemap tilemapGrid;

        [Header(nameof(Gizmos))]
        [Tooltip("Activa o desactiva la visualización de Gizmos para esta sala.")]
        [SerializeField] private bool enableGizmos;

        [FormerlySerializedAs("doorN")]
        [Header("Puertas")]
        [Tooltip("Posiciones en las que se instanciarán puertas hacia el norte.")]
        [SerializeField] private Transform[] doorNPos;

        [FormerlySerializedAs("doorS")]
        [Tooltip("Posiciones en las que se instanciarán puertas hacia el sur.")]
        [SerializeField] private Transform[] doorSPos;

        [FormerlySerializedAs("doorE")]
        [Tooltip("Posiciones en las que se instanciarán puertas hacia el este.")]
        [SerializeField] private Transform[] doorEPos;

        [FormerlySerializedAs("doorW")]
        [Tooltip("Posiciones en las que se instanciarán puertas hacia el oeste.")]
        [SerializeField] private Transform[] doorWPos;

        /// <summary>
        /// Indica si la sala ha sido completada.
        /// </summary>
        public bool RoomFinished { get; private set; }

        /// <summary>
        /// Diccionario que almacena las posiciones válidas de tiles en el mundo.
        /// La clave es la posición del tile en coordenadas del mundo, y el valor indica si está disponible.
        /// </summary>
        private readonly Dictionary<Vector3, bool> _tileList = new();

        /// <summary>
        /// Lista de referencias a las puertas instanciadas en la sala.
        /// </summary>
        private List<Doors> _doorsList = new();

        /// <summary>
        /// Inicializa la sala al empezar la escena:
        /// - Detecta los tiles ocupados del Tilemap.
        /// - Genera la sala según una plantilla visual aleatoria.
        /// - Instancia las puertas definidas.
        /// </summary>
        private void Start()
        {
            GetTiles();
            BuildDoor();
            TemplateRoomCreation();
            PuzzleRoomCreation();
        }

        /// <summary>
        /// Detecta todos los tiles ocupados dentro de los límites del Tilemap.
        /// Ignora las habitaciones de tipo "FreeRoom" y "EntranceRoom".
        /// </summary>
        private void GetTiles()
        {
            if (IsDefaultRoom()) return;

            foreach (Vector3Int cellPosition in tilemapGrid.cellBounds.allPositionsWithin)
            {
                Vector3Int localPos = new(cellPosition.x, cellPosition.y, cellPosition.z);
                Vector3 worldPos = tilemapGrid.CellToWorld(localPos);
                Vector3 adjustedPos = new(worldPos.x + 0.5f, worldPos.y + 0.5f, worldPos.z);

                if (tilemapGrid.HasTile(localPos))
                {
                    _tileList[adjustedPos] = true;
                }
            }
        }

        /// <summary>
        /// Genera una sala normal usando una plantilla visual aleatoria desde TemplatesRoom.
        /// Cada pixel de la textura representa una posición, y su color determina qué objeto (prop) instanciar.
        /// </summary>
        private void TemplateRoomCreation()
        {
            if (IsDefaultRoom() || IsPuzzleRoom()) return;

            int indexRandom = Random.Range(0, LevelManager.Instance.TemplatesRoom.Templates.Length);
            Texture2D texture2D = LevelManager.Instance.TemplatesRoom.Templates[indexRandom];

            BuildRoomFromTexture(texture2D);
        }

        /// <summary>
        /// Genera una sala de tipo Puzzle usando una plantilla aleatoria del PuzzleRoomTemplate activo.
        /// Los elementos interactuables se instancian según el color en la textura.
        /// </summary>
        private void PuzzleRoomCreation()
        {
            if (IsDefaultRoom() || IsPuzzleRoom() == false) return;

            int indexRandom = Random.Range(0, LevelManager.Instance.PuzzleRoomTemplate.PuzzleTemplate.Length);
            Texture2D texture2D = LevelManager.Instance.PuzzleRoomTemplate.PuzzleTemplate[indexRandom];

            BuildRoomFromTexture(texture2D);
        }

        /// <summary>
        /// Construye una sala instanciando elementos definidos en una textura plantilla.
        /// Cada píxel representa una posición de tile y su color se compara con los props definidos.
        /// </summary>
        /// <param name="texture2D">Textura usada como mapa de colocación.</param>
        private void BuildRoomFromTexture(Texture2D texture2D)
        {
            List<Vector3> positions = new List<Vector3>(_tileList.Keys);

            for (int y = 0, i = 0; y < texture2D.height; y++)
            {
                for (int x = 0; x < texture2D.width; x++, i++)
                {
                    Color pixelColor = texture2D.GetPixel(x, y);

                    foreach (PropsRoom prop in LevelManager.Instance.TemplatesRoom.Props)
                    {
                        if (pixelColor == prop.propColor)
                        {
                            GameObject newProp = Instantiate(prop.propPrefab, tilemapGrid.transform);
                            newProp.transform.position = new Vector3(positions[i].x, positions[i].y, 0f);

                            if (_tileList.ContainsKey(positions[i]))
                            {
                                _tileList[positions[i]] = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Instancia una puerta en la posición indicada, la añade a la lista interna de puertas.
        /// </summary>
        /// <param name="prefabDoor">Prefab de la puerta a instanciar.</param>
        /// <param name="objPosition">Transform que representa la posición donde colocar la puerta.</param>
        private void CreateDoor(GameObject prefabDoor, Transform objPosition)
        {
            GameObject newDoor = Instantiate(prefabDoor, objPosition.position, Quaternion.identity, objPosition);
            Doors door = newDoor.GetComponent<Doors>();
            _doorsList.Add(door);
        }

        /// <summary>
        /// Construye todas las puertas de la sala basándose en las posiciones definidas para cada dirección.
        /// Usa la librería de puertas del LevelManager.
        /// </summary>
        private void BuildDoor()
        {
            if (doorNPos.Length > 0)
            {
                for (int i = 0; i < doorNPos.Length; i++)
                {
                    CreateDoor(LevelManager.Instance.DungeonLibrary.doorN, doorNPos[i]);
                }
            }

            if (doorSPos.Length > 0)
            {
                for (int i = 0; i < doorSPos.Length; i++)
                {
                    CreateDoor(LevelManager.Instance.DungeonLibrary.doorS, doorSPos[i]);
                }
            }

            if (doorEPos.Length > 0)
            {
                for (int i = 0; i < doorEPos.Length; i++)
                {
                    CreateDoor(LevelManager.Instance.DungeonLibrary.doorE, doorEPos[i]);
                }
            }

            if (doorWPos.Length > 0)
            {
                for (int i = 0; i < doorWPos.Length; i++)
                {
                    CreateDoor(LevelManager.Instance.DungeonLibrary.doorW, doorWPos[i]);
                }
            }
        }

        /// <summary>
        /// Cierra todas las puertas instanciadas en esta sala.
        /// </summary>
        public void CloseDoors()
        {
            for (int i = 0; i < _doorsList.Count; i++)
            {
                _doorsList[i].CloseDoors();
            }
        }

        /// <summary>
        /// Abre todas las puertas instanciadas en esta sala.
        /// </summary>
        public void OpenDoors()
        {
            for (int i = 0; i < _doorsList.Count; i++)
            {
                _doorsList[i].OpenDoors();
            }
        }

        /// <summary>
        /// Determina si esta sala es de tipo "FreeRoom" o "EntranceRoom",
        /// en cuyo caso se considera predeterminada y no requiere procesamiento.
        /// </summary>
        /// <returns>True si es una sala predeterminada; de lo contrario, false.</returns>
        private bool IsDefaultRoom()
        {
            return roomType == RoomType.EntranceRoom || roomType == RoomType.FreeRoom;
        }

        /// <summary>
        /// Determina si esta sala es una sala de tipo puzzle.
        /// </summary>
        private bool IsPuzzleRoom()
        {
            return roomType == RoomType.PuzzleRoom;
        }

        /// <summary>
        /// Detecta si el jugador entra en la sala mediante un collider.
        /// Si no es una sala por defecto, lanza el evento PlayerInRoomEvent.
        /// </summary>
        /// <param name="other">Collider que entra al trigger.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsDefaultRoom()) return;

            if (other.gameObject.CompareTag(nameof(Player)))
            {
                PlayerInRoomEvent?.Invoke(this);
            }
        }

        /// <summary>
        /// Dibuja un Gizmo en el editor para mostrar la disponibilidad de cada tile detectado.
        /// Verde = disponible; rojo = ocupado.
        /// Solo se ejecuta si está activado y hay tiles almacenados.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!enableGizmos) return;
            if (_tileList.Count == 0) return;

            foreach (KeyValuePair<Vector3, bool> tile in _tileList)
            {
                Gizmos.color = tile.Value ? Color.green : Color.red;
                Gizmos.DrawWireCube(tile.Key, Vector3.one * 0.8f);
            }
#endif
        }
    }
}
