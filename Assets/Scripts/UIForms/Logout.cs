using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIForms
{
    public class Logout : MonoBehaviour
    {
        public void Disconnect()
        {
            UsuarioSesion.Instancia.Reset();
            SceneManager.LoadScene("LoginScene");
        }
    }
}