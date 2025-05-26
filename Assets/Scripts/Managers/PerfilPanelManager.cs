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
        public int UsuarioEditadoId
        {
            get => usuarioEditadoId;
            set
            {
                Debug.Log($"🛡 SET UsuarioEditadoId ← {value} (desde {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name})");

                usuarioEditadoId = value;
            }
        }
        
        
        public void AbrirPanelPerfil()
        {
            if (SesionBridge.UsuarioIdParaEdicion.HasValue)
            {
                CargarUsuarioDesdeId(SesionBridge.UsuarioIdParaEdicion.Value);
                SesionBridge.Limpiar();
            }
            else
            {
                CargarPerfilActual();
            }
        }

        public void SubmitEdit()
        {
            StartCoroutine(SendEditRequest());
        }

        private IEnumerator SendEditRequest()
        {
            errorText.text = "";

            Debug.Log($"📌 Verificación antes de PUT: UsuarioEditadoId = {UsuarioEditadoId}");
            if (UsuarioEditadoId <= 0)
            {
                errorText.text = "Error: no se ha definido correctamente el usuario a editar.";
                Debug.LogError("❌ usuarioEditadoId no válido en SubmitEdit()");
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

            int rolId = UsuarioSesion.Instancia.Rol == "Administrador" && adminRoleContainer.activeSelf
                ? MapRoleNameToId(roleDropdown.options[roleDropdown.value].text)
                : UsuarioSesion.Instancia.RolId;

            var userData = new UsuarioUpdateDTO
            {
                Id = UsuarioEditadoId,
                NombreUsuario = newName,
                Email = newEmail,
                ContrasenaHash = string.IsNullOrEmpty(newPassword) ? null : newPassword,
                RolId = rolId
            };

            string json = JsonUtility.ToJson(userData)
                .Replace("\"Id\"", "\"id\"")
                .Replace("\"NombreUsuario\"", "\"nombreUsuario\"")
                .Replace("\"Email\"", "\"email\"")
                .Replace("\"ContrasenaHash\"", "\"contrasenaHash\"")
                .Replace("\"RolId\"", "\"rolId\"");

            string url = $"http://localhost:5000/api/usuario/{UsuarioEditadoId}";
            Debug.Log($"PUT a: {url} con ID={userData.Id}");

            using var request = new UnityWebRequest(url, "PUT");
            byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Usuario modificado correctamente.");

                if (UsuarioEditadoId == UsuarioSesion.Instancia.Id)
                {
                    UsuarioSesion.Instancia.Nombre = newName;
                    UsuarioSesion.Instancia.Email = newEmail;
                    UsuarioSesion.Instancia.Password = newPassword;
                    UsuarioSesion.Instancia.RolId = rolId;
                    UsuarioSesion.Instancia.Rol = (rolId == 2) ? "Administrador" : "Jugador";
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
        private IEnumerator CargarDatosDesdeAPI(int id)
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
                    adminRoleContainer.SetActive(esAdmin || UsuarioSesion.Instancia.Rol == "Administrador");

                    int index = roleDropdown.options.FindIndex(opt => opt.text == datos.Rol?.Nombre);
                    if (index != -1) roleDropdown.value = index;
                }

                if (confirmDeleteButton != null)
                    confirmDeleteButton.SetActive(false);
            }
            else
            {
                errorText.text = "Error al cargar el usuario.";
                Debug.LogWarning($" Error: {request.downloadHandler.text}");
            }
        }

        public void CargarPerfilActual()
        {
            if (UsuarioSesion.Instancia == null)
            {
                Debug.LogError("UsuarioSesion no está inicializado.");
                errorText.text = "Error interno: sesión no iniciada.";
                return;
            }

            UsuarioEditadoId = UsuarioSesion.Instancia.Id;

            if (UsuarioEditadoId <= 0)
            {
                Debug.LogError("❌ ID del usuario inválido.");
                errorText.text = "Error interno: ID no válido.";
                return;
            }

            inputName.text = UsuarioSesion.Instancia.Nombre;
            inputEmail.text = UsuarioSesion.Instancia.Email;
            inputPassword.text = "";

            if (adminRoleContainer != null)
            {
                bool esAdmin = UsuarioSesion.Instancia.Rol == "Administrador";
                adminRoleContainer.SetActive(esAdmin);

                int index = roleDropdown.options.FindIndex(opt => opt.text == UsuarioSesion.Instancia.Rol);
                if (index != -1) roleDropdown.value = index;
            }

            if (confirmDeleteButton != null)
                confirmDeleteButton.SetActive(false);

            if (profilePanel != null)
                profilePanel.SetActive(true);
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
            public int Id;
            public string NombreUsuario;
            public string Email;
            public string ContrasenaHash;
            public int RolId;
        }
    }
}
