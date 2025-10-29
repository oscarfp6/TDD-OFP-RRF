namespace ModeloDatos
{
    public class Usuario
    {
        public int idUsuario;
        public string nombre;
        public string apellidos;
        public string email;
        public string contraseña;
        public string direccionPostal;

        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Contraseña
        {
            get { return contraseña; }
            set { contraseña = value; }
        }

        public string DireccionPostal
        {
            get { return direccionPostal; }
            set { direccionPostal = value; }
        }



        public Usuario(int idUsuario, string nombre, string apellidos, string email, string contraseña, string direccionPostal)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.email = email;
            this.contraseña = contraseña;
            this.direccionPostal = direccionPostal;
        }
    }
}