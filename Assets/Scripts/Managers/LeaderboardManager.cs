using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIForms;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    /// <summary>
    /// Manager principal que obtiene y muestra el leaderboard desde la API.
    /// FUNCIONALIDADES:
    /// 1. Consulta el backend en /api/leaderboard.
    /// 2. Instancia tarjetas visuales con ranking, nombre y puntos.
    /// 3. Ordena de mayor a menor puntuación.
    /// </summary>
    public class LeaderboardManager : MonoBehaviour
    {
        [Header("Configuración visual")]
        [Tooltip("Transform donde se instanciarán las tarjetas")]
        [SerializeField] private Transform contenedorLeaderboard;

        [Tooltip("Prefab de la tarjeta del leaderboard")]
        [SerializeField] private LeaderboardItem tarjetaPrefab;

        private void Start()
        {
            StartCoroutine(CargarLeaderboard());
        }

        private IEnumerator CargarLeaderboard()
        {
            using UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/api/leaderboard");
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"datos\":" + request.downloadHandler.text + "}";
                LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(json);

                // Limpiamos contenido anterior si existe
                foreach (Transform hijo in contenedorLeaderboard)
                    Destroy(hijo.gameObject);

                // Recorremos y creamos tarjetas
                for (int i = 0; i < wrapper.datos.Length; i++)
                {
                    var entrada = wrapper.datos[i];

                    LeaderboardItem item = Instantiate(tarjetaPrefab, contenedorLeaderboard);
                    item.Configurar(i + 1, entrada.nombreUsuario, entrada.puntuacion);                }
            }
            else
            {
                Debug.LogWarning(" Error al obtener el leaderboard: " + request.error);
            }
        }

        [System.Serializable]
        private class LeaderboardWrapper
        {
            public EntradaLeaderboard[] datos;
        }

        [System.Serializable]
        private class EntradaLeaderboard
        {
            public int id;
            public string nombreUsuario;
            public int puntuacion;
        }
    }
}
