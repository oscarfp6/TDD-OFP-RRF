using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System;

namespace ModeloDatosTest
{
    [TestClass]
    public class UsuarioTest
    {
        private readonly int idUsuario = 1;
        private readonly string nombre = "Oscar";
        private readonly string apellidos = "Fuentes Paniego";
        private readonly string email = "oscar@gmail.com";
        private readonly string password = "@Contraseñasegura123";
        private readonly string direccionPostal = "C/ Parralillos s/n";
        private Usuario u;
        private DateTime hoy;

        [TestInitialize()]
        public void TestInitialize()
        {
            hoy = DateTime.Now;
            u = new Usuario(idUsuario, nombre, apellidos, email, password, direccionPostal);
            // Código que se ejecuta antes de cada prueba
            u.fechaCreacion = hoy;
            u.fechaCaducidadPassword = hoy.AddDays(365);
        }


        [TestMethod]
        public void Constructor_AsignaTodasLasPropiedadesYValoresPorDefecto()
        {

            Assert.IsNotNull(u, "El objeto Usuario no debe ser nulo.");
            Assert.AreEqual(idUsuario, u.idUsuario, "El IdUsuario no se asignó correctamente.");
            Assert.AreEqual(nombre, u.nombre, "El Nombre no se asignó correctamente.");
            Assert.AreEqual(apellidos, u.apellidos, "El Apellidos no se asignó correctamente.");
            Assert.AreEqual(email, u.email, "El Email no se asignó correctamente.");
            Assert.AreEqual(password, u.password, "La Contraseña no se asignó correctamente.");
            Assert.AreEqual(direccionPostal, u.direccionPostal, "La DireccionPostal no se asignó correctamente.");
            Assert.IsTrue(u.cuentaActiva, "La cuenta debería estar activa por defecto.");
            Assert.AreEqual(DateTime.MinValue, u.ultimoAcceso, "El UltimoAcceso debería ser DateTime.MinValue por defecto.");

            Assert.IsTrue(Math.Abs((u.fechaCreacion - hoy).TotalSeconds) < 1, "FechaCreacion no es cercana a la fecha de hoy.");
            Assert.IsTrue(Math.Abs((u.fechaCaducidadPassword - hoy.AddDays(365)).TotalSeconds) < 1, "La FechaCaducidadContraseña no es 365 días después de hoy.");
        }

        [TestMethod]
        public void GetYSet_ModificaPropiedadesCorrectamente()
        {
            string nuevoNombre = "Ana";
            string nuevoEmail = "ana.perez@test.com";
            bool nuevoEstado = false;

            u.nombre = nuevoNombre;
            u.email = nuevoEmail;
            u.cuentaActiva = nuevoEstado;
            u.idUsuario = 99;

            Assert.AreEqual(nuevoNombre, u.nombre, "El setter/getter de Nombre no funciona.");
            Assert.AreEqual(nuevoEmail, u.email, "El setter/getter de Email no funciona.");
            Assert.AreEqual(nuevoEstado, u.cuentaActiva, "El setter/getter de CuentaActiva no funciona.");
            Assert.AreEqual(99, u.idUsuario, "El setter/getter de IdUsuario no funciona.");
        }

        [TestMethod]
        public void ComprobarContraseña_True()
        {
            Assert.IsTrue(u.ComprobarPassword(password), "Debería retornar TRUE si la contraseña es correcta.");
        }

        [TestMethod]
        public void ComprobarContraseña_False()
        {
            Assert.IsFalse(u.ComprobarPassword("ContraseñaFalsa"), "Debería retornar FALSE si la contraseña no coincide.");
        }

        [TestMethod]
        public void CambiarContraseña_True()
        {
            string passwordNueva = "NuevaContraseña456";
            bool resultado = u.CambiarPassword(password, passwordNueva);
            Assert.IsTrue(resultado, "Debería retornar TRUE al cambiar la contraseña con la anterior correcta.");
            Assert.AreEqual(passwordNueva, u.Contraseña, "La contraseña no fue actualizada.");
        }

        [TestMethod]
        public void CambiarContraseña_False()
        {
            string contraseñaNueva = "NuevaContraseña456";
            bool resultado = u.CambiarPassword("ContraseñaFalsa", contraseñaNueva);
            Assert.IsFalse(resultado, "Debería retornar FALSE al fallar la validación de la contraseña anterior.");
            Assert.AreEqual(password, u.Contraseña, "La contraseña NO debería haber sido cambiada.");
        }

        [TestMethod]
        public void EsValido_DatosCompletos_RetornaTrue()
        {
            // Comprueba el usuario instanciado en TestInitialize
            Assert.IsTrue(u.EsValido(), "El usuario con datos completos debería ser válido.");
        }

        [TestMethod]
        public void EsValido_DatosIncompletosOInvalidos_RetornaFalse()
        {
            // Usuario con ID negativo (Inválido)
            Usuario u1 = new Usuario(-1, nombre, apellidos, email, password, direccionPostal;
            Assert.IsFalse(u1.EsValido(), "ID negativo debería ser inválido.");

            // Usuario con Email vacío o nulo (Inválido)
            Usuario u2 = new Usuario(1, nombre, apellidos, "", password, direccionPostal);
            Assert.IsFalse(u2.EsValido(), "Email vacío debería ser inválido.");

            // Usuario con Contraseña vacía o nula (Inválido)
            Usuario u3 = new Usuario(1, nombre, apellidos, email, null, direccionPostal);
            Assert.IsFalse(u3.EsValido(), "Contraseña nula debería ser inválida.");
        }

        [TestMethod]
        public void ActualizarUltimoAcceso_CambiaUltimoAccesoALaHoraActual()
        {
            // Arrange
            DateTime tiempoDeAcceso = DateTime.Now.AddMinutes(5); // Simulamos un acceso posterior
            u.ultimoAcceso = tiempoDeAcceso;

            // Assert
            Assert.IsTrue(Math.Abs((u.ultimoAcceso - tiempoDeAcceso).TotalSeconds) < 1, "UltimoAcceso no fue actualizado correctamente.");
        }
    }
}
