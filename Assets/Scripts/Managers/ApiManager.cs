using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    /// <summary>
    /// Clase responsable de gestionar las peticiones HTTP a la API externa del leaderboard.
    /// 
    /// Funciones principales:
    /// 1. Enviar puntuación de un usuario al leaderboard.
    /// </summary>
    public class ApiManager : MonoBehaviour
    {
        [Header("API Config")]
        [Tooltip("URL base del servidor de la API (sin slash final).")]
        [SerializeField] private string baseApiUrl = "http://localhost:5000/api";

        /// <summary>
        /// Envía una nueva puntuación al leaderboard.
        /// </summary>
        /// <param name="dto">Datos a enviar.</param>
        public void SendLeaderboardEntry(LeaderboardCreateDTO dto)
        {
            StartCoroutine(PostLeaderboardEntry(dto));
        }

        /// <summary>
        /// Corrutina que realiza la petición POST a la API del leaderboard.
        /// </summary>
        private IEnumerator PostLeaderboardEntry(LeaderboardCreateDTO dto)
        {
            string url = $"{baseApiUrl}/api/Leaderboard";
            string jsonData = JsonUtility.ToJson(dto);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"✅ Leaderboard actualizado correctamente: {request.downloadHandler.text}");
            }
            else
            {
                Debug.LogError($"❌ Error al enviar puntuación al leaderboard: {request.error}");
            }
        }
    }
}