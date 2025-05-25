using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIForms;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Controlador del panel de perfil de usuario.
    /// Permite modificar nombre, email, contraseña y rol (si es administrador).
    /// También permite eliminar la cuenta con confirmación.
    /// 
    /// FUNCIONALIDADES:
    /// 1. Oculta el bloque de cambio de rol si no es administrador.
    /// 2. Muestra botón de confirmación tras pulsar "Eliminar cuenta".
    /// 3. Envía los cambios al backend al pulsar "Aceptar".
    /// 4. Borra la cuenta del usuario actual si confirma eliminación.
    /// 5. Vuelve al menú principal o a la escena de login según acción.
    /// </summary>
    public class PerfilManager : MonoBehaviour
    {
        [Header("Campos de entrada")]
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputEmail;
        [SerializeField] private TMP_InputField inputPassword;

        [Header("Rol (solo para administradores)")]
        [SerializeField] private GameObject adminRoleContainer;
        [SerializeField] private TMP_Dropdown roleDropdown;

        [Header("Botones y secciones")]
        [SerializeField] private GameObject confirmDeleteButton;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject profilePanel;

        [Header("Texto de errores")]
        [SerializeField] private TMP_Text errorText;

        private void OnEnable()
        {
            if (confirmDeleteButton != null)
                confirmDeleteButton.SetActive(false);

            if (UsuarioSesion.Instancia.Rol == "Administrador")
            {
                if (adminRoleContainer != null)
                {
                    adminRoleContainer.SetActive(true);
                    int index = roleDropdown.options.FindIndex(opt => opt.text == UsuarioSesion.Instancia.Rol);
                    if (index != -1) roleDropdown.value = index;
                }
            }
            else if (adminRoleContainer != null)
            {
                adminRoleContainer.SetActive(false);
            }

            errorText.text = "";
        }

        /// <summary>
        /// Ejecutado al pulsar el botón Aceptar. Envía los datos al backend.
        /// </summary>
        public void SubmitEdit()
        {
            StartCoroutine(SendEditRequest());
        }

        private IEnumerator SendEditRequest()
        {
            errorText.text = "";

            string newName = string.IsNullOrWhiteSpace(inputName.text) ? UsuarioSesion.Instancia.Nombre : inputName.text.Trim();
            string newEmail = string.IsNullOrWhiteSpace(inputEmail.text) ? UsuarioSesion.Instancia.Email : inputEmail.text.Trim();
            string newPassword = string.IsNullOrWhiteSpace(inputPassword.text) ? UsuarioSesion.Instancia.Password : inputPassword.text.Trim();

            int rolId = UsuarioSesion.Instancia.Rol == "Administrador" && adminRoleContainer.activeSelf
                ? MapRoleNameToId(roleDropdown.options[roleDropdown.value].text)
                : UsuarioSesion.Instancia.RolId;

            var userData = new UsuarioUpdateDTO
            {
                Id = UsuarioSesion.Instancia.Id,
                NombreUsuario = newName,
                Email = newEmail,
                ContrasenaHash = newPassword,
                RolId = rolId
            };

            string json = JsonUtility.ToJson(userData);

            using var request = new UnityWebRequest($"http://localhost:5000/api/usuario/{userData.Id}", "PUT");
            byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuario modificado correctamente.");

                // Actualizar sesión local
                UsuarioSesion.Instancia.Nombre = newName;
                UsuarioSesion.Instancia.Email = newEmail;
                UsuarioSesion.Instancia.Password = newPassword;
                UsuarioSesion.Instancia.RolId = rolId;
                UsuarioSesion.Instancia.Rol = (rolId == 2) ? "Administrador" : "Jugador";

                mainMenuPanel.SetActive(true);
                profilePanel.SetActive(false);

                inputName.text = "";
                inputEmail.text = "";
                inputPassword.text = "";
            }
            else
            {
                Debug.LogWarning(request.downloadHandler.text);
                errorText.text = "Error al guardar los cambios.";
            }
        }

        /// <summary>
        /// Muestra el botón de confirmación para eliminar cuenta.
        /// </summary>
        public void AskDeleteConfirmation()
        {
            if (confirmDeleteButton != null)
                confirmDeleteButton.SetActive(true);
        }

        /// <summary>
        /// Borra la cuenta del usuario actual y vuelve al login.
        /// </summary>
        public void ConfirmDelete()
        {
            StartCoroutine(SendDeleteRequest());
        }

        private IEnumerator SendDeleteRequest()
        {
            using var request = UnityWebRequest.Delete($"http://localhost:5000/api/usuario/{UsuarioSesion.Instancia.Id}");
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Cuenta eliminada correctamente.");
                SceneManager.LoadScene("LoginScene");
            }
            else
            {
                Debug.LogWarning(request.downloadHandler.text);
                errorText.text = "Error al eliminar la cuenta.";
            }
        }

        /// <summary>
        /// Convierte el nombre del rol a su ID correspondiente.
        /// </summary>
        private int MapRoleNameToId(string roleName)
        {
            return roleName == "Administrador" ? 2 : 1;
        }

        /// <summary>
        /// Clase auxiliar para enviar datos al backend.
        /// </summary>
        [System.Serializable]
        private class UsuarioUpdateDTO
        {
            public int Id;
            public string NombreUsuario;
            public string Email;
            public string ContrasenaHash;
            public int RolId;
        }
    }
}
