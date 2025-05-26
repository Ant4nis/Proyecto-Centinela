using UIForms;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manager que mantiene el usuario seleccionado desde la lista de sesiones.
    /// 
    /// FUNCIONALIDADES:
    /// 1. Guarda el ID y nombre del usuario seleccionado.
    /// 2. Activa los botones de acción cuando hay una selección.
    /// 3. Permite a otros managers acceder al usuario actual seleccionado.
    /// </summary>
    public class SesionSelectionManager : MonoBehaviour
    {
        [Header("Botones de acción")]
        [Tooltip("Botón para editar el usuario seleccionado.")]
        [SerializeField] private GameObject editButton;

        [Tooltip("Botón para eliminar la sesión seleccionada.")]
        [SerializeField] private GameObject deleteButton;

        [Tooltip("Botón para cerrar la sesión seleccionada.")]
        [SerializeField] private GameObject logoutButton;

        public static SesionSelectionManager Instance { get; private set; }

        public int SelectedUserId { get; private set; } = -1;
        public string SelectedUserName { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        /// <summary>
        /// Marca un usuario como seleccionado y activa botones.
        /// </summary>
        public void SelectUser(int userId, string userName)
        {
            SelectedUserId = userId;
            SelectedUserName = userName;

            if (editButton != null) editButton.SetActive(true);
            if (deleteButton != null) deleteButton.SetActive(true);
            if (logoutButton != null) logoutButton.SetActive(true);
        }

        /// <summary>
        /// Limpia la selección y desactiva los botones.
        /// </summary>
        public void ClearSelection()
        {
            SelectedUserId = -1;
            SelectedUserName = null;

            if (editButton != null) editButton.SetActive(false);
            if (deleteButton != null) deleteButton.SetActive(false);
            if (logoutButton != null) logoutButton.SetActive(false);
        }
    }
}
