using UnityEngine;

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
    public class Room : MonoBehaviour
    {
        [SerializeField] private bool activateDebug;
    }
}
