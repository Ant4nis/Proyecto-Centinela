using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Enemy.FSM
{
    [Serializable]
    public class StateFSM
    {
        [FormerlySerializedAs("estadoID")] public string stateID;
        public List<ActionFSM> actions = new List<ActionFSM>();
        public List<TransitionFSM> transitions = new List<TransitionFSM>();

        public void ExecuteState(EnemyFSM enemyFsm)
        {
            ExecuteActions();
            ExecuteTransitions(enemyFsm);
        }

        private void ExecuteActions()
        {
            if (actions.Count <= 0) return;
            
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].ExecuteAction();
            }
        }

        private void ExecuteTransitions(EnemyFSM enemyFsm)
        {
            if (transitions.Count <= 0) return;

            for (int i = 0; i < transitions.Count; i++)
            {
                bool response = transitions[i].decide.Decide(enemyFsm);

                if (response)
                {
                    if (string.IsNullOrEmpty(transitions[i].trueState) == false)
                    {
                        enemyFsm.ChangeState(transitions[i].trueState);
                        break;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(transitions[i].falseState) == false)
                    {
                        enemyFsm.ChangeState(transitions[i].falseState);
                        break;
                    }
                }
            }
        }
    }
}
