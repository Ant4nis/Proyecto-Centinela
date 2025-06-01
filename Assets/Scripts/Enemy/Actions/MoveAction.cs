using System;
using Enemy.FSM;
using UnityEngine;

namespace Enemy.Actions
{
    public class MoveAction : ActionFSM
    {
        [Header("Configuracion")]
        [SerializeField] private bool debug;
        [SerializeField] private bool randomMove;
        [SerializeField] private bool tileMove;
        
        [Header("Valores")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 moveRange;
        [SerializeField] private float minMoveDistanceCheck = 0.5f;
        
        [Header("Obstaculos")]
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float detectionRadius;

        private EnemyFSM _enemyFsm;
        private Vector3 movePosition;
        private Vector3 moveDirection;

        private void Awake()
        {
            _enemyFsm = GetComponent<EnemyFSM>();
        }

        private void Start()
        {
            GetNewMoveDirection();
        }

        public override void ExecuteAction()
        {
            moveDirection = (movePosition - transform.position).normalized;
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
            if (CanGetNewDirection())
            {
                GetNewMoveDirection();
            }
        }

        private void GetNewMoveDirection()
        {
            if (randomMove)
            {
                movePosition = transform.position +  GetRandomMoveDirection();
            }

            if (tileMove)
            {
                movePosition = _enemyFsm.RoomParent.GetAvailableTile();
            }
        }

        private Vector3 GetRandomMoveDirection()
        {
            float randomX = UnityEngine.Random.Range(-moveRange.x, moveRange.x);
            float randomY = UnityEngine.Random.Range(-moveRange.y, moveRange.y);
            return new Vector3(randomX, randomY, 0);
        }
        
        // BOOL terminar movimiento o tocar pared
        private bool CanGetNewDirection()
        {
            if (Vector3.Distance(transform.position, movePosition) < minMoveDistanceCheck)
            {
                return true;
            }
            
            Collider2D[] results = new Collider2D[10];
            int collisions = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, results, obstacleMask);

            if (collisions > 0)
            {
                for (int i = 0; i < collisions; i++)
                {
                    if (results[i] != null)
                    {
                        Vector3 opositeDirection = -moveDirection;
                        transform.position += opositeDirection * 0.1f;
                        return true;
                    }
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (debug == false) return;

            if (randomMove)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(transform.position, moveRange * 2);
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, movePosition);
            
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}