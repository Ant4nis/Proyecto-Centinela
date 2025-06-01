using Extra;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Colores por calidad de arma")]
        [SerializeField] private Color commonWeaponColor;
        [SerializeField] private Color rareWeaponColor;
        [SerializeField] private Color epicWeaponColor;
        [SerializeField] private Color legendaryWeaponColor;

        private void Update()
        {
            // 🔄 Detecta si se pulsa Escape para volver al menú principal
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenu();
            }
        }

        /// <summary>
        /// Carga la escena del menú principal.
        /// </summary>
        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        /// <summary>
        /// Devuelve un color en función de la calidad del arma.
        /// </summary>
        public Color GetWeaponColor(WeaponQuality quality)
        {
            switch (quality)
            {
                case WeaponQuality.Common: return commonWeaponColor;
                case WeaponQuality.Rare: return rareWeaponColor;
                case WeaponQuality.Epic: return epicWeaponColor;
                case WeaponQuality.Legendary: return legendaryWeaponColor;
                default: return Color.white;
            }
        }
    }
}