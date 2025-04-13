using System;
using Player;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// Componente encargado de actualizar la interfaz de usuario (UI) del jugador.
    /// 
    /// Funcionalidades:
    /// 1. Actualiza las barras de estado del jugador (salud, armadura y munición) utilizando interpolación (Lerp) para transiciones suaves.
    /// 2. Actualiza los textos de la UI que muestran los valores numéricos actuales y máximos de salud, armadura y munición.
    /// 3. Utiliza la configuración del jugador obtenida de un ScriptableObject para reflejar cambios en tiempo real.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        // TEMPORAL: Configuración del jugador para propósitos de testing.
        [Header("PARA TESTING")]
        [SerializeField] private PlayerConfiguration playerConfig;
        
        [Header("Imágenes UI"), Tooltip("Barra de salud del jugador")]
        [SerializeField] private Image playerHealthBar;
        [Tooltip("Barra de armadura del jugador")]
        [SerializeField] private Image playerArmorBar;
        [Tooltip("Barra de munición del jugador")]
        [SerializeField] private Image playerAmmoBar;
        
        [Header("Textos UI"), Tooltip("Nivel de salud del jugador")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [Tooltip("Nivel de blindaje del jugador")]
        [SerializeField] private TextMeshProUGUI playerArmorText;
        [Tooltip("Nivel de munición del jugador")]
        [SerializeField] private TextMeshProUGUI playerAmmoText;
        
        private void Update()
        {
            UpdateUI();
        }

        /// <summary>
        /// Actualiza los elementos de la interfaz de usuario.
        /// 
        /// Funcionalidades:
        /// 1. Actualiza las barras de salud, armadura y munición mediante interpolación para una transición suave.
        /// 2. Actualiza los textos de la UI para mostrar los valores actuales y máximos de cada estadística.
        /// </summary>
        private void UpdateUI()
        {
            // Actualiza las barras de la UI utilizando Lerp para transiciones suaves.
            playerHealthBar.fillAmount = Mathf.Lerp(playerHealthBar.fillAmount,
                playerConfig.CurrentHealth / playerConfig.MaxHealth, 10f * Time.deltaTime);
            playerArmorBar.fillAmount = Mathf.Lerp(playerArmorBar.fillAmount,
                playerConfig.CurrentArmor / playerConfig.MaxArmor, 10f * Time.deltaTime);
            playerAmmoBar.fillAmount = Mathf.Lerp(playerAmmoBar.fillAmount,
                playerConfig.CurrentAmmo / playerConfig.MaxAmmo, 10f * Time.deltaTime);

            // Actualiza los textos de la UI para reflejar los valores actuales y máximos.
            playerHealthText.text = $"{playerConfig.CurrentHealth}/{playerConfig.MaxHealth}";
            playerArmorText.text = $"{playerConfig.CurrentArmor}/{playerConfig.MaxArmor}";
            playerAmmoText.text = $"{playerConfig.CurrentAmmo}/{playerConfig.MaxAmmo}";
        }
    }
}
