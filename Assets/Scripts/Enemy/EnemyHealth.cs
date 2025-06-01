using System;
using System.Collections;
using Dungeon;
using Interfaces;
using Managers;
using UIForms;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Controlador de salud para enemigos.
    /// Aplica daño, muestra feedback visual y añade puntuación al morir.
    /// </summary>
    public class EnemyHealth : MonoBehaviour, ITakeDamage
    {
        [Header("Configuración de Salud")]
        [Tooltip("Salud máxima del enemigo.")]
        [SerializeField] private float health = 100;

        [Header("Puntuación al morir")]
        [Tooltip("Puntos que otorga este enemigo al morir.")]
        [SerializeField] private int scoreReward = 100;

        private float currentHealth;
        private Coroutine colorCoroutine;
        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            currentHealth = health;
            originalColor = spriteRenderer.color;
        }

        private IEnumerator IETakeDamage()
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = originalColor;
        }

        private void ShowDamageColor()
        {
            if (colorCoroutine != null)
                StopCoroutine(colorCoroutine);

            colorCoroutine = StartCoroutine(IETakeDamage());
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            ShowDamageColor();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (UsuarioSesion.Instance != null)
            {
                var dto = new LeaderboardCreateDTO(
                    UsuarioSesion.Instance.Id,
                    scoreReward,
                    "Nivel 1"
                );

                ApiManager apiManager = FindFirstObjectByType<ApiManager>();
                if (apiManager != null)
                {
                    apiManager.SendLeaderboardEntry(dto);
                }
                else
                {
                    Debug.LogWarning("❗ ApiManager no encontrado en la escena.");
                }
            }
            
            Room room = GetComponentInParent<Room>();
            if (room != null)
            {
                room.NotifyDeadEnemy(gameObject);
            }

            Destroy(gameObject);
        }

        public void RestoreHealth(float quantity)
        {
            throw new NotImplementedException();
        }
    }
}
