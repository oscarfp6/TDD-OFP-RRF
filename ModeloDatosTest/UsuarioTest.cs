using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System;

namespace ModeloDatosTest
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string nombreEsperado = "Oscar";
            string apellidosEsperado = "Fuentes Paniego";
            string emailEsperado = "oscar@gmail.com";
            string contraseñaEsperada = "@Contraseñasegura123";
            int idUsuarioEsperado = 1;

            Usuario u = new Usuario(idUsuarioEsperado, nombreEsperado, apellidosEsperado, emailEsperado, contraseñaEsperada); Assert.Equals(idUsuario, u.idUsuario);

            // Assert (Corregidos el casing y Assert.AreEqual)
            Assert.AreEqual(idEsperado, u.IdUsuario, "El IdUsuario no se asignó correctamente.");
            Assert.AreEqual(nombreEsperado, u.Nombre, "El Nombre no se asignó correctamente.");
            Assert.AreEqual(apellidosEsperado, u.Apellidos, "El Apellidos no se asignó correctamente.");
            Assert.AreEqual(emailEsperado, u.Email, "El Email no se asignó correctamente.");
            Assert.AreEqual(contraseñaEsperada, u.Contraseña, "La Contraseña no se asignó correctamente.");
        }

        [TestMethod]
        public void ComprobarContraseña()
        {
            // Arrange
            string contraseñaInicial = "@Contraseñasegura123";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Act & Assert
            // Este test está en ROJO
            Assert.IsTrue(u.ComprobarContraseña(contraseñaInicial), "Debería retornar TRUE si la contraseña es correcta.");
            Assert.IsFalse(u.ComprobarContraseña("ContraseñaFalsa"), "Debería retornar FALSE si la contraseña no coincide.");
        }

        [TestMethod]
        public void CambiarContraseña()
        {
            // Arrange
            string contraseñaInicial = "@Contraseñasegura123";
            string contraseñaNueva = "NuevaContraseña456";
            Usuario u = new Usuario(1, "N", "A", "e", contraseñaInicial);

            // Este test está en ROJO
            bool resultado = u.cambiarContraseña(contraseñaInicial, contraseñaNueva);
            bool resultadoFalso = u.cambiarContraseña("ContraseñaFalsa", contraseñaNueva);

            // Assert
            Assert.IsTrue(resultado, "Debería retornar TRUE al cambiar la contraseña con la anterior correcta.");
            Assert.AreEqual(contraseñaNueva, u.Contraseña, "La contraseña no fue actualizada.");

            Assert.IsFalse(resultadoFalso, "Debería retornar FALSE al fallar la validación de la contraseña anterior.");
            // Verificamos que la contraseña original se mantiene
            Assert.AreEqual(contraseñaInicial, u.Contraseña, "La contraseña NO debería haber sido cambiada.");
        }
    }
}
