using UIForms;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Controlador del menú principal. Permite:
    /// 1. Alternar entre paneles (por ejemplo: mostrar el de Gestiones y ocultar el principal).
    /// 2. Ocultar automáticamente el panel de gestiones si el usuario no es administrador.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Navegación entre paneles")]
        [Tooltip("Este panel se desactivará cuando el destino se active.")]
        [SerializeField] private GameObject originPanel;

        [Tooltip("Este panel se activará cuando se oculte el anterior.")]
        [SerializeField] private GameObject targetPanel;

        [Tooltip("Este panel se activará cuando se pulse perfil.")]
        [SerializeField] private GameObject perfilPanel;
        
        [Tooltip("Este panel se activará cuando se pulse Leaderboard.")]
        [SerializeField] private GameObject leaderboardPanel;
        
        [Header("Panel exclusivo para administradores")]
        [Tooltip("Este panel se ocultará si el usuario no tiene rol de administrador.")]
        [SerializeField] private GameObject adminOnlyPanel;
        
        [Header("Controlador de sesiones")]
        [Tooltip("Script que gestiona la lista de sesiones")]
        [SerializeField] private SesionListManager sesionListManager;

        /// <summary>
        /// Inicializa el menú comprobando el rol del usuario y ajustando los paneles.
        /// </summary>
        private void Start()
        {
            if (UsuarioSesion.Instancia.Rol != "Administrador" && adminOnlyPanel != null)
            {
                adminOnlyPanel.SetActive(false);
                Debug.Log("Panel de gestiones oculto: el usuario no es administrador.");
            }
            
            // Desvanecer desde negro al cargar el menú
            if (FadeController.Instance != null)
                FadeController.Instance.FadeIn();

            // Lógica de rol (ocultar panel admin si no corresponde)
            if (UsuarioSesion.Instancia.Rol != "Administrador" && adminOnlyPanel != null)
            {
                adminOnlyPanel.SetActive(false);
                Debug.Log("Panel de gestiones oculto: el usuario no es administrador.");
            }
        }

        /// <summary>
        /// Oculta el panel de origen y muestra el panel destino.
        /// </summary>
        public void ShowTargetAndHideOrigin()
        {
            if (originPanel != null) originPanel.SetActive(false);
            if (targetPanel != null) targetPanel.SetActive(true);

            if (sesionListManager != null)
            {
                sesionListManager.RefrescarSesiones();
            }
        }

        public void HideProfile()
        {
            if (perfilPanel != null) perfilPanel.SetActive(false);
            if (originPanel != null) originPanel.SetActive(true);
        }
        
        /// <summary>
        /// Muestra el panel original y oculta el destino (volver atrás).
        /// </summary>
        public void RevertPanels()
        {
            if (targetPanel != null) targetPanel.SetActive(false);
            if (leaderboardPanel != null) targetPanel.SetActive(false);
            if (originPanel != null) originPanel.SetActive(true);
        }

        public void EditProfile()
        {
            if (originPanel != null) originPanel.SetActive(false);
            if (perfilPanel != null) perfilPanel.SetActive(true);
            
        }

        public void ShowLeaderboard()
        {
            if (originPanel != null) originPanel.SetActive(false);
            if (leaderboardPanel != null) leaderboardPanel.SetActive(true);
        }

        public void HideLeaderboard()
        {
            if (leaderboardPanel != null) leaderboardPanel.SetActive(false);
            if (originPanel != null) originPanel.SetActive(true);
        }
        
    }
}