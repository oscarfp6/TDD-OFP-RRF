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
    public class ProyectoTest
    {
        private static Usuario usuarioAdmin = new Usuario(1, "Oscar", "Fuentes", "o@u.com", "@PasswordSegura123", "Calle X");
        private static Usuario usuarioEditor = new Usuario(2, "Ana", "Gomez", "a@u.com", "@PasswordSegura123", "Calle Y");
        private static Usuario usuarioLector = new Usuario(3, "Pedro", "Ruiz", "p@u.com", "@PasswordSegura123", "Calle Z");
        private static Rol rolAdmin, rolEditor, rolLector;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Crear Roles con Permisos (Basado en el Enum Permisos.cs)
            rolAdmin = new Rol("Administrador", "Todos los permisos");
            rolAdmin.AñadirPermiso(Permisos.GestionUsuarios);
            rolAdmin.AñadirPermiso(Permisos.GestionRoles);
            rolAdmin.AñadirPermiso(Permisos.GestionProyectoPruebas);

            rolEditor = new Rol("Editor", "Edición de casos de prueba");
            rolEditor.AñadirPermiso(Permisos.VerCasosPrueba);
            rolEditor.AñadirPermiso(Permisos.CrearEditarPlanPruebas);
            rolEditor.AñadirPermiso(Permisos.GestionRequisitos); // Permiso 3 (Shared)

            rolLector = new Rol("Lector", "Solo visualización");
            rolLector.AñadirPermiso(Permisos.VerCasosPrueba); // Permiso 1 (Shared)
            rolLector.AñadirPermiso(Permisos.VerRequisitos); // Permiso 2 (Unique)
        }

        [TestMethod]
        public void Constructor_AsignaAtributosYInicializaColecciones()
        {
            // Arrange
            int idEsperado = 1;
            string nombreEsperado = "Proyecto Alpha";
            string prefijoEsperado = "PA";


            Proyecto p = new Proyecto(idEsperado, nombreEsperado, prefijoEsperado, "Descripción", true, false);

            // Assert
            Assert.AreEqual(idEsperado, p.IdProyecto, "El IdProyecto no se asignó.");
            Assert.AreEqual(nombreEsperado, p.Nombre, "El Nombre no se asignó.");
            Assert.AreEqual(prefijoEsperado, p.Prefijo, "El Prefijo no se asignó.");
            Assert.IsTrue(p.Activo, "El estado Activo debe ser True.");
            Assert.IsFalse(p.Publico, "El estado Publico debe ser False.");
            Assert.IsNotNull(p.RolesAsignables, "RolesAsignables no debe ser nulo.");
            Assert.IsNotNull(p.UsuariosEnRoles, "UsuariosEnRoles no debe ser nulo.");
            Assert.AreEqual(0, p.RolesAsignables.Count, "RolesAsignables debe estar vacío.");
        }

        [TestMethod]
        public void AñadirRolAsignable_AgregaRolYNoPermiteDuplicados()
        {
            // Arrange
            Proyecto p = new Proyecto(2, "Proyecto Beta", "PB", "Desc", true, true);

            // Act
            p.AñadirRolAsignable(rolAdmin);
            p.AñadirRolAsignable(rolEditor);
            p.AñadirRolAsignable(rolAdmin); // Intentar duplicado

            // Assert
            Assert.AreEqual(2, p.RolesAsignables.Count, "Solo se deben agregar 2 roles únicos.");
            Assert.IsTrue(p.RolesAsignables.Contains(rolAdmin), "Debe contener rolAdmin.");
        }

        [TestMethod]
        public void AsignarRol_RolAsignable_AsignaRolAUsuario()
        {
            // Arrange
            Proyecto p = new Proyecto(3, "Proyecto Gamma", "PG", "Desc", true, false);
            p.AñadirRolAsignable(rolEditor);

            // Act
            // ¡ROJO! El método AsignarRol() aún no existe
            p.AsignarRol(usuarioEditor, rolEditor);

            // Assert
            Assert.IsTrue(p.UsuariosEnRoles.ContainsKey(usuarioEditor), "El usuario debe estar en el diccionario.");
            Assert.IsTrue(p.UsuariosEnRoles[usuarioEditor].Contains(rolEditor), "El rol Editor debe estar asignado.");
        }

        [TestMethod]
        public void AsignarRol_MultiplesRoles_AsignaCorrectamente()
        {
            // Arrange
            Proyecto p = new Proyecto(4, "Proyecto Delta", "PD", "Desc", true, false);
            p.AñadirRolAsignable(rolEditor);
            p.AñadirRolAsignable(rolLector);

            // Act
            p.AsignarRol(usuarioEditor, rolEditor);
            p.AsignarRol(usuarioEditor, rolLector);
            p.AsignarRol(usuarioEditor, rolEditor); // Intento de duplicado

            // Assert
            Assert.AreEqual(2, p.UsuariosEnRoles[usuarioEditor].Count, "El usuario debe tener 2 roles únicos asignados.");
            Assert.IsTrue(p.UsuariosEnRoles[usuarioEditor].Contains(rolEditor));
            Assert.IsTrue(p.UsuariosEnRoles[usuarioEditor].Contains(rolLector));
        }

        [TestMethod]
        public void ObtenerPermisosFinales_SumaPermisosDeRolesUnicos()
        {
            // Arrange
            Proyecto p = new Proyecto(5, "Proyecto Epsilon", "PE", "Desc", true, false);
            p.AñadirRolAsignable(rolEditor); // Permisos: Ver, CrearEditarPlan, GestionReq
            p.AñadirRolAsignable(rolLector);  // Permisos: Ver, VerReq

            p.AsignarRol(usuarioEditor, rolEditor);
            p.AsignarRol(usuarioEditor, rolLector);

            // Act
            // ¡ROJO! El método ObtenerPermisosFinales() aún no existe
            List<Permisos> permisosFinales = p.ObtenerPermisosFinales(usuarioEditor);

            // Expected Permisos (Union of Editor + Lector):
            // 1. VerCasosPrueba (Compartido)
            // 2. CrearEditarPlanPruebas (Editor único)
            // 3. GestionRequisitos (Editor único)
            // 4. VerRequisitos (Lector único)
            int numPermisosUnicosEsperados = 4;

            // Assert
            Assert.AreEqual(numPermisosUnicosEsperados, permisosFinales.Count, "El conteo de permisos finales únicos es incorrecto.");
            Assert.IsTrue(permisosFinales.Contains(Permisos.VerCasosPrueba));
            Assert.IsTrue(permisosFinales.Contains(Permisos.GestionRequisitos));
            Assert.IsTrue(permisosFinales.Contains(Permisos.VerRequisitos));
            Assert.IsFalse(permisosFinales.Contains(Permisos.GestionUsuarios), "No debe tener permisos de Admin.");
        }

        [TestMethod]
        public void ObtenerPermisosFinales_UsuarioSinRoles_RetornaListaVacia()
        {
            // Arrange
            Proyecto p = new Proyecto(6, "Proyecto Zeta", "PZ", "Desc", true, false);

            // Act
            List<Permisos> permisosFinales = p.ObtenerPermisosFinales(usuarioLector);

            // Assert
            Assert.IsNotNull(permisosFinales);
            Assert.AreEqual(0, permisosFinales.Count, "Un usuario sin roles debe tener 0 permisos.");
        }
    }
}
