using System;
using System.Collections.Generic;
using Dungeon;
using UnityEngine;

namespace Enemy.FSM
{
    public class EnemyFSM : MonoBehaviour
    {
        [Header("Configuracion")] 
        [SerializeField] private string originalStateID;
        
        [Header("Estados")]
        public List<StateFSM> states;
        
        public StateFSM currentState { get; private set; }
        public Room RoomParent { get;  set; }
        public Transform Player { get; set; }

        private void Start()
        {
            ChangeState(originalStateID);
        }

        private void Update()
        {
            if (currentState == null) return;
            currentState.ExecuteState(this);
        }

        public void ChangeState(string newStateID)
        {
            // Primer estado
            if (currentState == null)
            {
                currentState = SearchState(newStateID);
            }

            if (currentState.stateID == newStateID) return;
            
            currentState = SearchState(newStateID);
                
            
        }

        private StateFSM SearchState(string searchedStateID)
        {
            for (int i = 0; i < states.Count; i++)
            {
                if (states[i].stateID == searchedStateID)
                {
                    return states[i];
                }
            }
            
            return null;
        }
    }
}