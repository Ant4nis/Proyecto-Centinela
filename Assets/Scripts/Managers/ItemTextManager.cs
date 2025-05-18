using System;
using UnityEngine;

namespace Managers
{
    public class ItemTextManager : MonoBehaviour
    {
        public static ItemTextManager Instance;

        [Header("Prefab")]
        [SerializeField] private ItemText prefabText;
        

        private void Awake()
        {
            Instance = this;
        }

        public ItemText ShowMessage(string message, Vector3 position, Color color)
        {
            
           ItemText text = Instantiate(prefabText, transform);
           text.SetText(message, color);
           text.transform.position = position;
           return text;
        }
    }
}