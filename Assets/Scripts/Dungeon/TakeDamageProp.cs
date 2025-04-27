using System;
using Interfaces;
using UnityEngine;

namespace Dungeon
{
    /// <summary>
    /// PropDestructible:
    /// Script que permite que un GameObject tipo caja reciba daño y sea destruido 
    /// cuando su salud se agote.
    /// Funciones implementadas:
    /// 1. TakeDamage(float damage)
    /// 2. RestoreHealth(float health)
    /// </summary>
    public class TakeDamageProp : MonoBehaviour, ITakeDamage
    {
        [Header("Stats de Salud")]
        [Tooltip("Salud máxima de este prop.")]
        [SerializeField] private float maxHealth = 100f;

        [Header("Efectos de Destrucción")]
        [Tooltip("Prefab de efecto que se instanciará al destruir el prop.")]
        [SerializeField] private GameObject destructionEffectPrefab;

        // Salud actual interna
        private float _currentHealth;

        /// <summary>
        /// Inicializa la salud al valor máximo.
        /// </summary>
        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                TakeDamage(1f);
            }
        }

        /// <summary>
        /// Aplica daño al prop; si la salud llega a cero, provoca su destrucción.
        /// </summary>
        /// <param name="damage">Cantidad de daño a aplicar.</param>
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }

        /// <summary>
        /// Restaura salud al prop, sin exceder la salud máxima.
        /// </summary>
        /// <param name="health">Cantidad de salud a restaurar.</param>
        public void RestoreHealth(float health)
        {
            _currentHealth = Mathf.Min(_currentHealth + health, maxHealth);
        }

        /// <summary>
        /// Gestiona la destrucción del prop:
        ///  - Instancia un efecto.
        ///  - Destruye este GameObject.
        /// </summary>
        private void Die()
        {
            if (destructionEffectPrefab != null)
            {
                Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
