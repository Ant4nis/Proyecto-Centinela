using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerSelection : MonoBehaviour
    {
        [SerializeField] private PlayerConfiguration configPlayer;
        public PlayerConfiguration ConfigPlayer => configPlayer;

        private void OnMouseDown()
        {
            
        }
    }
}