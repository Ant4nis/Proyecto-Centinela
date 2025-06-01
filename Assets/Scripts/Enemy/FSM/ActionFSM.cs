using UnityEngine;

namespace Enemy.FSM
{
    public abstract class ActionFSM : MonoBehaviour
    {
        public abstract void ExecuteAction();
    }
}
