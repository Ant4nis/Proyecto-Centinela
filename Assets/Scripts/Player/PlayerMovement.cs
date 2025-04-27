using System.Collections;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Componente que permite mover al jugador en 8 direcciones utilizando físicas (Rigidbody2D).
    /// Además, incorpora funcionalidad de dash que aumenta temporalmente la velocidad de movimiento
    /// y modifica la transparencia del sprite.
    /// 
    /// Funciones:
    /// 1. Inicializa los componentes esenciales (Rigidbody2D, PlayerInputReader y SpriteRenderer).
    /// 2. Gestiona el movimiento continuo del jugador en función de la entrada recibida.
    /// 3. Permite activar un dash que incrementa la velocidad y ajusta la transparencia del sprite durante un tiempo determinado.
    /// 4. Restaura el estado normal del jugador una vez finalizado el dash.
    /// 5. Permite actualizar el estado de colisión con paredes para evitar movimientos no deseados.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Velocidad de Movimiento"), Tooltip("Velocidad a la que se mueve el jugador")]
        [SerializeField] private float moveSpeed = 3f;
        [Tooltip("Velocidad a la que se mueve al realizar un dash")]
        [SerializeField] private float dashSpeed = 5f;
        
        [Header("Configuración de Dash"), Tooltip("Tiempo en segundos que dura el dash")]
        [SerializeField] private float dashTime = 5f;
        [Tooltip("Valor que se asigna al canal alfa del sprite durante el dash")]
        [SerializeField] private float transparency = 1f;
        
        [Header("Referencia al Collider"), Tooltip("Componente collider del personaje (CapsuleCollider2D)")]
        [SerializeField] private CapsuleCollider2D capsule2D;
        
        private Rigidbody2D _rb2D;
        private PlayerInputReader _inputReader;
        private SpriteRenderer _spriteRenderer;

        /// <summary>Indica la velocidad actual del jugador.</summary>
        private float _currentSpeed;
        /// <summary>Indica si el jugador está realizando un Dash.</summary>
        private bool _isDashing;
        /// <summary>Indica si el jugador está tocando una pared.</summary>
        private bool _isTouchingWall;

        
        /// <summary>
        /// Propiedad que indica si el jugador se encuentra en movimiento.
        /// Retorna true si se recibe entrada de movimiento y el jugador no está en contacto con una pared.
        /// </summary>
        public bool IsMoving => _inputReader.MoveInput.magnitude > 0f && !_isTouchingWall;
        
        /// <summary>Guarda la última dirección de movimiento.</summary>
        public Vector2 LastMoveDirection { get; private set; } = Vector2.down;

        /// <summary>
        /// Inicializa los componentes esenciales del jugador.
        /// 1. Obtiene el componente Rigidbody2D para el control físico.
        /// 2. Obtiene el componente PlayerInputReader que utiliza el nuevo sistema de InputActions.
        /// 3. Obtiene el componente SpriteRenderer para modificar la transparencia durante el dash.
        /// </summary>
        private void Awake()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _inputReader = GetComponentInChildren<PlayerInputReader>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        /// <summary>
        /// Inicializa la velocidad actual del jugador.
        /// </summary>
        private void Start()
        {
            _currentSpeed = moveSpeed;
        }

        /// <summary>
        /// Se ejecuta en cada actualización de físicas.
        /// Aplica el movimiento del jugador basándose en la entrada obtenida mediante PlayerInputReader.
        /// </summary>
        private void FixedUpdate()
        { 
            Move(_inputReader.MoveInput);
        }

        /// <summary>
        /// Aplica el movimiento del jugador en función de la dirección proporcionada.
        /// Calcula la nueva posición y la establece mediante Rigidbody2D.MovePosition.
        /// </summary>
        /// <param name="direction">Dirección normalizada en la que se moverá el jugador.</param>
        private void Move(Vector2 direction)
        {
            if (direction.magnitude > 0.1f)
            {
                LastMoveDirection = direction.normalized;
            }

            _rb2D.MovePosition(_rb2D.position + direction * (_currentSpeed * Time.fixedDeltaTime));
        }

        /// <summary>
        /// Activa el dash del jugador, incrementando la velocidad temporalmente y modificando la transparencia del sprite.
        /// Si ya se está ejecutando un dash, la función no hace nada.
        /// </summary>
        public void Dash()
        {
            if (_isDashing) return;
            
            _isDashing = true;
            StartCoroutine(IEDash());
        }

        /// <summary>
        /// Modifica la transparencia (canal alfa) del sprite del jugador.
        /// </summary>
        /// <param name="value">Nuevo valor para el canal alfa del sprite.</param>
        private void SpriteModifier(float value)
        {
            // Obtiene el color actual del sprite.
            Color color = _spriteRenderer.color;
            // Crea un nuevo color con la transparencia modificada.
            color = new Color(color.r, color.g, color.b, value);
            // Aplica el nuevo color al sprite.
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// Corrutina que ejecuta el dash del jugador.
        /// Aumenta la velocidad a dashSpeed, modifica la transparencia del sprite,
        /// espera durante dashTime segundos y luego restaura la velocidad y la transparencia originales.
        /// </summary>
        private IEnumerator IEDash()
        {
            _currentSpeed = dashSpeed;
            SpriteModifier(transparency);
            
            yield return new WaitForSeconds(dashTime);
            
            SpriteModifier(1f);
            _currentSpeed = moveSpeed;
            _isDashing = false;
        }

        /// <summary>
        /// Actualiza el estado de colisión del jugador con una pared.
        /// </summary>
        /// <param name="value">True si el jugador está en contacto con una pared, false en caso contrario.</param>
        public void IsTouchingWall(bool value)
        {
            _isTouchingWall = value;
        } 
    }
}
