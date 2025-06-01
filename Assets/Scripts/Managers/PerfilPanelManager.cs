using System.Collections;
using TMPro;
using UIForms;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Extra; 

namespace Managers
{
    public class PerfilPanelManager : MonoBehaviour
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

        private int usuarioEditadoId;
        private static int respaldoIdEditado; //  Se mantiene entre activaciones

        public int UsuarioEditadoId
        {
            get => usuarioEditadoId != 0 ? usuarioEditadoId : respaldoIdEditado;
            set
            {
                usuarioEditadoId = value;
                respaldoIdEditado = value;

                Debug.Log($"🛡 SET UsuarioEditadoId ← {value} (desde {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name})");
            }
        }
        
        
        public void AbrirPanelPerfil()
        {
            if (SesionBridge.UsuarioIdParaEdicion.HasValue)
            {
                int id = SesionBridge.UsuarioIdParaEdicion.Value;
                StartCoroutine(CargarDatosDesdeAPI(id, limpiarBridge: true)); // <- nuevo parámetro
            }
            else
            {
                CargarPerfilActual();
            }
        }

        public void AskDeleteConfirmation()
        {
            if (confirmDeleteButton != null)
                confirmDeleteButton.SetActive(true);
        }

        public void ConfirmDelete()
        {
            Debug.Log("🗑 Ejecutando ConfirmDelete()");

            StartCoroutine(SendDeleteRequest());
        }
        
        private IEnumerator SendDeleteRequest()
        {
            string url = $"http://localhost:5000/api/usuario/{UsuarioEditadoId}";
            using var request = UnityWebRequest.Delete(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"✅ Cuenta con ID={UsuarioEditadoId} eliminada correctamente.");

                if (UsuarioEditadoId == UsuarioSesion.Instance.Id)
                {
                    Debug.Log("Usuario eliminado era el actual. Limpiando sesión y volviendo al login.");
                    UsuarioSesion.Instance.Reset();
                    SceneManager.LoadScene("LoginScene");
                }
                else
                {
                    Debug.Log("Usuario eliminado era otro. Cerrando panel de perfil.");
                    mainMenuPanel.SetActive(true);
                    profilePanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning(request.downloadHandler.text);
                errorText.text = "Error al eliminar la cuenta.";
            }
        }
        
        public void SubmitEdit()
        {
            StartCoroutine(SendEditRequest());
        }

