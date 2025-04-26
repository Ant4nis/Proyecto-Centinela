using System.Collections;
using UnityEngine;

public enum Direction8
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

namespace Extra
{
    /// <summary>
    /// Métodos útiles.
    /// 1. IEFade: interpola el alpha de un CanvasGroup desde su valor actual al valor objetivo en un tiempo dado.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Interpola el alpha de un CanvasGroup hasta el valor indicado.
        /// </summary>
        /// <param name="canvasGroup">CanvasGroup al que aplicar el fade.</param>
        /// <param name="targetAlpha">Valor final de alpha (0 = transparente, 1 = opaco).</param>
        /// <param name="duration">Duración de la interpolación en segundos.</param>
        public static IEnumerator IEFade(CanvasGroup canvasGroup, float targetAlpha, float duration)
        {
            float timer = 0f;
            float initialAlpha = canvasGroup.alpha;

            // Mientras dure la interpolación
            while (timer < duration)
            {
                // Lerp desde initialAlpha hasta targetAlpha según el progreso (timer/duration)
                canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Al final, asegurar que queda exactamente en el valor objetivo
            canvasGroup.alpha = targetAlpha;
        }
        
    }
}