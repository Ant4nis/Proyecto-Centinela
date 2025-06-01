using UnityEngine;

namespace Dungeon.Lists
{
    [CreateAssetMenu(fileName = "CofreItems_Nivel_", menuName = "Dungeon/Cofre Items")]
    public class TreasureItems : ScriptableObject
    {
        public GameObject[] AvailableItems;
    }
}