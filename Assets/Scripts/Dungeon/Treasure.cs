using System;
using UnityEngine;

namespace Dungeon
{
    public class Treasure : MonoBehaviour
    {
        [Header("")]
        [SerializeField] private Transform itemPos;
        
        [Header("")]
        [SerializeField] private bool customTreasure;
        [SerializeField] private GameObject customItem;
        
        private readonly int _openTreasure = Animator.StringToHash("Open");
        
        private Animator _animator;
        private bool _isOpen;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void CreateTreasure()
        {
            if (customTreasure)
            {
                Instantiate(customItem, transform.position, Quaternion.identity, itemPos);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isOpen) return;
            if (collision.CompareTag("Player") == false) return;
            
            _animator.SetTrigger(_openTreasure);
            CreateTreasure();
            _isOpen = true;
        }
    }
}
