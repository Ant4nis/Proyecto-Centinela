using UnityEngine;

namespace Player
{
    /// <summary>
    /// Componente que gestiona y actualiza las animaciones del jugador según su entrada y estado de combate.
    /// 
    /// Funcionalidades:
    /// 1. Inicializa el Animator y referencias necesarias.
    /// 2. Actualiza continuamente las direcciones de movimiento para el Animator.
    /// 3. Controla la transición entre estados del jugador [Ataque, movimiento, idle...].
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Referencias de Input")]
        [Tooltip("Componente encargado de leer la entrada del jugador.")]
        [SerializeField] private PlayerInputReader inputReader;

        // Hashes precomputados de los parámetros del Animator para mejorar el rendimiento en tiempo de ejecución.
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsMoving = Animator.StringToHash("Moving");
        private static readonly int IsAttacking = Animator.StringToHash("Attacking");
        
        private Animator _animator;
        private PlayerMovement _movement;
        private PlayerWeapon _playerWeapon;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponentInParent<PlayerMovement>();
            _playerWeapon = GetComponentInParent<PlayerWeapon>();
        }

        private void Update()
        {
            UpdateDirections();
        }

        /// <summary>
        /// Actualiza los parámetros de dirección y movimiento del Animator.
        /// 
        /// Funcionalidades:
        /// 1. Si el jugador está atacando, bloquea el control de movimiento visual.
        /// 2. Si no está atacando:
        ///     - Si el jugador se mueve, actualiza la dirección actual.
        ///     - Si está detenido, mantiene la última dirección de movimiento.
        /// </summary>
        private void UpdateDirections()
        {
            if (_playerWeapon.IsAttacking) return; // No actualizar movimiento si está atacando.

            Vector2 direction;

            if (_movement.IsMoving)
            {
                direction = inputReader.MoveInput;
                _animator.SetBool(IsMoving, true);
            }
            else
            {
                direction = _movement.LastMoveDirection;
                _animator.SetBool(IsMoving, false);
            }

            _animator.SetFloat(X, direction.x);
            _animator.SetFloat(Y, direction.y);
        }

        /// <summary>
        /// Cambia el estado del Animator para reflejar si el jugador está atacando.
        /// </summary>
        /// <param name="isAttacking">True si el jugador está atacando; False si no.</param>
        public void SetAttacking(bool isAttacking)
        {
            _animator.SetBool(IsAttacking, isAttacking);
        }
    }
}
