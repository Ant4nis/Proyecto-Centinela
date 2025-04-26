using UnityEngine;


namespace Items
{
    /// <summary>
    /// Clase base de la que heredaran los demas items
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Item")]
    public class ItemData : ScriptableObject
    {
        [Header("Datos base")] 
        public string ID;
        public string Name;
        public Sprite Icon;
    }
}