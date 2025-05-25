using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Representa una tarjeta visual de sesión de usuario.
    /// Este script debe colocarse en el prefab y exponer sus elementos para uso externo.
    /// </summary>
    public class SesionItem : MonoBehaviour
    {
        [Header("Referencias visuales")]
        [Tooltip("Texto para el nombre del usuario")]
        public TMP_Text NombreTMP;

        [Tooltip("Texto para la última conexión")]
        public TMP_Text FechaTMP;

        [Tooltip("Texto para la IP del usuario")]
        public TMP_Text IpTMP;

        [Tooltip("Icono que representa si está conectado")]
        public Image EstadoIcon;
    }
}