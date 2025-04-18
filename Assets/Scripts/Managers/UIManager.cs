using System;
using Extra;
using Player;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// Gestor de la interfaz de usuario (UI) del jugador que:
    /// 1. Implementa el patrón Singleton para acceso global.
    /// 2. Actualiza las barras de estado (salud, armadura, munición) con interpolación suave (Lerp).
    /// 3. Actualiza los textos de la UI con los valores actuales y máximos.
    /// 4. Gestiona el efecto de fade de transición entre mazmorras.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Instancia global del UIManager accesible desde cualquier parte del juego.
        /// </summary>
        public static UIManager Instance;
        
        [Header("PARA TESTING")]
        [Tooltip("Configuración temporal del jugador para pruebas.")]
        [SerializeField] private PlayerConfiguration playerConfig;
        
        [Header("Imágenes UI")]
        [Tooltip("Barra que muestra el porcentaje de salud actual.")]
        [SerializeField] private Image playerHealthBar;
        [Tooltip("Barra que muestra el porcentaje de armadura actual.")]
        [SerializeField] private Image playerArmorBar;
        [Tooltip("Barra que muestra el porcentaje de munición actual.")]
        [SerializeField] private Image playerAmmoBar;
        
        [Header("Textos UI")]
        [Tooltip("Texto que muestra la salud actual y máxima.")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [Tooltip("Texto que muestra la armadura actual y máxima.")]
        [SerializeField] private TextMeshProUGUI playerArmorText;
        [Tooltip("Texto que muestra la munición actual y máxima.")]
        [SerializeField] private TextMeshProUGUI playerAmmoText;
        
        [Header("UI Extra")]
        [Tooltip("CanvasGroup utilizado para el efecto de fade en transiciones.")]
        [SerializeField] private CanvasGroup fadePanel;

        /// <summary>
        /// Inicializa la instancia Singleton al cargar el componente.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Llamado cada frame para actualizar dinámicamente la UI.
        /// </summary>
        private void Update()
        {
            UpdateUI();
        }

        /// <summary>
        /// Actualiza las barras y textos de la UI.
        /// 
        /// Funcionalidades:
        /// 1. Interpola (Lerp) el valor de las barras de salud, armadura y munición.
        /// 2. Actualiza los textos con el formato "actual/máximo".
        /// </summary>
        private void UpdateUI()
        {
            // Transición suave de las barras
            playerHealthBar.fillAmount = Mathf.Lerp(
                playerHealthBar.fillAmount,
                playerConfig.CurrentHealth / playerConfig.MaxHealth,
                10f * Time.deltaTime
            );
            playerArmorBar.fillAmount = Mathf.Lerp(
                playerArmorBar.fillAmount,
                playerConfig.CurrentArmor / playerConfig.MaxArmor,
                10f * Time.deltaTime
            );
            playerAmmoBar.fillAmount = Mathf.Lerp(
                playerAmmoBar.fillAmount,
                playerConfig.CurrentAmmo / playerConfig.MaxAmmo,
                10f * Time.deltaTime
            );

            // Actualización de textos
            playerHealthText.text = $"{playerConfig.CurrentHealth}/{playerConfig.MaxHealth}";
            playerArmorText.text  = $"{playerConfig.CurrentArmor}/{playerConfig.MaxArmor}";
            playerAmmoText.text   = $"{playerConfig.CurrentAmmo}/{playerConfig.MaxAmmo}";
        }

        /// <summary>
        /// Inicia un fade del panel de UI a un valor de alpha determinado.
        /// </summary>
        /// <param name="targetAlpha">Valor final de alpha (0 = transparente, 1 = opaco).</param>
        public void NewDungeonFade(float targetAlpha)
        {
            StartCoroutine(Helper.IEFade(fadePanel, targetAlpha, 1.5f));
        }
    }
}
