using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Representa una tarjeta visual de sesión de usuario.
    /// Este script debe colocarse en el prefab y exponer sus elementos para uso externo.
    /// Además, permite seleccionar la tarjeta y notificar al manager de selección.
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

        [Header("Botón de selección")]
        [Tooltip("Botón que representa esta tarjeta y detecta clics")]
        [SerializeField] private Button selectButton;

        private int usuarioId;
        private string nombreUsuario;

        private void Awake()
        {
            // Asigna el evento OnClick dinámicamente
            if (selectButton != null)
                selectButton.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Inicializa los datos asociados a esta tarjeta.
        /// </summary>
        /// <param name="id">ID del usuario representado</param>
        /// <param name="nombre">Nombre del usuario</param>
        public void Inicializar(int id, string nombre)
        {
            usuarioId = id;
            nombreUsuario = nombre;
        }

        /// <summary>
        /// Método llamado al hacer clic sobre la tarjeta.
        /// Notifica al manager de selección.
        /// </summary>
        private void OnClick()
        {
            Debug.Log($"🔍 Tarjeta seleccionada: ID = {usuarioId}, Nombre = {nombreUsuario}");
            SesionSelectionManager.Instance.SelectUser(usuarioId, nombreUsuario);
        }
    }
}