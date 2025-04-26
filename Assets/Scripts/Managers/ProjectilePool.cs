using System.Collections.Generic;
using Items.Weapons;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Sistema de Object Pool para proyectiles de diferentes tipos.
    /// 
    /// Funciones:
    /// 1. Soporta múltiples tipos de proyectiles.
    /// 2. Pre-instancia una cantidad configurable de cada tipo.
    /// 3. Reutiliza proyectiles inactivos y expande el pool según necesidad.
    /// </summary>
    public class ProjectilePool : MonoBehaviour
    {
        /// <summary>
        /// Instancia global del pool.
        /// </summary>
        public static ProjectilePool Instance;

        [Header("Configuración Inicial")]
        [Tooltip("Lista de proyectiles que se gestionarán.")]
        [SerializeField] private List<ProjectilePoolEntry> poolEntries;

        /// <summary>
        /// Diccionario que mapea cada tipo de proyectil a su cola de objetos disponibles.
        /// </summary>
        private Dictionary<Projectile, Queue<Projectile>> _poolDictionary;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializePools();
        }

        /// <summary>
        /// Inicializa todos los pools configurados.
        /// </summary>
        private void InitializePools()
        {
            _poolDictionary = new Dictionary<Projectile, Queue<Projectile>>();

            foreach (var entry in poolEntries)
            {
                var queue = new Queue<Projectile>();

                for (int i = 0; i < entry.initialAmount; i++)
                {
                    Projectile proj = Instantiate(entry.projectilePrefab, transform);
                    proj.gameObject.SetActive(false);
                    queue.Enqueue(proj);
                }

                _poolDictionary.Add(entry.projectilePrefab, queue);
            }
        }

        /// <summary>
        /// Solicita un proyectil de un tipo específico.
        /// </summary>
        /// <param name="projectilePrefab">Prefab del proyectil solicitado.</param>
        /// <returns>Instancia activa del proyectil.</returns>
        public Projectile GetProjectile(Projectile projectilePrefab)
        {
            if (!_poolDictionary.ContainsKey(projectilePrefab))
            {
                Debug.LogWarning($"No hay pool para el prefab: {projectilePrefab.name}. Se crea uno nuevo.");
                _poolDictionary[projectilePrefab] = new Queue<Projectile>();
            }

            Queue<Projectile> pool = _poolDictionary[projectilePrefab];

            if (pool.Count == 0)
            {
                Projectile newProj = Instantiate(projectilePrefab, transform);
                newProj.gameObject.SetActive(false);
                pool.Enqueue(newProj);
            }

            Projectile proj = pool.Dequeue();
            proj.gameObject.SetActive(true);
            return proj;
        }

        /// <summary>
        /// Devuelve un proyectil al pool correspondiente.
        /// </summary>
        /// <param name="projectilePrefab">Prefab base del proyectil.</param>
        /// <param name="proj">Instancia a devolver.</param>
        public void ReturnToPool(Projectile projectilePrefab, Projectile proj)
        {
            proj.gameObject.SetActive(false);
            if (!_poolDictionary.ContainsKey(projectilePrefab))
            {
                _poolDictionary[projectilePrefab] = new Queue<Projectile>();
            }
            _poolDictionary[projectilePrefab].Enqueue(proj);
        }
    }

    [System.Serializable]
    public class ProjectilePoolEntry
    {
        [Tooltip("Prefab del proyectil que se va a gestionar.")]
        public Projectile projectilePrefab;

        [Tooltip("Cantidad inicial que se pre-instanciará.")]
        public int initialAmount = 10;
    }
}
