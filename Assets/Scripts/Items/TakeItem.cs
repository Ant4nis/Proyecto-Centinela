using System;
using InputSystem;
using Player;
using UnityEngine;

namespace Items
{
    public class TakeItem : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private ItemData item;
        
        public PlayerInputReader _playerInputReader;
        private bool _canTake;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetItem();
            }
        }

        private void GetItem()
        {
            if (_canTake)
            {
                item.Take();
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _canTake = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _canTake = false;
            }
        }
    }
}