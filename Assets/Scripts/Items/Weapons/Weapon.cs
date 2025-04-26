
using UnityEngine;

namespace Items.Weapons
{
    // Clase base
    public class Weapon : MonoBehaviour
    {
        private readonly int _shootingAnim = Animator.StringToHash("Shot");
        
        [Header("Referencias")] 
        [SerializeField] protected ItemWeapon itemWeapon;
        
        [Header("")] 
        [SerializeField] protected Transform shootingPosition;

        public ItemWeapon ItemWeapon => itemWeapon;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected void Animate()
        {
            _animator.SetTrigger(_shootingAnim);
        }

        // herederos pueden sobreescribirlo
        public virtual void Fire()
        {
            
        }
    }
}