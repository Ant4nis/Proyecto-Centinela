using System;

namespace Enemy.FSM
{
    [Serializable]
    public class TransitionFSM
    {
        public DecisionFSM decide;
        public string trueState;
        public string falseState;
    }
}
