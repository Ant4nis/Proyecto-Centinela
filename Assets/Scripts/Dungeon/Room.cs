using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using Tests;
using UnityEngine;
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
    /// Representa una sala del dungeon. Asocia un tipo de sala, detecta y almacena los tiles activos,
    /// y permite visualizar dicha información en tiempo de desarrollo mediante Gizmos.
    /// </summary>
    public class Room : MonoBehaviour
    {
        [Header("Configuración de Room")]
        [Tooltip("Tipo de habitación para esta sala (entrada, puzzle, combate, etc).")]
        [SerializeField] private RoomType roomType;

        [Header("Grid")]
        [Tooltip("Tilemap asociado a esta habitación, usado para detectar los tiles ocupados.")]
        [SerializeField] private Tilemap tilemapGrid;

        [Header("Gizmos")]
        [Tooltip("Activa o desactiva la visualización de Gizmos para esta sala.")]
        [SerializeField] private bool enableGizmos;

        /// <summary>
        /// Diccionario que almacena las posiciones de los tiles válidos en el Tilemap.
        /// La clave es la posición en el mundo, y el valor indica si está activa.
        /// </summary>
        private readonly Dictionary<Vector3, bool> _tileList = new();

        private void Start()
        {
            GetTiles();
            TemplateRoomCreation();
        }

        /// <summary>
        /// Recorre todas las posiciones del Tilemap y almacena las posiciones de los tiles activos.
        /// Se ignoran las salas de tipo "Free" y "Entrance".
        /// </summary>
        private void GetTiles()
        {
            if (IsDefaultRoom()) return;

            foreach (Vector3Int cellPosition in tilemapGrid.cellBounds.allPositionsWithin)
            {
                Vector3Int localPos = new(cellPosition.x, cellPosition.y, cellPosition.z);

                // Convertimos la posición del tile a coordenadas del mundo
                Vector3 worldPos = tilemapGrid.CellToWorld(localPos);
                Vector3 adjustedPos = new(worldPos.x + 0.5f, worldPos.y + 0.5f, worldPos.z);

                if (tilemapGrid.HasTile(localPos))
                {
                    _tileList[adjustedPos] = true;
                }
            }
        }

        private void TemplateRoomCreation()
        {
            if (IsDefaultRoom()) return;
            
            //plantilla random
            int indexRandom = Random.Range(0, LevelManager.Instance.TemplatesRoom.Templates.Length);
            Texture2D texture2D = LevelManager.Instance.TemplatesRoom.Templates[indexRandom];
            // Lista con todas las posiciones
            List<Vector3> positions = new List<Vector3>(_tileList.Keys);
            // Recorremos altura de los tiles
            for (int y = 0, i = 0; y < texture2D.height; y++)
            {
                // Recorrido del ancho
                for (int x = 0; x < texture2D.width; x++, i++)
                {
                    // obtener color pixeles de las plantillas para colocar segun color el prop
                    Color pixelColor = texture2D.GetPixel(x, y);

                    foreach (PropsRoom prop in LevelManager.Instance.TemplatesRoom.Props)
                    {
                        if (pixelColor == prop.propColor)
                        {
                            GameObject newProp = Instantiate(prop.propPrefab, tilemapGrid.transform);
                            newProp.transform.position = new Vector3(positions[i].x, positions[i].y, 0f);
                            
                            // Establecemos como no disponible al ocuiparse por el prefab
                            if (_tileList.ContainsKey(positions[i]))
                            {
                                _tileList[positions[i]] = false;
                            }
                        }
                    }
                }
            }
        }

        private bool IsDefaultRoom()
        {
            return roomType == RoomType.EntranceRoom || roomType == RoomType.FreeRoom;
        }

        /// <summary>
        /// Dibuja un Gizmo en el editor para visualizar los tiles activos de esta sala.
        /// Solo se ejecuta si la opción está activada y hay tiles almacenados.
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
