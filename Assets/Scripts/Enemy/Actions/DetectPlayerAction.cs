using System;
using Enemy.FSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Actions
{
    public class DetectPlayerAction : ActionFSM
    {
        [Header("Configuracion")]
        [SerializeField] private float detectionRadius;
        [SerializeField] private LayerMask playerMask;
        
        private Collider2D[] _results = new Collider2D[10];
        private EnemyFSM _enemyFsm;

        private void Awake()
        {
            _enemyFsm = GetComponent<EnemyFSM>();
        }

        public override void ExecuteAction()
        {
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            int quantity = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, _results, playerMask);
            if (quantity <= 0)
            {
                _enemyFsm.Player = null;
                return;
            }
            
            _enemyFsm.Player = _results[0].transform;
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}