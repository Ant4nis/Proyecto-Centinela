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
        // Hashes de los parámetros del Animator para mejorar el rendimiento.
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsMoving = Animator.StringToHash("Moving");

        [Header("Referencias de Input"), Tooltip("Componente que gestiona la entrada del jugador utilizando el nuevo sistema de InputActions")]
        [SerializeField] private PlayerInputReader inputReader;
        
        private Animator _animator;
        private PlayerMovement _movement;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponentInParent<PlayerMovement>();
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
            if (!_movement.IsMoving)
            {
                _animator.SetBool(IsMoving, false);
                return;
            }
            
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(X, inputReader.MoveInput.x);
            _animator.SetFloat(Y, inputReader.MoveInput.y);
        }
    }
}
