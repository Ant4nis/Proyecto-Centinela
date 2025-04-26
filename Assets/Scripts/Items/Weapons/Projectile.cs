using System;
using Managers;
using UnityEngine;

namespace Items.Weapons
{
    /// <summary>
    /// Comportamiento del proyectil.
    /// 1. Se mueve en una dirección con velocidad configurable.
    /// 2. Aplica daño.
    /// 3. Al colisionar se devuelve al pool.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Velocidad del proyectil.")]
        [SerializeField] private float projectileSpeed;

        /// <summary>
        /// Dirección de movimiento del proyectil.
        /// </summary>
        public Vector3 ProjectileDirection { get; set; }

        /// <summary>
        /// Daño que inflige el proyectil.
        /// </summary>
        public float ProjectileDamage { get; set; }

        /// <summary>
        /// Referencia al prefab original usada para devolver al pool.
        /// </summary>
        public Projectile OriginalPrefab { get; set; }

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            // Reinicia la velocidad por si el proyectil fue reciclado
            if (_rb != null)
                _rb.linearVelocity = ProjectileDirection.normalized * projectileSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Aquí podrías aplicar daño al objetivo si implementas IDamageable

            // Se devuelve al pool
            if (OriginalPrefab != null)
                ProjectilePool.Instance.ReturnToPool(OriginalPrefab, this);
            else
                gameObject.SetActive(false);
        }

        public void ResetProjectile(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            ProjectileDirection = direction;
            gameObject.SetActive(true);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (_rb != null)
                _rb.linearVelocity = direction.normalized * projectileSpeed;
        }
    }
}