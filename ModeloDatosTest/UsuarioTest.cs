using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System;

namespace ModeloDatosTest
{
    [TestClass]


    public class UsuarioTest
    {
        int idUsuario = 1;
        string nombre = "Oscar";
        string apellidos = "Fuentes Paniego";
        string email = "oscar@gmail.com";
        string contraseña = "@Contraseñasegura123";
        string direccionPostal = "C/ Parralillos s/n";
        DateTime hoy = DateTime.Now;
        Usuario u = null;

        [TestInitialize()]
        public void TestInitialize()
        {
            u = new Usuario(idUsuario, nombre, apellidos, email, contraseña, direccionPostal);
            // Código que se ejecuta antes de cada prueba
        }


        [TestMethod]
        public void Constructor()
        {
            int idUsuario = 1;
            string nombre = "Oscar";
            string apellidos = "Fuentes Paniego";
            string email = "oscar@gmail.com";
            string contraseña = "@Contraseñasegura123";
            string direccionPostal = "C/ Parralillos s/n";
            bool cuentaActiva = true;
            DateTime hoy = DateTime.Now;

            Usuario u = new Usuario(idUsuario, nombre, apellidos, email, contraseña, direccionPostal); 

            Assert.IsNotNull(u);
            Assert.AreEqual(idUsuario, u.idUsuario, "El IdUsuario no se asignó correctamente.");
            Assert.AreEqual(nombre, u.nombre, "El Nombre no se asignó correctamente.");
            Assert.AreEqual(apellidos, u.apellidos, "El Apellidos no se asignó correctamente.");
            Assert.AreEqual(email, u.email, "El Email no se asignó correctamente.");
            Assert.AreEqual(contraseña, u.c, "La Contraseña no se asignó correctamente.");
            Assert.IsTrue(u., "La cuenta debería estar activa por defecto.");
            Assert.IsTrue(u.ComprobarContraseña(contraseña), "La contraseña no coincide.");
            Assert.AreEqual(DateTime.MinValue, u.UltimoAcceso, "El UltimoAcceso debería ser DateTime.MinValue por defecto.");

            //Comprobamos las fechas
            Assert.AreEqual(hoy.AddDays(365), u.);
            Assert.AreEqual(hoy.AddDays(365), u.FechaCaducidadContraseña);

        }

        public void GetYSetTest()
        {

        }

        [TestMethod]
        public void ComprobarContraseña_True()
        {
            // Arrange
            string contraseñaInicial = "@Contraseñasegura123";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Act & Assert
            // Este test está en ROJO
            Assert.IsTrue(u.ComprobarContraseña(contraseñaInicial), "Debería retornar TRUE si la contraseña es correcta.");
        }

        [TestMethod]
        public void ComprobarContraseña_False()
        {
            string contraseñaInicial = "@Contraseñasegura123";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Act & Assert
            // Comprobamos que con una contraseña distinta devuelve FALSE
            Assert.IsFalse(u.ComprobarContraseña("ContraseñaFalsa"), "Debería retornar FALSE si la contraseña no coincide.");
        }

        [TestMethod]
        public void CambiarContraseña_True()
        {
            // Arrange
            string contraseñaInicial = "@Contraseñasegura123";
            string contraseñaNueva = "NuevaContraseña456";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Este test está en ROJO
            bool resultado = u.cambiarContraseña(contraseñaInicial, contraseñaNueva);

            // Assert
            Assert.IsTrue(resultado, "Debería retornar TRUE al cambiar la contraseña con la anterior correcta.");
            Assert.AreEqual(contraseñaNueva, u.Contraseña, "La contraseña no fue actualizada.");
        }

        [TestMethod]
        public void CambiarContraseña_False()
        {
            string contraseñaInicial = "@Contraseñasegura123";
            string contraseñaNueva = "NuevaContraseña456";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Act
            // Intentamos cambiar la contraseña usando una anterior incorrecta
            bool resultado = u.cambiarContraseña("ContraseñaFalsa", contraseñaNueva);

            // Assert
            Assert.IsFalse(resultado, "Debería retornar FALSE al fallar la validación de la contraseña anterior.");
            // Verificamos que la contraseña original se mantiene
            Assert.AreEqual(contraseñaInicial, u.Contraseña, "La contraseña NO debería haber sido cambiada.");
        }

        [TestMethod]
        public void Setter_Correctos()
        {
            // Arrange
            Usuario u = new Usuario(1, "NombreInicial", "ApellidoInicial", "e@mail.com", "C");
            string nuevoNombre = "NuevoNombre";
            string nuevoEmail = "nuevo.email@dominio.com";

            // Act
            u.Nombre = nuevoNombre;
            u.Email = nuevoEmail;

            // Assert
            Assert.AreEqual(nuevoNombre, u.Nombre, "El setter/getter de Nombre no funciona correctamente.");
            Assert.AreEqual(nuevoEmail, u.Email, "El setter/getter de Email no funciona correctamente.");
        }


    }
}
