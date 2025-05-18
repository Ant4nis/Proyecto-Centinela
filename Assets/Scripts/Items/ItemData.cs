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
        [Tooltip("ID del item")]
        public string ID;
        [Tooltip("Nombre del item")]
        public string Name;
        [Tooltip("Icono del item")]
        public Sprite Icon;

        public virtual void Take()
        {
            
        } 
    }
}