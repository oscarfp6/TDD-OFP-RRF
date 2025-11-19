using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloDatosTest
{
    [TestClass]
    public class RolTest
    {
        private const string NOMBRE_ROL = "Administrador";
        private const string DESCRIPCION_ROL = "Acceso total al sistema y gestión de usuarios.";

        [TestMethod]
        public void Constructor_AsignaNombreDescripcionYListaPermisosVacia()
        {
            // Act
            // ¡ROJO! La clase Rol aún no existe
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);

            // Assert
            Assert.IsNotNull(r, "El objeto Rol no debe ser nulo.");
            Assert.AreEqual(NOMBRE_ROL, r.Nombre, "El Nombre no se asignó correctamente.");
            Assert.AreEqual(DESCRIPCION_ROL, r.Descripcion, "La Descripción no se asignó correctamente.");
            Assert.IsNotNull(r.Permisos, "La lista de Permisos no debe ser nula.");
            Assert.AreEqual(0, r.Permisos.Count, "La lista de Permisos debe empezar vacía.");
        }

        [TestMethod]
        public void AñadirPermiso_AgregaPermisoYNoPermiteDuplicados()
        {
            // Arrange
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);

            // Act
            // ¡ROJO! El método AñadirPermiso() aún no existe
            r.AñadirPermiso(Permisos.GestionUsuarios);
            r.AñadirPermiso(Permisos.GestionRoles);
            r.AñadirPermiso(Permisos.GestionUsuarios); // Intentar duplicado

            // Assert
            Assert.AreEqual(2, r.Permisos.Count, "Solo deben agregarse 2 permisos únicos.");
            Assert.IsTrue(r.Permisos.Contains(Permisos.GestionUsuarios), "Debe contener el permiso GestionUsuarios.");
        }

        [TestMethod]
        public void TienePermiso_VerificaExistenciaCorrectamente()
        {
            // Arrange
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);
            r.AñadirPermiso(Permisos.GestionUsuarios);

            // Act & Assert
            // ¡ROJO! El método TienePermiso() aún no existe
            Assert.IsTrue(r.TienePermiso(Permisos.GestionUsuarios), "Debería retornar TRUE para un permiso existente.");
            Assert.IsFalse(r.TienePermiso(Permisos.VerRequisitos), "Debería retornar FALSE para un permiso no existente.");
        }

        [TestMethod]
        public void EliminarPermiso_EliminaPermisoExistente()
        {
            // Arrange
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);
            r.AñadirPermiso(Permisos.GestionUsuarios);
            r.AñadirPermiso(Permisos.GestionRoles);

            // Act
            // ¡ROJO! El método EliminarPermiso() aún no existe
            r.EliminarPermiso(Permisos.GestionUsuarios);

            // Assert
            Assert.AreEqual(1, r.Permisos.Count, "El número de permisos debe disminuir.");
            Assert.IsFalse(r.TienePermiso(Permisos.GestionUsuarios), "El permiso GestionUsuarios debe haber sido eliminado.");
            Assert.IsTrue(r.TienePermiso(Permisos.GestionRoles), "El permiso GestionRoles debe permanecer.");
        }

        [TestMethod]
        public void CambiarPermiso_ReemplazaPermisoExistente_RetornaTrue()
        {
            // Arrange
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);
            r.AñadirPermiso(Permisos.VerCasosPrueba);

            // Act
            // ¡ROJO! El método CambiarPermiso() aún no existe
            bool resultado = r.CambiarPermiso(Permisos.VerCasosPrueba, Permisos.EditarCasosPruebaEjecutados);

            // Assert
            Assert.IsTrue(resultado, "El cambio de permiso debe ser exitoso.");
            Assert.AreEqual(1, r.Permisos.Count, "El conteo de permisos debe ser el mismo.");
            Assert.IsFalse(r.TienePermiso(Permisos.VerCasosPrueba), "El permiso anterior debe desaparecer.");
            Assert.IsTrue(r.TienePermiso(Permisos.EditarCasosPruebaEjecutados), "El nuevo permiso debe estar presente.");
        }

        [TestMethod]
        public void CambiarPermiso_PermisoAnteriorNoExiste_RetornaFalse()
        {
            // Arrange
            Rol r = new Rol(NOMBRE_ROL, DESCRIPCION_ROL);
            r.AñadirPermiso(Permisos.VerCasosPrueba);

            // Act
            bool resultado = r.CambiarPermiso(Permisos.GestionEventos, Permisos.GestionUsuarios); // GestionEventos no existe

            // Assert
            Assert.IsFalse(resultado, "El cambio debe fallar si el permiso anterior no existe.");
            Assert.AreEqual(1, r.Permisos.Count, "El conteo de permisos debe ser el mismo.");
            Assert.IsTrue(r.TienePermiso(Permisos.VerCasosPrueba), "El permiso original debe permanecer inalterado.");
        }


    }
}
