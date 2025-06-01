using System;
using Extra;
using UnityEngine;

namespace Managers
{
    public class ItemTextManager : Singleton<ItemTextManager>
    {
        [Header("Prefab")]
        [SerializeField] private ItemText prefabText;
 
        public ItemText ShowMessage(string message, Vector3 position, Color color)
        {
           ItemText text = Instantiate(prefabText, transform);
           text.SetText(message, color);
           text.transform.position = position;
           return text;
        }
    }
}