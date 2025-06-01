using Items.Weapons;
using UnityEngine;
using Enemy.FSM;

namespace Enemy
{
    /// <summary>
    /// Controla el arma de un enemigo y gestiona su rotación y disparo.
    /// 
    /// Funcionalidades:
    /// 1. Instancia el arma inicial al despertar.
    /// 2. Rota el arma hacia el objetivo si existe.
    /// 3. Permite disparar mediante un método público.
    /// </summary>
    public class EnemyWeapon : MonoBehaviour
    {
        [Header("Configuration")] 
        [Tooltip("Objeto arma que se instanciará al inicio.")] 
        [SerializeField] private Weapon initialWeapon;

        [Tooltip("Transform desde el cual se rotará y posicionará el arma.")] 
        [SerializeField] private Transform weaponRotationPoint;


        private Weapon currentWeapon;
        private SpriteRenderer spriteRenderer;
        private EnemyFSM enemyFSM;

        /// <summary>Referencia pública al arma actual instanciada.</summary>
        public Weapon CurrentWeapon => currentWeapon;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            enemyFSM = GetComponent<EnemyFSM>();
            CreateWeapon();
        }

        private void Update()
        {
            if (currentWeapon == null || enemyFSM == null) return;

            Transform detectedPlayer = enemyFSM.Player;
            if (detectedPlayer == null) return; // aún no detectado

            Vector3 direction = detectedPlayer.position - transform.position;
            RotateWeaponTowards(direction);

        }

        /// <summary>
        /// Instancia el arma inicial y la coloca como hija del punto de rotación.
        /// </summary>
        private void CreateWeapon()
        {
            currentWeapon = Instantiate(
                initialWeapon,
                weaponRotationPoint.position,
                Quaternion.identity,
                weaponRotationPoint
            );
        }

        /// <summary>
        /// Rota el arma hacia la dirección indicada y ajusta la escala según la orientación.
        /// </summary>
        /// <param name="direction">Dirección hacia la que apuntar el arma.</param>
        private void RotateWeaponTowards(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (direction.x > 0f)
            {
                weaponRotationPoint.localScale = Vector3.one;
                currentWeapon.transform.localScale = Vector3.one;
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0f)
            {
                weaponRotationPoint.localScale = new Vector3(-1, 1, 1);
                currentWeapon.transform.localScale = new Vector3(1, -1, 1);
                spriteRenderer.flipX = true;
            }

            currentWeapon.transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

        /// <summary>
        /// Dispara el arma actual si está disponible.
        /// </summary>
        public void TryShoot()
        {
            if (currentWeapon == null) return;
            currentWeapon.Fire();
        }
    }
}
