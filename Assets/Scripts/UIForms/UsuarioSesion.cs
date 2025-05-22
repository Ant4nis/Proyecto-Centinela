namespace UIForms
{
    /// <summary>
    /// Almacena la sesión activa del usuario.
    /// </summary>
    public class UsuarioSesion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }

        private static UsuarioSesion instancia;

        /// <summary>
        /// Acceso global a la sesión activa del usuario.
        /// </summary>
        public static UsuarioSesion Instancia => instancia ??= new UsuarioSesion();
    }
}