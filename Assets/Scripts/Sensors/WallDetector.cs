using Player;
using UnityEngine;

namespace Sensors
{
    /// <summary>
    /// Componente que detecta la presencia de paredes utilizando un rayo (Raycast) y actualiza el estado del movimiento del jugador.
    /// 
    /// Funciones:
    /// 1. Emite un rayo desde el centro del collider en la dirección de movimiento del jugador.
    /// 2. Ajusta la longitud del rayo si la dirección en Y es positiva.
    /// 3. Determina si el rayo colisiona con un objeto que pertenece a la capa definida como pared.
    /// 4. Notifica al componente de movimiento del jugador para evitar movimientos no deseados al estar en contacto con una pared.
    /// </summary>
    public class WallDetector : MonoBehaviour
    {
        [Header("Referencias"), Tooltip("Componente que gestiona la entrada del jugador")]
        [SerializeField] private PlayerInputReader input;

        [Tooltip("Referencia al script de movimiento del jugador")]
        [SerializeField] private PlayerMovement movement;

        [Header("Configuración del Sensor"), Tooltip("Longitud base del rayo utilizado para detectar paredes")]
        [SerializeField] private float rayLength = 0.1f;
        [Tooltip("Referencia al script de movimiento del jugador")]
        [SerializeField] private float adjustedRayLength =  0.145f;

        [Header("Configuración de la Capa"), Tooltip("Capa a detectar como pared")]
        [SerializeField] private LayerMask layerWall;

        // Dirección en la que se emite el rayo, basada en la entrada del jugador.
        private Vector2 _rayDirection;

        // Componente collider del objeto para obtener el origen del rayo.
        private CapsuleCollider2D _collider;

        /// <summary>
        /// Inicializa el componente collider.
        /// </summary>
        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
        }

        /// <summary>
        /// Se ejecuta en cada actualización de físicas.
        /// Llama al sensor para detectar paredes.
        /// </summary>
        private void FixedUpdate()
        {
            SensorWall();
        }

        /// <summary>
        /// Emite un rayo desde el centro del collider en la dirección indicada por la entrada del jugador.
        /// Si el rayo detecta un objeto en la capa especificada, se notifica al componente de movimiento.
        /// La longitud del rayo se incrementa ligeramente si la dirección Y es positiva.
        /// </summary>
        private void SensorWall()
        {
            // Obtiene el centro del collider como origen del rayo.
            var origin = _collider.bounds.center;
            // Dirección del input actual.
            _rayDirection = input.MoveInput;
            // Para ajustar la longitud del rayo si se pulsa en dirección Y.
            float finalRayLength = rayLength;

            if (_rayDirection.y != 0 && _rayDirection.x == 0)
            {
                finalRayLength += adjustedRayLength;
            }
                
            /*
             if (_rayDirection is { y: > 0, x: 0 })
                adjustedRayLength += 0.045f;
            */
            
            // Dibuja el rayo en la escena para fines de depuración (solo visible en el editor).
            Debug.DrawRay(origin, _rayDirection * finalRayLength, Color.red);
            // Emite el rayo y verifica si colisiona con un objeto en la capa de pared.
            RaycastHit2D hit = Physics2D.Raycast(origin, _rayDirection, finalRayLength, layerWall);
            // Actualiza el estado de colisión en el script de movimiento.
            movement.IsTouchingWall(hit.collider != null);
        }
    }
}
