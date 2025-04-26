using Managers;
using Player;
using UnityEngine;

namespace Items.Weapons
{
    /// <summary>
    /// Arma de tipo pistola que dispara proyectiles desde un pool.
    /// </summary>
    public class GunWeapon : Weapon
    {
        [Header("Proyectil")]
        [Tooltip("Prefab del proyectil a usar.")]
        [SerializeField] protected Projectile projectilePrefab;

        public override void Fire()
        {
            Animate();

            // Dirección base
            Vector3 direction = shootingPosition.right.normalized;

            // Aplica dispersión
            float spreadAngle = Random.Range(itemWeapon.MinAccuracy, itemWeapon.MaxAccuracy);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngle);
            direction = spreadRotation * direction;

            // Obtener proyectil desde el pool
            Projectile projectile = ProjectilePool.Instance.GetProjectile(projectilePrefab);
            projectile.OriginalPrefab = projectilePrefab; // necesario para devolver al pool
            //projectile.ProjectileDamage = itemWeapon.Damage;

            // Reinicia estado y activa
            projectile.ResetProjectile(shootingPosition.position, direction);
        }
    }
}