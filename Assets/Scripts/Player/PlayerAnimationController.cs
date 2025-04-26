using UnityEngine;

namespace Player
{
    /// <summary>
    /// Componente que gestiona las animaciones del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Inicializa el componente Animator y la referencia al movimiento del jugador.
    /// 2. Actualiza las direcciones de animación basándose en la entrada del jugador (nuevo sistema InputActions).
    /// 3. Establece las variables del Animator para reflejar el estado de movimiento y dirección.
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        // Hashes de los parámetros del Animator para mejorar el rendimiento y prevenir errores de escritura.
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsMoving = Animator.StringToHash("Moving");
        private static readonly int IsAttacking = Animator.StringToHash("Attacking");

        [Header("Referencias de Input"), Tooltip("Componente que gestiona la entrada del jugador")]
        [SerializeField] private PlayerInputReader inputReader;
        
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
        /// Actualiza las variables del Animator en función del estado de movimiento y la entrada del jugador.
        /// 
        /// Funcionalidades:
        /// 1. Si el jugador no se está moviendo, desactiva la animación de movimiento, transicionando a estado Idle.
        /// 2. Si el jugador se mueve, activa la animación de movimiento y actualiza las direcciones 'X' y 'Y'.
        /// </summary>
        private void UpdateDirections()
        {
            if (_playerWeapon.IsAttacking) return;
            
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

        public void SetAttacking(bool isAttacking)
        {
            _animator.SetBool(IsAttacking, isAttacking);
        }
    }
}
