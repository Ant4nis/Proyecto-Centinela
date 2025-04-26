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
    /// 1. Instancia el arma inicial.
    /// 2. Cambia el sortingOrder de sus sprites según dirección.
    /// 3. Controla la lógica de ataque con animaciones y tiempo de espera.
    /// </summary>
    public class PlayerWeapon : MonoBehaviour
    {
        [Header("Configuración Inicial")]
        [SerializeField] private Weapon startingWeapon;
        [SerializeField] private Transform weaponRotationPos;

        [Header("Config Dirección (Solo Sorting)")]
        [SerializeField] private WeaponDirectionConfig directionConfig;

        [Header("Datos del arma equipada")]
        [SerializeField] private ItemWeapon itemWeapon;

        private SpriteRenderer _spriteRendererSprite;
        private SpriteRenderer _spriteRendererAmmo;

        private PlayerInputReader _playerInputReader;
        private PlayerMovement _playerMovement;
        private PlayerAnimationController _playerAnimationController;
        private PlayerAmmo _playerAmmo;

        private Weapon _currentWeapon;
        private bool _isAttacking;
        private Coroutine _fireRoutine;
        private float _lastShootTime = Mathf.NegativeInfinity;
        
        public Weapon CurrentWeapon => _currentWeapon;
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
        /// Instancia el arma inicial del jugador y guarda referencias visuales.
        /// </summary>
        private void CreateWeapon(Weapon weaponPrefab)
        {
            _currentWeapon = Instantiate(weaponPrefab, weaponRotationPos.position, Quaternion.identity, weaponRotationPos);
            _spriteRendererSprite = _currentWeapon.transform.Find("Sprite")?.GetComponent<SpriteRenderer>();
            _spriteRendererAmmo = _currentWeapon.transform.Find("Ammo")?.GetComponent<SpriteRenderer>();

            if (_spriteRendererSprite == null || _spriteRendererAmmo == null)
                Debug.LogWarning("No se encontraron los SpriteRenderers del arma.");
        }
        
        public void StartFiring()
        {
            if (_fireRoutine != null) return;
            if (WeaponWithAmmo() == false) return;

            _playerAnimationController.SetAttacking(true); // <-- aquí empieza la animación una sola vez
            _fireRoutine = StartCoroutine(FireContinuously());
        }

        public void StopFiring()
        {
            if (_fireRoutine != null)
            {
                StopCoroutine(_fireRoutine);
                _fireRoutine = null;
                _playerAnimationController.SetAttacking(false); // <-- la param se resetea aquí al soltar botón
            }
        }

        private IEnumerator FireContinuously()
        {
            while (true)
            {
                ShootWeapon(); // deja que el cooldown lo controle ShootWeapon
                yield return null; // intentamos cada frame, pero el cooldown lo limita
            }
            
        }

        /// <summary>
        /// Lanza un disparo si no se está atacando.
        /// </summary>
        public void ShootWeapon()
        {
            if (_currentWeapon == null) return;

            // Evita disparar si no ha pasado el tiempo necesario
            if (Time.time < _lastShootTime + itemWeapon.TimeBetweenAttacks) return;

            _lastShootTime = Time.time;
            
            _playerAmmo.SpendAmmo(_currentWeapon.ItemWeapon.Ammo);

            StartCoroutine(IEShootWeapon());
            
        }

        /// <summary>
        /// Controla la secuencia de ataque: animación, espera y disparo.
        /// </summary>
        private IEnumerator IEShootWeapon()
        {
            _isAttacking = true;

            yield return null; // dejar que la animación mueva el brazo/arma si hace falta

            _currentWeapon.Fire();

            yield return new WaitForSeconds(itemWeapon.TimeBetweenAttacks); // opcional

            _isAttacking = false;
        }

        private bool WeaponWithAmmo()
        {
            if (_currentWeapon.ItemWeapon.Type == WeaponType.Distance && _playerAmmo.HaveAmmo)
            {
                return true;
            }

            if (_currentWeapon.ItemWeapon.Type == WeaponType.Melee)
            {
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Aplica el `sortingOrder` correcto del arma según la dirección de movimiento.
        /// </summary>
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
        /// Devuelve una dirección cardinal aproximada a partir de un vector.
        /// </summary>
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
