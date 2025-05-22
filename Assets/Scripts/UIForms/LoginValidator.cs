using System.Collections;
using UnityEngine;

namespace UIForms
{
    /// <summary>
    /// Valida el formulario de inicio de sesión.
    /// Usa la lógica base y agrega reglas específicas.
    /// </summary>
    public class LoginValidator : FormValidatorBase
    {
        public override void Validar()
        {
            string user = inputUser.text.Trim();
            string password = inputPassword.text;

            if (string.IsNullOrEmpty(user))
            {
                ShowError("El nombre de usuario es obligatorio.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("La contraseña es obligatoria.");
                return;
            }

            if (user.Length > MAX_LENGTH || password.Length > MAX_LENGTH)
            {
                ShowError("Has superado el tamaño máximo permitido.");
                return;
            }

            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                ShowError("La contraseña debe tener al menos 6 caracteres.");
                return;
            }

            if (user.Contains(" ") || password.Contains(" "))
            {
                ShowError("Los campos no deben contener espacios.");
                return;
            }

            LimpiarError();
            Debug.Log("Login válido. Puedes proceder a enviar la solicitud.");
            // TODO: enviar a backend
            StartCoroutine(EnviarLogin(inputUser.text.Trim(), inputPassword.text));
        }
        
        private IEnumerator EnviarLogin(string email, string contrasena)
        {
            var datos = new CredencialesLogin
            {
                email = email,
                contrasena = contrasena
            };

            string json = JsonUtility.ToJson(datos);

            using var request = new UnityEngine.Networking.UnityWebRequest("http://localhost:5000/api/auth/login", "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                var respuesta = JsonUtility.FromJson<RespuestaLogin>(request.downloadHandler.text);

                // Guardar sesión activa
                UsuarioSesion.Instancia.Id = respuesta.id;
                UsuarioSesion.Instancia.Nombre = respuesta.nombre;
                UsuarioSesion.Instancia.Rol = respuesta.rol;

                Debug.Log("✅ Login exitoso. Rol: " + respuesta.rol);

                // Aquí puedes cargar otra escena o mostrar paneles según rol
                // Ejemplo:
                // SceneManager.LoadScene("MenuPrincipal");
            }
            else
            {
                ShowError("❌ Error de login o conexión.");
                Debug.LogWarning(request.downloadHandler.text);
            }
        }

        [System.Serializable]
        private class CredencialesLogin
        {
            public string email;
            public string contrasena;
        }

        [System.Serializable]
        private class RespuestaLogin
        {
            public int id;
            public string nombre;
            public string rol;
        }

    }
}