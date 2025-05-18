using System;
using InputSystem;
using Managers;
using Player;
using UnityEngine;

namespace Items
{
    public class TakeItem : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private ItemData item;
        
        //public PlayerInputReader _playerInputReader;
        private bool _canTake;
        private ItemText _createdText;


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

        private void ShowName()
        {
            Vector3 textPosition = new Vector3(0f, 1f, 0f);
            if (item is ItemWeapon weapon)
            {
                _createdText = ItemTextManager.Instance.ShowMessage(weapon.ID, transform.position + textPosition, Color.green);
            }
            else
            {
                _createdText = ItemTextManager.Instance.ShowMessage(item.ID, transform.position + textPosition, Color.white);

            }
        }

        private void HideName()
        {
            Destroy(_createdText.gameObject);
            _createdText = null; // Evitar referencias colgantes
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _canTake = true;
                ShowName();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _canTake = false;
                HideName();
            }
        }
    }
}