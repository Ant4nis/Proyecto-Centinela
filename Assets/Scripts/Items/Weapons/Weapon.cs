using UnityEngine;

namespace Items.Weapons
{
    /// <summary>
    /// Clase base para todos los tipos de armas.
    /// 
    /// Funcionalidades:
    /// 1. Gestiona la animación de disparo.
    /// 2. Expone la referencia a los datos del arma (`ItemWeapon`).
    /// 3. Define un método virtual de disparo (`Fire`) que puede ser sobrescrito por armas específicas.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        [Header("Referencias")]
        [Tooltip("Datos configurables del arma (daño, tipo, precisión, etc.).")]
        [SerializeField] protected ItemWeapon itemWeapon;

        [Header("Posición de disparo")]
        [Tooltip("Transform que define la posición desde donde se disparan los proyectiles.")]
        [SerializeField] protected Transform shootingPosition;

        private readonly int _shootingAnim = Animator.StringToHash("Shot");

        private Animator _animator;

        /// <summary>Acceso público a los datos del arma.</summary>
        public ItemWeapon ItemWeapon => itemWeapon;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Reproduce la animación de disparo configurada en el Animator.
        /// </summary>
        protected void Animate()
        {
            _animator.SetTrigger(_shootingAnim);
        }

        /// <summary>
        /// Método virtual para realizar el disparo.
        /// Puede ser sobrescrito por clases derivadas para implementar comportamientos específicos.
        /// </summary>
        public virtual void Fire()
        {
            // Implementación vacía. Se sobrescribe en clases hijas como GunWeapon, etc.
        }
    }
}