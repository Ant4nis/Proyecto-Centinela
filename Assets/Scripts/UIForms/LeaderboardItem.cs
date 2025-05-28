using TMPro;
using UnityEngine;

namespace UIForms
{
    /// <summary>
    /// Controlador individual de una tarjeta del leaderboard.
    /// Expone los campos visuales para ser llenados desde el manager.
    /// </summary>
    public class LeaderboardItem : MonoBehaviour
    {
        [Header("Referencias de texto")]
        [Tooltip("Texto de ranking (posición)")]
        public TMP_Text rankingText;

        [Tooltip("Texto del nombre del jugador")]
        public TMP_Text nombreText;

        [Tooltip("Texto de los puntos obtenidos")]
        public TMP_Text puntosText;

        /// <summary>
        /// Establece los valores de la tarjeta.
        /// </summary>
        public void Configurar(int posicion, string nombre, int puntos)
        {
            rankingText.text = $"{posicion}.";
            nombreText.text = nombre;
            puntosText.text = $"<color=green>{puntos}</color> puntos";
        }
    }
}