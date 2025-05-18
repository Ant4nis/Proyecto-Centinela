using TMPro;
using UnityEngine;

namespace UIForms
{
    /// <summary>
    /// Clase base abstracta para formularios de validación.
    /// Contiene validaciones comunes (usuario, contraseña) y lógica compartida.
    /// </summary>
    public abstract class FormValidatorBase : MonoBehaviour
    {
        [Header("Campos comunes")]
        [SerializeField] protected TMP_InputField inputUser;
        [SerializeField] protected TMP_InputField inputPassword;
        [SerializeField] protected TMP_Text errorText;

        protected const int MAX_LENGTH = 50;
        protected const int MIN_PASSWORD_LENGTH = 6;

        /// <summary>
        /// Método a implementar por las subclases para validar sus formularios.
        /// </summary>
        public abstract void Validar();

        /// <summary>
        /// Muestra un mensaje de error y detiene el proceso.
        /// </summary>
        protected bool ShowError(string mensaje)
        {
            errorText.text = mensaje;
            return false;
        }

        /// <summary>
        /// Limpia los errores en pantalla.
        /// </summary>
        protected void LimpiarError()
        {
            errorText.text = "";
        }
    }
}