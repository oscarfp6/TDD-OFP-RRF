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
            string nombre = "Oscar";
            string apellidos = "Fuentes Paniego";
            string email = "oscar@gmail.com";
            string contraseña = "@Contraseñasegura123";
            int idUsuario = 1;
            Usuario u = new Usuario(idUsuario, nombre,apellidos , email, contraseña);
            Assert.Equals(idUsuario, u.idUsuario);
            Assert.Equals("Oscar", u.Nombre);
            Assert.Equals("Fuentes Paniego", u.apellidos);
            Assert.IsFalse(u.ComprobarContraseña("@Contraseñasegura1234"));
            Assert.IsTrue(u.ComprobarContraseña("@Contraseñasegura123"));
            Assert.IsFalse(u.cambiarContraseña("anterior", "nueva"));
            Assert.IsTrue(u.cambiarContraseña("@Contraseñasegura123", "nueva"));

        }
    }
}
