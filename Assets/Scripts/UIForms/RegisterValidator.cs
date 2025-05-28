using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Valida el formulario de registro de usuario y envía los datos a la API.
    /// </summary>
    public class RegisterValidator : FormValidatorBase
    {
        [Header("Código de admin")]
        [Tooltip("Código secreto que debe introducirse si el usuario selecciona el rol de administrador")]
        [SerializeField] private string adminCode;

        [Header("Campos adicionales")]
        [SerializeField] private TMP_InputField inputRepeatPassword;
        [SerializeField] private TMP_InputField inputEmail;
        [SerializeField] private Toggle togglePlayer;
        [SerializeField] private Toggle toggleAdmin;
        [SerializeField] private TMP_InputField inputCodeAdmin;

        public override void Validar()
        {
            string user = inputUser.text.Trim();
            string email = inputEmail.text.Trim();
            string password = inputPassword.text;
            string repeatPassword = inputRepeatPassword.text;
            string codeAdmin = inputCodeAdmin.text.Trim();
            bool isAdmin = toggleAdmin != null && toggleAdmin.isOn;

            if (string.IsNullOrEmpty(user)) { ShowError("El nombre de usuario es obligatorio."); return; }
            if (string.IsNullOrEmpty(email) || !EsEmailValido(email)) { ShowError("Introduce un correo electrónico válido."); return; }
            if (string.IsNullOrEmpty(password)) { ShowError("La contraseña no puede estar vacía."); return; }
            if (password != repeatPassword) { ShowError("Las contraseñas no coinciden."); return; }
            if (!togglePlayer.isOn && !toggleAdmin.isOn) { ShowError("Selecciona un rol para continuar."); return; }
            if (isAdmin && (string.IsNullOrEmpty(codeAdmin) || codeAdmin != adminCode)) { ShowError("Introduce bien el código de administrador."); return; }
            if (user.Length > MAX_LENGTH || email.Length > MAX_LENGTH || password.Length > MAX_LENGTH)
            { ShowError("Algún campo supera la longitud máxima permitida."); return; }
            if (password.Length < MIN_PASSWORD_LENGTH) { ShowError("La contraseña debe tener al menos 6 caracteres."); return; }
            if (user.Contains(" ") || email.Contains(" ") || password.Contains(" ")) { ShowError("Los campos no deben contener espacios."); return; }

            LimpiarError();
            Debug.Log("Registro válido. Procediendo al backend...");
            StartCoroutine(EnviarRegistro(user, email, password, isAdmin));
        }

        private IEnumerator EnviarRegistro(string user, string email, string password, bool isAdmin)
        {
            int rolId = isAdmin ? 2 : 1;

            var dto = new UsuarioRegisterDTO
            {
                NombreUsuario = user,
                Email = email,
                ContrasenaHash = password
            };

            string json = JsonUtility.ToJson(dto);
            string url = $"http://localhost:5000/api/auth/register?rolId={rolId}";

            using var request = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.responseCode == 200 || request.responseCode == 201)
            {
                Debug.Log("Usuario registrado correctamente.");
                SceneManager.LoadScene("LoginScene");
            }
            else
            {
                Debug.LogWarning($"Registro fallido ({request.responseCode}): {request.downloadHandler.text}");
                ShowError("No se pudo registrar el usuario. " + ExtraerMensaje(request.downloadHandler.text));
            }
        }

        private bool EsEmailValido(string email)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }

        private string ExtraerMensaje(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return "";

            try
            {
                var wrapper = JsonUtility.FromJson<ErrorRespuesta>(json);
                return wrapper.mensaje;
            }
            catch
            {
                return ""; // si no es un JSON esperable
            }
        }

        [System.Serializable]
        private class ErrorRespuesta
        {
            public string mensaje;
        }
        
        [System.Serializable]
        private class UsuarioRegisterDTO
        {
            public string NombreUsuario;
            public string Email;
            public string ContrasenaHash;
        }
    }
}