        private IEnumerator SendEditRequest()
        {
            errorText.text = "";

            Debug.Log($"Verificación antes de PUT: UsuarioEditadoId = {UsuarioEditadoId}");
            if (UsuarioEditadoId <= 0)
            {
                errorText.text = "Error: no se ha definido correctamente el usuario a editar.";
                Debug.LogError("usuarioEditadoId no válido en SubmitEdit()");
                yield break;
            }

            string newName = inputName.text.Trim();
            string newEmail = inputEmail.text.Trim();
            string newPassword = inputPassword.text.Trim();

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newEmail))
            {
                errorText.text = "Rellena al menos nombre y email.";
                yield break;
            }

            int rolId = UsuarioSesion.Instance.Rol == "Administrador" && adminRoleContainer.activeSelf
                ? MapRoleNameToId(roleDropdown.options[roleDropdown.value].text)
                : UsuarioSesion.Instance.RolId;

            var userData = new UsuarioUpdateDTO
            {
                id = UsuarioEditadoId,
                nombreUsuario = newName,
                email = newEmail,
                contrasenaHash = string.IsNullOrEmpty(newPassword) ? null : newPassword,
                rolId = rolId
            };

            string json = JsonUtility.ToJson(userData);

            string url = $"http://localhost:5000/api/usuario/{UsuarioEditadoId}";
            Debug.Log($"PUT a: {url} con ID={userData.id}");

            using var request = new UnityWebRequest(url, "PUT");
            byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Usuario modificado correctamente.");

                if (UsuarioEditadoId == UsuarioSesion.Instance.Id)
                {
                    UsuarioSesion.Instance.Nombre = newName;
                    UsuarioSesion.Instance.Email = newEmail;
                    UsuarioSesion.Instance.Password = newPassword;
                    UsuarioSesion.Instance.RolId = rolId;
                    UsuarioSesion.Instance.Rol = (rolId == 2) ? "Administrador" : "Jugador";
                }

                mainMenuPanel.SetActive(true);
                profilePanel.SetActive(false);

                inputName.text = "";
                inputEmail.text = "";
                inputPassword.text = "";
            }
            else
            {
                errorText.text = "Error al guardar los cambios.";
                Debug.LogWarning(request.downloadHandler.text);
            }
        }

        public void CargarUsuarioDesdeId(int id)
        {
            StartCoroutine(CargarDatosDesdeAPI(id));
        }
        
        private IEnumerator CargarDatosDesdeAPI(int id, bool limpiarBridge = false)
        {
            string url = $"http://localhost:5000/api/usuario/{id}";
            Debug.Log($"🔍 Solicitando datos del usuario con ID = {id} → {url}");

            using var request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            Debug.Log($"🌐 Respuesta GET usuario: {request.responseCode} - {request.downloadHandler.text}");

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text
                    .Replace("\"id\"", "\"Id\"")
                    .Replace("\"nombreUsuario\"", "\"NombreUsuario\"")
                    .Replace("\"email\"", "\"Email\"")
                    .Replace("\"contrasenaHash\"", "\"ContrasenaHash\"")
                    .Replace("\"fechaRegistro\"", "\"FechaRegistro\"")
                    .Replace("\"rolId\"", "\"RolId\"")
                    .Replace("\"rol\"", "\"Rol\"")
                    .Replace("\"nombre\"", "\"Nombre\"")
                    .Replace("\"usuarios\"", "\"Usuarios\"")
                    .Replace("\"sesiones\"", "\"Sesiones\"")
                    .Replace("\"ultimaConexion\"", "\"UltimaConexion\"")
                    .Replace("\"usuarioId\"", "\"UsuarioId\"")
                    .Replace("\"leaderboards\"", "\"Leaderboards\"");

                var datos = JsonUtility.FromJson<UsuarioDTOWrapper>(json);

                if (datos == null)
                {
                    Debug.LogError("JsonUtility devolvió null al deserializar.");
                    errorText.text = "Error al leer los datos del usuario.";
                    yield break;
                }

                Debug.Log($"📦 Datos cargados: {datos.Id}, {datos.NombreUsuario}, {datos.Email}, Rol: {datos.Rol?.Nombre}");

                UsuarioEditadoId = datos.Id;
                inputName.text = datos.NombreUsuario;
                inputEmail.text = datos.Email;
                inputPassword.text = "";

                if (adminRoleContainer != null)
                {
                    bool esAdmin = datos.Rol != null && datos.Rol.Nombre == "Administrador";
                    adminRoleContainer.SetActive(esAdmin || UsuarioSesion.Instance.Rol == "Administrador");

                    int index = roleDropdown.options.FindIndex(opt => opt.text == datos.Rol?.Nombre);
                    if (index != -1) roleDropdown.value = index;
                }

                if (confirmDeleteButton != null)
                    confirmDeleteButton.SetActive(false);
                if (limpiarBridge)
                    SesionBridge.Limpiar();
            }
            else
            {
                errorText.text = "Error al cargar el usuario.";
                Debug.LogWarning($" Error: {request.downloadHandler.text}");
            }
        }

        public void CargarPerfilActual()
        {
            if (UsuarioSesion.Instance == null)
            {
                Debug.LogError("UsuarioSesion no está inicializado.");
                errorText.text = "Error interno: sesión no iniciada.";
                return;
            }

            UsuarioEditadoId = UsuarioSesion.Instance.Id;

            if (UsuarioEditadoId <= 0)
            {
                Debug.LogError("ID del usuario inválido.");
                errorText.text = "Error interno: ID no válido.";
                return;
            }

            inputName.text = UsuarioSesion.Instance.Nombre;
            inputEmail.text = UsuarioSesion.Instance.Email;
            inputPassword.text = "";

            if (adminRoleContainer != null)
            {
                bool esAdmin = UsuarioSesion.Instance.Rol == "Administrador";
                adminRoleContainer.SetActive(esAdmin);

                int index = roleDropdown.options.FindIndex(opt => opt.text == UsuarioSesion.Instance.Rol);
                if (index != -1) roleDropdown.value = index;
            }

            if (confirmDeleteButton != null)
                confirmDeleteButton.SetActive(false);

            if (profilePanel != null)
                profilePanel.SetActive(true);
            
            usuarioEditadoId = UsuarioSesion.Instance.Id; // fuerza sobreescribir por si estaba cacheado
        }

        [System.Serializable]
        private class UsuarioDTOWrapper
        {
            public int Id;
            public string NombreUsuario;
            public string Email;
            public int RolId;
            public RolDTO Rol;

            [System.Serializable]
            public class RolDTO
            {
                public int Id;
                public string Nombre;
            }
        }

        private int MapRoleNameToId(string roleName)
        {
            return roleName == "Administrador" ? 2 : 1;
        }

        [System.Serializable]
        private class UsuarioUpdateDTO
        {
            public int id;
            public string nombreUsuario;
            public string email;
            public string contrasenaHash;
            public int rolId;
        }
    }
}
