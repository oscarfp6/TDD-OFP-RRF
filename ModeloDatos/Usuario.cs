using System;
using System.Globalization;

namespace ModeloDatos
{
    public class Usuario
    {
        public int _idUsuario;
        public string _nombre;
        public string _apellidos;
        public string _email;
        public string _password;
        public string _direccionPostal;

        public string Nombre 
        { 
            get => _nombre;  
            set {
                if (!Valid.Nombre(value))
                {
                    throw new ArgumentException("El nombre no puede contener números u otros caracteres no válidos.");
                }
                _nombre = value;
            } 
        }

        public string Apellidos 
        { 
            get => _apellidos; 
            set {
                if (!Valid.Nombre(value))
                {
                    throw new ArgumentException("El apellido no puede contener números u otros caracteres no válidos.");
                }
                _apellidos = value;
            } 
        }
        public string Email 
        {
            get => _email;
            set
            {
                // VALIDACIÓN: Usa la utilidad de validación de Email.
                if (!Valid.ValidarEmail(value))
                {
                    throw new ArgumentException("El formato del email no es válido.");
                }
                _email = value;
            }
        }

        public int IdUsuario { get; set; }
        public string DireccionPostal { get { return _direccionPostal; } set { _direccionPostal = value; } }
        public bool CuentaActiva { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCaducidadPassword { get; set; }
        public DateTime UltimoAcceso { get; set; }

        public Usuario()
        {
            this.IdUsuario = 0;
            this.Nombre = string.Empty;
            this.Apellidos = string.Empty;
            this.Email = "example@gmail.com"; // Email válido por defecto
            this._password = string.Empty;
            this.DireccionPostal = string.Empty;
            this.CuentaActiva = false;
        }
        public Usuario(int idUsuario, string nombre, string apellidos, string email, string password, string direccionPostal)
        {
            if (!Valid.ValidarPassword(password))
            {
                throw new ArgumentException("La contraseña no cumple los requisitos de seguridad.");
            }

            this.IdUsuario = idUsuario;
            this.Nombre = nombre;
            this.Apellidos = apellidos;
            this.Email = email;
            this._password = Encriptar.EncriptarPasswordSHA256(password); ;
            this.DireccionPostal = direccionPostal;
            this.CuentaActiva = true;
            
        }

        public bool ComprobarPassword(string passwordAComprobar)
        {
            string passwordEncriptada = Encriptar.EncriptarPasswordSHA256(passwordAComprobar);
            return this._password == passwordEncriptada;
        }

        public bool CambiarPassword(string passwordActual, string nuevoPassword)
        {
            if (this.CuentaActiva == false)
            {
                return false;
            }

            // 2. VALIDACIÓN:
            //    - La contraseña actual debe ser correcta.
            //    - La nueva contraseña debe cumplir las reglas de seguridad.
            if (!ComprobarPassword(passwordActual) || !Valid.ValidarPassword(nuevoPassword))
            {
                return false;
            }

            // 3. Asignar el nuevo hash y restablecer la cuenta (por seguridad).
            this._password = Encriptar.EncriptarPasswordSHA256(nuevoPassword);
            this.RestablecerCuenta();
            return true;
        }

        private void RestablecerCuenta()
        {
            this.CuentaActiva = true;
            //this.intentosFallidosTimestamps.Clear(); // Limpia intentos fallidos
            this.UltimoAcceso = DateTime.Now;
            
        }

        public bool EsValido()
        {
            return this.IdUsuario > 0 &&
                   !string.IsNullOrEmpty(this.Email) &&
                   this._password != null;
        }

        public override string ToString()
        { 
            return $"ID: {IdUsuario}, Nombre: {Nombre}, Apellidos: {Apellidos}, Email: {Email}, Cuenta Activa: {CuentaActiva}, Dirección Postal: {DireccionPostal}, Último Login: {UltimoAcceso:dd/MM/yyyy HH:mm}";
        }
    }
}