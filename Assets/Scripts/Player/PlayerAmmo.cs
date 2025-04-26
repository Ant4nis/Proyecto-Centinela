using ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerAmmo : MonoBehaviour
    {
        [Header("")] 
        [SerializeField] private PlayerConfiguration playerConfiguration;

        public bool HaveAmmo => playerConfiguration.CurrentAmmo > 0f;
        
        public void SpendAmmo(float ammo)
        {
            playerConfiguration.CurrentAmmo -= ammo;
            
            if (playerConfiguration.CurrentAmmo < 0f)
            {
                playerConfiguration.CurrentAmmo = 0;
            }
        }

        public void RecoverAmmo(float ammo)
        {
            playerConfiguration.CurrentAmmo += ammo;

            if (playerConfiguration.CurrentAmmo > playerConfiguration.MaxAmmo)
            {
                playerConfiguration.CurrentAmmo = playerConfiguration.MaxAmmo;
            }
        }
    }
}