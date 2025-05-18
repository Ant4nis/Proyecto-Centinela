using System;
using System.Collections;
using Items;
using Items.Weapons;
using ScriptableObjects;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Controlador del arma del jugador.
    /// 
    /// Funciones principales:
    /// 1. Instancia el arma inicial equipada.
    /// 2. Controla el ataque y disparo de armas, gestionando animaciones y munición.
    /// 3. Ajusta el orden de renderizado del arma según la dirección del movimiento del jugador.
    /// </summary>
    public class PlayerWeapon : MonoBehaviour
    {
        [Header("Configuración Inicial")]
        [Tooltip("Prefab del arma con la que inicia el jugador.")]
        [SerializeField] private Weapon startingWeapon;
        [Tooltip("Transform donde se instanciará el arma.")]
        [SerializeField] private Transform weaponRotationPos;

        [Header("Config Dirección (Solo Sorting)")]
        [Tooltip("Configuración para aplicar orden de renderizado según dirección.")]
        [SerializeField] private WeaponDirectionConfig directionConfig;

        [Header("Datos del arma equipada")]
        [Tooltip("Datos del arma equipada actualmente (daño, cadencia, tipo).")]
        [SerializeField] private ItemWeapon itemWeapon;

        private SpriteRenderer _spriteRendererSprite;
        private SpriteRenderer _spriteRendererAmmo;

        private PlayerInputReader _playerInputReader;
        private PlayerMovement _playerMovement;
        private PlayerAnimationController _playerAnimationController;
        private PlayerAmmo _playerAmmo;

        private Weapon _currentWeapon;
        private Coroutine _fireRoutine;
        private bool _isAttacking;
        private float _lastShootTime = Mathf.NegativeInfinity;

        private int _indexWeapon; // para cambiar entre armas
        private Weapon[] _equippedWeapons = new Weapon[2]; // max dos armas equipadas

        /// <summary>Arma actual equipada por el jugador.</summary>
        public Weapon CurrentWeapon => _currentWeapon;

        /// <summary>Indica si el jugador está actualmente atacando.</summary>
        public bool IsAttacking => _isAttacking;

        private void Awake()
        {
            _playerInputReader = GetComponentInChildren<PlayerInputReader>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAnimationController = GetComponentInChildren<PlayerAnimationController>();
            _playerAmmo = GetComponent<PlayerAmmo>();
        }

        private void Start()
        {
            CreateWeapon(startingWeapon);
        }

        private void Update()
        {
            if (_playerMovement.IsMoving)
                UpdateWeaponSorting(_playerInputReader.MoveInput);
        }

        /// <summary>
        /// Instancia el arma proporcionada y guarda las referencias visuales.
        /// </summary>
        /// <param name="weaponPrefab">Prefab del arma a instanciar.</param>
        private void CreateWeapon(Weapon weaponPrefab)
        {
            _currentWeapon = Instantiate(weaponPrefab, weaponRotationPos.position, Quaternion.identity, weaponRotationPos);
            _spriteRendererSprite = _currentWeapon.transform.Find("Sprite")?.GetComponent<SpriteRenderer>();
            _spriteRendererAmmo = _currentWeapon.transform.Find("Ammo")?.GetComponent<SpriteRenderer>();
            _equippedWeapons[_indexWeapon] = _currentWeapon;
            
            if (_spriteRendererSprite == null || _spriteRendererAmmo == null)
                Debug.LogWarning("No se encontraron los SpriteRenderers del arma.");
        }

        /// <summary>
        /// Inicia el disparo continuo si hay munición disponible.
        /// </summary>
        public void StartFiring()
        {
            if (_fireRoutine != null) return;
            
            if (WeaponWithAmmo() == false) return;

            _playerAnimationController.SetAttacking(true);
            _fireRoutine = StartCoroutine(FireContinuously());
        }

        /// <summary>
        /// Detiene el disparo continuo y resetea la animación de ataque.
        /// </summary>
        public void StopFiring()
        {
            if (_fireRoutine != null)
            {
                StopCoroutine(_fireRoutine);
                _fireRoutine = null;
                _playerAnimationController.SetAttacking(false);
            }
        }

        /// <summary>
        /// Corrutina que intenta disparar continuamente mientras se mantenga el disparo.
        /// </summary>
        private IEnumerator FireContinuously()
        {
            while (true)
            {
                ShootWeapon();
                yield return null;
            }
        }

        /// <summary>
        /// Lanza un disparo si el arma está lista y hay munición disponible.
        /// Controla el gasto de munición y el cooldown entre disparos.
        /// </summary>
        private void ShootWeapon()
        {
            if (_currentWeapon == null) return;

            if (Time.time < _lastShootTime + itemWeapon.TimeBetweenAttacks) return;

            _lastShootTime = Time.time;

            _playerAmmo.SpendAmmo(_currentWeapon.ItemWeapon.Ammo);

            StartCoroutine(IEShootWeapon());
        }

        /// <summary>
        /// Controla la secuencia de ataque: reproduce animación y llama al disparo del arma.
        /// </summary>
        private IEnumerator IEShootWeapon()
        {
            _isAttacking = true;

            yield return null; // Deja margen para animación de ataque (por ejemplo, mover el brazo antes de disparar)

            _currentWeapon.Fire();

            yield return new WaitForSeconds(itemWeapon.TimeBetweenAttacks);

            _isAttacking = false;
        }

        /// <summary>
        /// Verifica si el arma puede disparar, en base al tipo de arma y munición disponible.
        /// </summary>
        /// <returns>True si puede disparar, False si no tiene munición (en armas a distancia).</returns>
        private bool WeaponWithAmmo()
        {
            if (_currentWeapon.ItemWeapon.Type == WeaponType.Distance && _playerAmmo.HaveAmmo)
                return true;

            if (_currentWeapon.ItemWeapon.Type == WeaponType.Melee)
                return true;

            return false;
        }

        private void EquipWeapon(Weapon weapon)
        {
            if (_equippedWeapons[0] == null)
            {
                CreateWeapon(weapon);
                return;
            }
            
            
        }
        
        /// <summary>
        /// Actualiza el orden de renderizado del arma en función de la dirección del movimiento.
        /// </summary>
        /// <param name="direction">Dirección de movimiento del jugador.</param>
        private void UpdateWeaponSorting(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            var dirEnum = GetClosestDirection(direction.normalized);
            var config = directionConfig.GetSettingsFor(dirEnum);
            if (config == null) return;

            if (_spriteRendererSprite != null)
                _spriteRendererSprite.sortingOrder = config.sortingOrder;

            if (_spriteRendererAmmo != null)
                _spriteRendererAmmo.sortingOrder = config.sortingOrder;
        }

        /// <summary>
        /// Convierte una dirección en vector a una dirección cardinal de 8 direcciones.
        /// </summary>
        /// <param name="input">Vector de dirección.</param>
        /// <returns>Dirección cardinal aproximada.</returns>
        private Direction8 GetClosestDirection(Vector2 input)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            if (angle >= 337.5f || angle < 22.5f) return Direction8.Right;
            if (angle < 67.5f) return Direction8.UpRight;
            if (angle < 112.5f) return Direction8.Up;
            if (angle < 157.5f) return Direction8.UpLeft;
            if (angle < 202.5f) return Direction8.Left;
            if (angle < 247.5f) return Direction8.DownLeft;
            if (angle < 292.5f) return Direction8.Down;
            return Direction8.DownRight;
        }
    }
}
