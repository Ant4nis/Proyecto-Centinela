using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace UIForms
{
    /// <summary>
    /// Envía puntuación al backend para registrarla en el leaderboard.
    /// </summary>
    public static class LeaderboardSender
    {
        private const string ApiUrl = "http://localhost:5000/api/Leaderboard"; // Sustituye con tu puerto real

        public static IEnumerator EnviarPuntuacion(int usuarioId, int puntos, string nivel)
        {
            // Construir el JSON manualmente
            string json = JsonUtility.ToJson(new LeaderboardDTO(usuarioId, puntos, nivel));

            using UnityWebRequest request = new UnityWebRequest(ApiUrl, "POST");
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("✅ Puntuación enviada al leaderboard.");
            }
            else
            {
                Debug.LogError($"❌ Error al enviar puntuación: {request.responseCode} → {request.downloadHandler.text}");
            }
        }
    }

    [Serializable]
    public class LeaderboardDTO
    {
        public int usuarioId;
        public int puntuacion;
        public string nivel;
        public string fecha;

        public LeaderboardDTO(int usuarioId, int puntuacion, string nivel)
        {
            this.usuarioId = usuarioId;
            this.puntuacion = puntuacion;
            this.nivel = nivel;
            this.fecha = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}