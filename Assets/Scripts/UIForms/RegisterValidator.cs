using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Valida el formulario de registro de usuario.
    /// Extiende la validación con email, repetir contraseña, rol, etc.
    /// </summary>
    public class RegisterValidator : FormValidatorBase
    {
        [Header("Código de admin")]

        [Tooltip("Código secreto que debe introducirse si el usuario selecciona el rol de administrador")]
        [SerializeField] private string adminCode;

        [Header("Campos adicionales")]

        [Tooltip("Campo de texto para repetir la contraseña introducida")]
        [SerializeField] private TMP_InputField inputRepeatPassword;

        [Tooltip("Campo de texto para introducir el correo electrónico")]
        [SerializeField] private TMP_InputField inputEmail;

        [Tooltip("Toggle que representa la selección del rol 'Jugador'")]
        [SerializeField] private Toggle togglePlayer;

        [Tooltip("Toggle que representa la selección del rol 'Administrador'")]
        [SerializeField] private Toggle toggleAdmin;

        [Tooltip("Campo de texto para introducir el código de administrador si se selecciona ese rol")]
        [SerializeField] private TMP_InputField inputCodeAdmin;

        /// <summary>
        /// Valida todos los campos del formulario de registro y muestra errores si los hay.
        /// </summary>
        public override void Validar()
        {
            string user = inputUser.text.Trim();
            string email = inputEmail.text.Trim();
            string password = inputPassword.text;
            string repeatPassword = inputRepeatPassword.text;
            string codeAdmin = inputCodeAdmin.text.Trim();
            bool isAdmin = toggleAdmin != null && toggleAdmin.isOn;

            if (string.IsNullOrEmpty(user))
            {
                ShowError("El nombre de usuario es obligatorio.");
                return;
            }

            if (string.IsNullOrEmpty(email) || !EsEmailValido(email))
            {
                ShowError("Introduce un correo electrónico válido.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("La contraseña no puede estar vacía.");
                return;
            }

            if (password != repeatPassword)
            {
                ShowError("Las contraseñas no coinciden.");
                return;
            }

            if ((togglePlayer != null && !togglePlayer.isOn) && (toggleAdmin != null && !toggleAdmin.isOn))
            {
                ShowError("Selecciona un rol para continuar.");
                return;
            }

            if (isAdmin && (string.IsNullOrEmpty(codeAdmin) || codeAdmin != adminCode))
            {
                ShowError("Introduce bien el código de administrador.");
                return;
            }

            if (user.Length > MAX_LENGTH || email.Length > MAX_LENGTH || password.Length > MAX_LENGTH)
            {
                ShowError("Alguno de los campos supera la longitud máxima permitida.");
                return;
            }

            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                ShowError("La contraseña debe tener al menos 6 caracteres.");
                return;
            }

            if (user.Contains(" ") || email.Contains(" ") || password.Contains(" "))
            {
                ShowError("Los campos no deben contener espacios.");
                return;
            }

            LimpiarError();
            Debug.Log("Registro válido. Puedes proceder a enviar la solicitud.");
            // Aquí iría el envío real (a backend o simulación)
        }

        /// <summary>
        /// Valida si el email tiene un formato básico correcto.
        /// </summary>
        private bool EsEmailValido(string email)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }
    }
}
