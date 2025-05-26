using Managers;
using UnityEngine;
using Extra;

namespace UIForms
{
    /// <summary>
    /// Controlador de acciones asociadas a los botones del panel de sesiones.
    /// FUNCIONALIDADES:
    /// 1. Editar: abre el panel de perfil con los datos del usuario seleccionado.
    /// 2. Eliminar: elimina la sesión del usuario seleccionado.
    /// 3. Cerrar sesión: (TODO) acción futura.
    /// </summary>
    public class SesionButtonActions : MonoBehaviour
    {
        [Header("Paneles")]
        [Tooltip("Panel de perfil que se mostrará al editar")] 
        [SerializeField] private GameObject perfilPanel;

        [Tooltip("Panel de sesiones que se ocultará al editar")] 
        [SerializeField] private GameObject sesionesPanel;

        [Tooltip("Manager de perfil que cargará los datos")] 
        [SerializeField] private PerfilPanelManager perfilPanelManager;
        [SerializeField] private SesionListManager sesionListManager;

        /// <summary>
        /// Abre el panel de perfil con los datos del usuario seleccionado.
        /// </summary>
        public void EditarUsuarioSeleccionado()
        {
            int id = SesionSelectionManager.Instance.SelectedUserId;
            if (id < 0)
            {
                Debug.LogWarning("No hay usuario seleccionado para editar.");
                return;
            }

            SesionBridge.UsuarioIdParaEdicion = id; // Usamos el puente

            if (sesionesPanel != null) sesionesPanel.SetActive(false);
            if (perfilPanel != null) perfilPanel.SetActive(true);

            if (perfilPanelManager != null)
                perfilPanelManager.AbrirPanelPerfil();
            else
                Debug.LogWarning("PerfilManager no asignado en SesionButtonActions.");
        }

        /// <summary>
        /// Elimina la sesión seleccionada del backend.
        /// </summary>
        public void EliminarSesionSeleccionada()
        {
            int id = SesionSelectionManager.Instance.SelectedUserId;
            if (id < 0)
            {
                Debug.LogWarning("No hay sesión seleccionada para eliminar.");
                return;
            }

            StartCoroutine(EliminarSesion(id));
        }

        private System.Collections.IEnumerator EliminarSesion(int id)
        {
            string url = $"http://localhost:5000/api/sesion/{id}";
            using var request = UnityEngine.Networking.UnityWebRequest.Delete(url);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Sesión eliminada correctamente.");
                SesionSelectionManager.Instance.ClearSelection();
                sesionListManager?.RefrescarSesiones();            }
            else
            {
                Debug.LogWarning("Error al eliminar la sesión: " + request.downloadHandler.text);
            }
        }

        /// <summary>
        /// Cierra la sesión seleccionada. (Por implementar)
        /// </summary>
        public void CerrarSesionSeleccionada()
        {
            Debug.Log("TODO: Cerrar sesión seleccionada.");
        }
    }
}