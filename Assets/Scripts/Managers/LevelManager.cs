using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        
        [FormerlySerializedAs("templates")]
        [Header("Plantillas")]
        [SerializeField] private TemplatesRoom templatesRoom;

        public TemplatesRoom TemplatesRoom => templatesRoom;
        
        private void Awake()
        {
            Instance = this;
        }
        
    }
}
