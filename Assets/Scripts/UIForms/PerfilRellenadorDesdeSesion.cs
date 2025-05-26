using System.Collections;
using TMPro;
using UIForms;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    /// <summary>
    /// Controlador para rellenar el panel de perfil con los datos del usuario seleccionado desde Sesiones.
    /// </summary>
    public class PerfilRellenadorDesdeSesion : MonoBehaviour
    {
        [Header("Panel de edición de perfil")]
        [Tooltip("Panel visual que se mostrará para editar el usuario")] 
        [SerializeField] private GameObject perfilPanel;

        [Header("Campos de entrada")]
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputEmail;
        [SerializeField] private TMP_InputField inputPassword;

        [Header("Campo de rol solo visible si es admin")]
        [SerializeField] private GameObject adminRoleContainer;
        [SerializeField] private TMP_Dropdown roleDropdown;

        /// <summary>
        /// Método que se llamará desde el botón Editar en Sesiones.
        /// </summary>
        public void CargarPerfilDeUsuarioSeleccionado()
        {
            int id = SesionSelectionManager.Instance.SelectedUserId;
            if (id <= 0)
            {
                Debug.LogWarning("No hay usuario seleccionado para editar.");
                return;
            }
            StartCoroutine(ObtenerDatosUsuario(id));
        }

        private IEnumerator ObtenerDatosUsuario(int id)
        {
            using var request = UnityWebRequest.Get($"http://localhost:5000/api/usuario/{id}");
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                UsuarioDTO usuario = JsonUtility.FromJson<UsuarioDTO>(request.downloadHandler.text);

                inputName.text = usuario.nombreUsuario;
                inputEmail.text = usuario.email;
                inputPassword.text = "";

                if (adminRoleContainer != null)
                {
                    bool esAdmin = usuario.rol != null && usuario.rol.nombre == "Administrador";
                    adminRoleContainer.SetActive(esAdmin);

                    if (esAdmin && roleDropdown != null)
                    {
                        int index = roleDropdown.options.FindIndex(opt => opt.text == usuario.rol.nombre);
                        if (index != -1) roleDropdown.value = index;
                    }
                }

                if (perfilPanel != null)
                    perfilPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Error al obtener datos del usuario: " + request.downloadHandler.text);
            }
        }

        /// <summary>
        /// DTO para mapear los datos recibidos del backend.
        /// </summary>
        [System.Serializable]
        private class UsuarioDTO
        {
            public int id;
            public string nombreUsuario;
            public string email;
            public RolDTO rol;
        }

        [System.Serializable]
        private class RolDTO
        {
            public int id;
            public string nombre;
        }
    }
}
