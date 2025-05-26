namespace Extra
{
    /// <summary>
    /// Clase puente para transferir el ID del usuario que se va a editar.
    /// Se usa entre selección de sesión y el panel de perfil.
    /// </summary>
    public static class SesionBridge
    {
        /// <summary>
        /// Si se establece, indica que el perfil a editar no es el actual.
        /// </summary>
        public static int? UsuarioIdParaEdicion { get; set; }

        /// <summary>
        /// Limpia el valor después de usarlo.
        /// </summary>
        public static void Limpiar()
        {
            UsuarioIdParaEdicion = null;
        }
    }
}