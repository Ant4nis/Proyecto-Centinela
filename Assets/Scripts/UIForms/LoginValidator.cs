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
        }
    }
}