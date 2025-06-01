using UnityEngine;

namespace Enemy.FSM
{
    public abstract class DecisionFSM : MonoBehaviour
    {
        public abstract bool Decide(EnemyFSM enemy);
    }
}
