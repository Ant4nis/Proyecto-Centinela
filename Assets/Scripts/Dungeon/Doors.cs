using System;
using UnityEngine;

namespace Dungeon
{
    /// <summary>
    /// Controlador de animaciones para las puertas dentro del sistema de habitaciones del dungeon.
    /// 
    /// Funcionalidades:
    /// 1. Referencia interna al componente Animator.
    /// 2. Proporciona métodos públicos para abrir y cerrar puertas mediante triggers animados.
    /// </summary>
    public class Doors : MonoBehaviour
    {
        /// <summary>
        /// Hash para el trigger "OpenDoor" del Animator.
        /// </summary>
        private static readonly int OpenDoor = Animator.StringToHash("OpenDoor");

        /// <summary>
        /// Hash para el trigger "CloseDoor" del Animator.
        /// </summary>
        private static readonly int CloseDoor = Animator.StringToHash("CloseDoor");

        /// <summary>
        /// Referencia al componente Animator que controla la animación de la puerta.
        /// </summary>
        private Animator _animator;

        /// <summary>
        /// Inicializa la referencia al Animator al iniciar el componente.
        /// </summary>
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Lanza el trigger de apertura en el Animator para abrir la puerta.
        /// </summary>
        public void OpenDoors()
        {
            _animator.SetTrigger(OpenDoor);
        }

        /// <summary>
        /// Lanza el trigger de cierre en el Animator para cerrar la puerta.
        /// </summary>
        public void CloseDoors()
        {
            _animator.SetTrigger(CloseDoor);
        }
    }
}
