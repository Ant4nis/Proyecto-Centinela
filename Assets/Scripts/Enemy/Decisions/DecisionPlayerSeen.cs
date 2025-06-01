using System;
using Enemy.FSM;
using UnityEngine;

namespace Enemy.Decisions
{
    public class DecisionPlayerSeen : DecisionFSM
    {
        [Header("Configuracion")]
        [SerializeField] private LayerMask wallMask;

        private EnemyFSM _enemyFsm;

        private void Awake()
        {
            _enemyFsm = GetComponent<EnemyFSM>();
        }

        public override bool Decide(EnemyFSM enemy)
        {
            return DetectPlayerInRangeToSeen(enemy);
        }

        private bool DetectPlayerInRangeToSeen(EnemyFSM enemy)
        {
            if (enemy.Player == null) return false;
            Vector3 dirToPlayer = enemy.Player.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, dirToPlayer.magnitude, wallMask);

            if (hit.collider != null)
            {
                return false;
            }
            
            return true;
        }

        private void OnDrawGizmos()
        {
            if(_enemyFsm == null) return;
            if (_enemyFsm.Player == null) return;
            
            Gizmos.color = DetectPlayerInRangeToSeen(_enemyFsm) ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, _enemyFsm.Player.position);
        }
    }
}