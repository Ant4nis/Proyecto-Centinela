using System;
using System.Collections;
using TMPro;
using UIForms;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// Controlador que consulta la API para obtener todas las sesiones activas
    /// y genera una tarjeta visual para cada una en el contenedor.
    /// 
    /// FUNCIONALIDADES:
    /// 1. Llama a /api/sesion al iniciar el panel.
    /// 2. Instancia tarjetas con nombre, última conexión e IP.
    /// 3. Colorea el icono como verde si la sesión es del usuario actual.
    /// </summary>
    public class SesionListManager : MonoBehaviour
    {
        [Header("Configuración del contenedor")]
        [Tooltip("Contenedor donde se instanciarán las tarjetas de sesión.")]
        [SerializeField] private Transform contentTransform;

        [Header("Prefab de la tarjeta")]
        [Tooltip("Prefab que representa una fila de sesión.")]
        [SerializeField] private SesionItem tarjetaPrefab;

        [Header("Colores de estado")]
        [Tooltip("Color del icono si la sesión corresponde al usuario actual.")]
        [SerializeField] private Color colorConectado = Color.green;

        [Tooltip("Color del icono si es otro usuario.")]
        [SerializeField] private Color colorDesconectado = Color.red;

        /// <summary>
        /// Llama al backend y construye las tarjetas visuales.
        /// </summary>
        private IEnumerator ObtenerSesionesDesdeAPI()
        {
            using UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/api/sesion");
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = "{\"sesiones\":" + request.downloadHandler.text + "}";
                SesionRespuestaWrapper data = JsonUtility.FromJson<SesionRespuestaWrapper>(json);

                foreach (SesionDTO sesion in data.sesiones)
                {
                    SesionItem nuevaTarjeta = Instantiate(tarjetaPrefab, contentTransform);

                    nuevaTarjeta.NombreTMP.text =
                        sesion.usuarioId == UsuarioSesion.Instancia.Id
                            ? UsuarioSesion.Instancia.Nombre
                            : sesion.usuario;

                    if (DateTime.TryParse(sesion.ultimaConexion, out var fecha))
                    {
                        nuevaTarjeta.FechaTMP.text = fecha.ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        nuevaTarjeta.FechaTMP.text = "-";
                    }

                    nuevaTarjeta.IpTMP.text = sesion.ip;

                    bool esUsuarioActual = sesion.usuarioId == UsuarioSesion.Instancia.Id;
                    nuevaTarjeta.EstadoIcon.color = esUsuarioActual ? colorConectado : colorDesconectado;

                    // Inicializa la tarjeta con los datos del usuario
                    nuevaTarjeta.Inicializar(sesion.usuarioId, sesion.usuario);
                }
            }
            else
            {
                Debug.LogWarning("Error al obtener sesiones: " + request.error);
            }
        }

        /// <summary>
        /// Llama desde fuera para volver a cargar las sesiones.
        /// </summary>
        public void RefrescarSesiones()
        {
            // Elimina los hijos actuales
            foreach (Transform hijo in contentTransform)
            {
                Destroy(hijo.gameObject);
            }

            // Llama de nuevo a la API
            StartCoroutine(ObtenerSesionesDesdeAPI());
        }

        /// <summary>
        /// Estructura que representa una sesión individual.
        /// </summary>
        [System.Serializable]
        private class SesionDTO
        {
            public int id;
            public int usuarioId;
            public string usuario;
            public string ultimaConexion;
            public string ip;
        }

        /// <summary>
        /// Wrapper para deserializar el array de sesiones.
        /// </summary>
        [System.Serializable]
        private class SesionRespuestaWrapper
        {
            public SesionDTO[] sesiones;
        }
    }
}
