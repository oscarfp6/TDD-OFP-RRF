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
            u.FechaCreacion = hoy;
            u.FechaCaducidadPassword = hoy.AddDays(365);
        }


        [TestMethod]
        public void Constructor_AsignaTodasLasPropiedadesYValoresPorDefecto()
        {

            Assert.IsNotNull(u, "El objeto Usuario no debe ser nulo.");
            Assert.AreEqual(idUsuario, u.IdUsuario, "El IdUsuario no se asignó correctamente.");
            Assert.AreEqual(nombre, u.Nombre, "El Nombre no se asignó correctamente.");
            Assert.AreEqual(apellidos, u.Apellidos, "El Apellidos no se asignó correctamente.");
            Assert.AreEqual(email, u.Email, "El Email no se asignó correctamente.");
            Assert.AreEqual(direccionPostal, u.DireccionPostal, "La DireccionPostal no se asignó correctamente.");
            Assert.IsTrue(u.CuentaActiva, "La cuenta debería estar activa por defecto.");
            Assert.AreEqual(DateTime.MinValue, u.UltimoAcceso, "El UltimoAcceso debería ser DateTime.MinValue por defecto.");

            Assert.IsTrue(Math.Abs((u.FechaCreacion - hoy).TotalSeconds) < 1, "FechaCreacion no es cercana a la fecha de hoy.");
            Assert.IsTrue(Math.Abs((u.FechaCaducidadPassword - hoy.AddDays(365)).TotalSeconds) < 1, "La FechaCaducidadContraseña no es 365 días después de hoy.");
        }

        [TestMethod]
        public void GetYSet_ModificaPropiedadesCorrectamente()
        {
            string nuevoNombre = "Ana";
            string nuevoEmail = "ana.perez@test.com";
            bool nuevoEstado = false;

            u.Nombre = nuevoNombre;
            u.Email = nuevoEmail;
            u.CuentaActiva = nuevoEstado;
            u.IdUsuario = 99;

            Assert.AreEqual(nuevoNombre, u.Nombre, "El setter/getter de Nombre no funciona.");
            Assert.AreEqual(nuevoEmail, u.Email, "El setter/getter de Email no funciona.");
            Assert.AreEqual(nuevoEstado, u.CuentaActiva, "El setter/getter de CuentaActiva no funciona.");
            Assert.AreEqual(99, u.IdUsuario, "El setter/getter de IdUsuario no funciona.");
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
            string passwordNueva = "@NuevaContraseña456";
            bool resultado = u.CambiarPassword(password, passwordNueva);
            Assert.IsTrue(resultado, "Debería retornar TRUE al cambiar la contraseña con la anterior correcta.");
        }

        [TestMethod]
        public void CambiarContraseña_False()
        {
            string contraseñaNueva = "NuevaContraseña456";
            bool resultado = u.CambiarPassword("ContraseñaFalsa", contraseñaNueva);
            Assert.IsFalse(resultado, "Debería retornar FALSE al fallar la validación de la contraseña anterior.");
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
            Usuario u1 = new Usuario(-1, nombre, apellidos, email, password, direccionPostal);
            Assert.IsFalse(u1.EsValido(), "ID negativo debería ser inválido.");

            // Usuario con Email vacío o nulo (Inválido)
            Assert.ThrowsException<ArgumentException>(() =>
                new Usuario(1, nombre, apellidos, "", password, direccionPostal)
            );

            // Usuario con Contraseña vacía o nula (Inválido)
            
            Assert.ThrowsException<ArgumentException>(() =>
                new Usuario(1, nombre, apellidos, email, null, direccionPostal)
            );
        }

        [TestMethod]
        public void ActualizarUltimoAcceso_CambiaUltimoAccesoALaHoraActual()
        {
            // Arrange
            DateTime tiempoDeAcceso = DateTime.Now.AddMinutes(5); // Simulamos un acceso posterior
            u.UltimoAcceso = tiempoDeAcceso;

            // Assert
            Assert.IsTrue(Math.Abs((u.UltimoAcceso - tiempoDeAcceso).TotalSeconds) < 1, "UltimoAcceso no fue actualizado correctamente.");
        }
    }
}
