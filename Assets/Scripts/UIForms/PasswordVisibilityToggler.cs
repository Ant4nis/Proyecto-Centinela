using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Permite alternar la visibilidad de un campo de contraseña con un botón tipo "ojito".
    /// El componente es completamente desacoplado y reutilizable.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class PasswordVisibilityToggler : MonoBehaviour
    {
        [Header("Campo de entrada asociado")]
        [Tooltip("Se asigna el campo de entrada (el input de contraseña) ")]
        [SerializeField] private TMP_InputField inputField;

        [Header("Iconos")]
        [Tooltip("Icono de contraseña visible en la ruta: Sprites/UI/Icono.png")]
        [SerializeField] private Sprite iconVisible;
        [Tooltip("Icono de contraseña oculta en la ruta: Sprites/UI/Icono.png")]
        [SerializeField] private Sprite iconHidden;

        private Image _buttonImage;
        private bool _isVisible;

        private void Awake()
        {
            _buttonImage = GetComponent<Image>();           // El componente Image del mismo GameObject.
            if (inputField == null)
            {
                Debug.LogError("[PasswordVisibilityToggle] No se asignó el TMP_InputField.");
            }

            GetComponent<Button>().onClick.AddListener(ToggleVisibility);       // Se asigna el listener para el clic.
            UpdateState();
        }

        /// <summary>
        /// Alterna entre mostrar u ocultar el contenido del campo.
        /// </summary>
        private void ToggleVisibility()
        {
            _isVisible = !_isVisible;       // Alternar
            UpdateState();
        }

        /// <summary>
        /// Aplica los cambios visuales y funcionales.
        /// </summary>
        private void UpdateState()
        {
            if (inputField == null) return;

            // Se cambia el tipo de contenido del input:
            // Si _isVisible es true, muestra el texto de forma normal; si no, lo oculta (modo Password).
            inputField.contentType = _isVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            inputField.ForceLabelUpdate();

            // Actualiza el ícono del botón según el estado actual:
            if (_buttonImage != null)
                _buttonImage.sprite = _isVisible ? iconVisible : iconHidden;
        }
    }
}