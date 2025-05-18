using UnityEngine;
using UnityEngine.UI;

namespace UIForms
{
    /// <summary>
    /// Controla la visibilidad del campo de código de administrador y ajusta la alineación
    /// del grupo de botones de rol dependiendo de la opción seleccionada.
    /// </summary>
    public class AdminCodeToggler : MonoBehaviour
    {
        [Header("Botón Admin")]
        [SerializeField] private Toggle adminToggle;

        [Header("Contenedores de UI")]
        [Tooltip("Contenedor - CódigoAdmin")]
        [SerializeField] private GameObject adminCodeContainer;
        [Tooltip("Contenedor - Roles")]
        [SerializeField] private HorizontalLayoutGroup rolesLayoutGroup;

        /// <summary>
        /// Se ejecuta cuando cambia el rol seleccionado.
        /// Muestra u oculta el campo de código de administrador y ajusta el centrado del layout.
        /// </summary>
        public void OnRoleChanged()
        {
            bool isAdmin = adminToggle.isOn;

            // Mostrar u ocultar el input de código
            adminCodeContainer.SetActive(isAdmin);

            // Cambiar la alineación del grupo de roles según el tipo
            if (isAdmin)
            {
                rolesLayoutGroup.childAlignment = TextAnchor.UpperLeft;
            }
            else
            {
                rolesLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            }

            // Forzar que el layout se actualice inmediatamente
            LayoutRebuilder.ForceRebuildLayoutImmediate(rolesLayoutGroup.GetComponent<RectTransform>());
        }
    }
}