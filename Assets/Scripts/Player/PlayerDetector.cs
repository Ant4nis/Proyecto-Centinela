using System;
using System.Collections.Generic;
using Enemy;
using NUnit.Framework;
using UnityEngine;

namespace Player
{
    public class PlayerDetector:MonoBehaviour
    {
        [Header("Configuration")] 
        [SerializeField] private float detectorRadius;
        [SerializeField] private bool debug;

        [Header("RayCast")]
        [SerializeField] private LayerMask obstaclesMask;
        
        public EnemyHealth EnemyObjective { get; private set;}
        
        private CircleCollider2D myCollider2D;
        private List<EnemyHealth> enemiesList = new List<EnemyHealth>();
        private List<EnemyHealth> enemiesDetectedList = new List<EnemyHealth>();

        private void Awake()
        {
            myCollider2D = GetComponent<CircleCollider2D>();
        }

        private void Start()
        {
            myCollider2D.radius = detectorRadius;
        }

        private void Update()
        {
            CalculateEnemiesSeen();
            GetEnemyNearest();
        }

        private void GetEnemyNearest()
        {
            float minDistance = Mathf.Infinity;
            EnemyHealth enemySearched = null;
            for (int i = 0; i < enemiesDetectedList.Count; i++)
            {
                Vector3 enemyPos = enemiesDetectedList[i].transform.position;
                float distanceTowardsEnemy = Vector3.Distance(transform.position, enemyPos);
                if (distanceTowardsEnemy < minDistance)
                {
                    enemySearched = enemiesDetectedList[i];
                    minDistance = distanceTowardsEnemy;
                }
            }

            if (enemySearched != null)
            {
                EnemyObjective = enemySearched;
                enemiesDetectedList.Clear();
            }
        }
        
        private void CalculateEnemiesSeen()
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                if (enemiesList.Count == 0 || enemiesList == null)
                {
                    return;
                }
                
                Vector3 playerPosition = transform.position;
                Vector3 dirToEnemey = enemiesList[i].transform.position - playerPosition;
                RaycastHit2D hit = Physics2D.Raycast(playerPosition, dirToEnemey, dirToEnemey.magnitude, obstaclesMask);
                if (hit.collider == null)
                {
                    //sin obstaculo entre player y enemigo, le vemos
                    if (enemiesDetectedList.Contains(enemiesList[i]) == false)
                    {
                        enemiesDetectedList.Add(enemiesList[i]);
                    }
                }
                else
                {
                    //si hay obstaculo no vemos
                    if (enemiesDetectedList.Contains(enemiesList[i]))
                    {
                        enemiesDetectedList.Remove(enemiesList[i]);
                    }

                    if (EnemyObjective == enemiesList[i])
                    {
                        EnemyObjective = null;
                    }
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                enemiesList.Add(enemy);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();

                if (enemiesList.Contains(enemy))
                {
                    enemiesList.Remove(enemy);
                }

                if (enemy == EnemyObjective)
                {
                    EnemyObjective = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (debug == false) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < enemiesList.Count; i++)
            {
                Gizmos.DrawLine(transform.position, enemiesList[i].transform.position);
            }

            if (EnemyObjective != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, EnemyObjective.transform.position);
            }
        }
    }
}