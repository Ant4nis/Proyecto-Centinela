using System;
using Interfaces;
using Managers;
using UnityEngine;

namespace Items.Weapons
{
    /// <summary>
    /// Componente que gestiona el comportamiento de un proyectil.
    /// 
    /// Funcionalidades:
    /// 1. Se desplaza en una dirección establecida a una velocidad configurada.
    /// 2. Aplica daño a los objetivos que implementen la interfaz ITakeDamage.
    /// 3. Se devuelve automáticamente al pool tras impactar o ser reciclado.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Velocidad a la que se mueve el proyectil.")]
        [SerializeField] private float projectileSpeed;

        /// <summary>
        /// Dirección en la que el proyectil se desplazará.
        /// </summary>
        public Vector3 ProjectileDirection { get; set; }

        /// <summary>
        /// Daño que el proyectil inflige al impactar.
        /// </summary>
        public float ProjectileDamage { get; set; }

        /// <summary>
        /// Prefab original del proyectil, usado para devolverlo al pool correspondiente.
        /// </summary>
        public Projectile OriginalPrefab { get; set; }

        private Rigidbody2D _rb;

        /// <summary>
        /// Inicializa las referencias necesarias.
        /// </summary>
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Reinicializa el estado del proyectil antes de ser lanzado.
        /// 
        /// Establece posición, dirección, rotación y velocidad.
        /// </summary>
        /// <param name="position">Nueva posición inicial del proyectil.</param>
        /// <param name="direction">Dirección hacia la que debe desplazarse.</param>
        public void ResetProjectile(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            ProjectileDirection = direction;
            gameObject.SetActive(true);

            // Calcula la rotación visual según la dirección
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (_rb != null)
                _rb.linearVelocity = direction.normalized * projectileSpeed;
        }
        
        /// <summary>
        /// Detecta colisiones y aplica daño si el objeto impactado implementa ITakeDamage.
        /// Luego, devuelve el proyectil al pool o lo desactiva si no hay pool disponible.
        /// </summary>
        /// <param name="other">Collider del objeto con el que se colisionó.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Aplica daño al objetivo si soporta la interfaz ITakeDamage
            other.GetComponent<ITakeDamage>()?.TakeDamage(1f);

            // Devuelve el proyectil al pool
            if (OriginalPrefab != null)
                ProjectilePool.Instance.ReturnToPool(OriginalPrefab, this);
            else
                gameObject.SetActive(false);
        }
        
        /// <summary>Al activarse, reinicia su velocidad basándose en la dirección asignada.</summary>
        private void OnEnable()
        {
            if (_rb != null)
                _rb.linearVelocity = ProjectileDirection.normalized * projectileSpeed;
        }
    }
}
