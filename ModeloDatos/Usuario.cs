namespace ModeloDatos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Apellidos { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }

        public Usuario(int idUsuario, string nombre, string apellidos, string email, string contraseña)
        {
            this.IdUsuario = idUsuario;
            this.Nombre = nombre;
            this.Apellidos = apellidos;
            this.Email= email;
            this.Contraseña= contraseña;
        }
    }
}