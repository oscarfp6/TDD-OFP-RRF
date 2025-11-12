using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiLogica.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloDatosTest.Utils
{
    [TestClass()]
    public class ValidTests
    {
        [TestMethod()]
        public void NombreTest()
        {
            // Caso 1: Nombre válido, solo letras (se espera éxito).
            Assert.IsTrue(Valid.Nombre("Oscar"));
            // Caso 2: Nombre con dígitos (se espera fallo, ya que los nombres no deben contener números).
            Assert.IsFalse(Valid.Nombre("Oscar4"));
            // Caso 3: Cadena vacía (se espera fallo).
            Assert.IsFalse(Valid.Nombre(""));
            // Caso 4: Cadena con solo espacios en blanco (se espera fallo).
            Assert.IsFalse(Valid.Nombre(" "));
        }

        [TestMethod()]
        public void ValidarPasswordTest() 
        {
            // Assert: Verifica un caso de éxito esperado (contraseña válida).
            Assert.IsTrue(Valid.ValidarPassword("@Contraseñavalida123"));
            // Assert: Verifica un caso de fallo esperado (ej. contraseña muy corta).
            Assert.IsFalse(Valid.ValidarPassword("short1A@"));
        }

        [TestMethod()]
        public void ValidarEmailTest()
        {
            // Caso de Partición Inválida: Le falta el separador principal '@'.
            Assert.IsFalse(Valid.ValidarEmail("oscargmail.com"));

            // Caso de Partición Inválida: Le falta el punto de separación del TLD (Top-Level Domain).
            Assert.IsFalse(Valid.ValidarEmail("oscar@gmailcom"));

            // Caso de Partición Inválida: El punto está justo después del '@' (ej. local@.com).
            Assert.IsFalse(Valid.ValidarEmail("oscar@.com"));

            // Caso de Partición Inválida: Termina en punto (le falta el TLD).
            Assert.IsFalse(Valid.ValidarEmail("oscar@gmail."));

            // Caso de Partición Válida: Formato estándar correcto.
            Assert.IsTrue(Valid.ValidarEmail("gepeto@gmail.com"));

            // Caso de Partición Inválida: Puntos consecutivos en el dominio.
            Assert.IsFalse(Valid.ValidarEmail("oscar@gmail..com"));
        }
    }
}
