using System;
using UnityEngine;
using InputSystem;

namespace Player
{
    /// <summary>
    /// Componente que gestiona la lectura de entradas del jugador utilizando el nuevo sistema de InputActions.
    /// 
    /// Funcionalidades:
    /// 1. Inicializa y configura las acciones de entrada del jugador.
    /// 2. Lee la dirección de movimiento en cada frame y la normaliza.
    /// 3. Gestiona la suscripción y desuscripción de las acciones al activarse o desactivarse el GameObject.
    /// 4. Invoca la acción de dash en el componente de movimiento cuando se detecta la entrada correspondiente.
    /// </summary>
    public class PlayerInputReader : MonoBehaviour
    {
        // Acciones de entrada del jugador generadas por el sistema de InputActions.
        private PlayerInputActions _playerActions;
        // Referencia al componente de movimiento del jugador para invocar el dash.
        private PlayerMovement _playerMovement;
        private PlayerWeapon _playerWeapon;
        
        /// <summary>Dirección de movimiento actual obtenida de la entrada del jugador (ya normalizada).</summary>
        public Vector2 MoveInput { get; private set; }
        
        private void Awake()
        {
            _playerActions = new PlayerInputActions();
            _playerMovement = GetComponentInParent<PlayerMovement>();
            _playerWeapon = GetComponentInParent<PlayerWeapon>();
        }

        /// <summary>
        /// Se suscribe a las acciones de entrada del jugador:
        /// 1. Ejecuta el dash cuando se realiza la acción de Dash.
        /// 2. Inicia el disparo continuo cuando se realiza la acción de Attack [Mantener presionado].
        /// 3. Detiene el disparo continuo cuando se cancela la acción de Attack [Soltar tecla].
        /// </summary>
        private void Start()
        {
            _playerActions.Player.Dash.performed += _ => _playerMovement.Dash();
            _playerActions.Player.Attack.performed += _ => _playerWeapon.StartFiring();
            _playerActions.Player.Attack.canceled += _ => _playerWeapon.StopFiring();
        }
        
        private void Update()
        {
            ReadMovementInput();
        }

        /// <summary>Lee la entrada de movimiento del jugador utilizando el sistema de InputActions y la normaliza.</summary>
        private void ReadMovementInput()
        {
            MoveInput = _playerActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        /// <summary>Habilita las acciones de entrada al activarse el GameObject.</summary>
        private void OnEnable()
        {
            _playerActions.Enable();
        }

        /// <summary>Deshabilita las acciones de entrada al desactivarse el GameObject.</summary>
        private void OnDisable()
        {
            _playerActions.Disable();
        }
    }
}
