using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModeloDatosTest
{
    [TestClass]
    public class AppTest
    {
        private  Usuario usuario;
        private App app;
        private Proyecto proyectoAlpha;
        private Proyecto proyectoBeta;
        private Rol rolEditor;
        private Rol rolLector;

        [TestInitialize()]
        public void TestInitialize()
        {
            // 1. Configuración de datos previos (Usuario, Rol, Proyecto)
            usuario = new Usuario(1, "Laura", "Diaz", "laura@test.com", "@Pass123456789", "Calle A");

            rolEditor = new Rol("Editor", "Edita casos de prueba");
            rolEditor.AñadirPermiso(Permisos.VerCasosPrueba);
            rolEditor.AñadirPermiso(Permisos.EditarCasosPruebaEjecutados);

            rolLector = new Rol("Lector", "Solo lee");
            rolLector.AñadirPermiso(Permisos.VerCasosPrueba);

            proyectoAlpha = new Proyecto(1, "Proyecto Alpha", "PA", "Desc", true, false);
            proyectoAlpha.AñadirRolAsignable(rolEditor);
            proyectoAlpha.AñadirRolAsignable(rolLector);

            proyectoBeta = new Proyecto(2, "Proyecto Beta", "PB", "Desc", true, true);

            // 2. Instanciamos la App (SUT - System Under Test)
            app = new App(usuario);
        }

        // 1. PRUEBA: Constructor
        [TestMethod]
        public void Constructor_IniciaSesionConUsuarioYColeccionesVacias()
        {
            Assert.IsNotNull(app, "La instancia de App no debe ser nula.");
            Assert.AreEqual(usuario, app.UsuarioActivo, "El usuario activo no se asignó correctamente.");

            // Verificamos que las colecciones existan (aunque estén vacías)
            Assert.IsNotNull(app.PermisosPorProyecto, "El diccionario de permisos debe inicializarse.");

            
            Assert.AreEqual(0, app.ObtenerProyectos().Count);
        }

        // 2. PRUEBA: CargarPermisos(Proyecto) - Calcula y almacena permisos
        // Este método cumple el requisito: "Se almacenará los derechos y privilegios para cada uno de los proyectos"
        [TestMethod]
        public void CargarPermisos_CalculaYAlmacenaPermisosDelProyecto()
        {
            // Arrange: Configuramos el proyecto con sus roles internos (Lógica antigua)
            // Para que esto funcione, el Proyecto debe saber que el usuario tiene roles
            proyectoAlpha.AsignarRol(usuario, rolEditor);

            // Act
            app.CargarPermisos(proyectoAlpha);

            // Assert
            Assert.IsTrue(app.PermisosPorProyecto.ContainsKey(proyectoAlpha), "El proyecto debería estar en el diccionario de permisos.");
            List<Permisos> permisos = app.PermisosPorProyecto[proyectoAlpha];

            // El rolEditor tiene 2 permisos
            Assert.AreEqual(2, permisos.Count, "Debería haber cargado los permisos calculados.");
            Assert.IsTrue(permisos.Contains(Permisos.VerCasosPrueba));
        }

        // 3. PRUEBA: TienePermiso(Proyecto, Permiso) - Verificación directa
        // Cumple el requisito: "Determinar los permisos y derechos que un usuario tiene en un proyecto determinado"
        [TestMethod]
        public void TienePermiso_VerificaSiUsuarioTienePermisoEnProyecto()
        {
            proyectoAlpha.AsignarRol(usuario, rolEditor); // Configuración lado Proyecto
            app.CargarPermisos(proyectoAlpha); // Pre-condición: permisos cargados en App

            // Act & Assert
            Assert.IsTrue(app.TienePermiso(proyectoAlpha, Permisos.EditarCasosPruebaEjecutados), "Debería tener permiso de Editar.");
            Assert.IsFalse(app.TienePermiso(proyectoAlpha, Permisos.GestionUsuarios), "NO debería tener permiso de Gestión de Usuarios.");
        }

        // 4. PRUEBA: TienePermiso - Proyecto no cargado o usuario sin roles
        [TestMethod]
        public void TienePermiso_ProyectoNoCargado_RetornaFalse()
        {
            // Act & Assert
            // Si no se han cargado permisos para este proyecto, debe retornar false
            Assert.IsFalse(app.TienePermiso(proyectoBeta, Permisos.VerCasosPrueba));
        }

        [TestMethod]
        public void AgregarParticipacion_PrimerRol_DebeAgregarProyecto()
        {
            // Act: Asignamos un rol a través de la App (Requisito del PDF)
            // Nota: Este método requerirá que actualices App.cs para incluir AgregarParticipacion
            app.AgregarParticipacion(proyectoAlpha, rolEditor);

            // Assert
            var proyectos = app.ObtenerProyectos();
            Assert.AreEqual(1, proyectos.Count);
            Assert.AreEqual(proyectoAlpha, proyectos[0]);
        }

        [TestMethod]
        public void AgregarParticipacion_NoDebeDuplicarProyecto()
        {
            // Act: Añadimos dos roles al mismo proyecto
            app.AgregarParticipacion(proyectoAlpha, rolEditor);
            app.AgregarParticipacion(proyectoAlpha, rolLector);

            // Assert
            var proyectos = app.ObtenerProyectos();
            Assert.AreEqual(1, proyectos.Count, "El proyecto no debe duplicarse en la lista.");
        }

        [TestMethod]
        public void ObtenerRolesEnProyecto_DebeDevolverTodosLosRolesAsignados()
        {
            // Arrange
            app.AgregarParticipacion(proyectoAlpha, rolEditor);
            app.AgregarParticipacion(proyectoAlpha, rolLector);

            // Act
            var roles = app.ObtenerRolesEnProyecto(proyectoAlpha);

            // Assert
            Assert.AreEqual(2, roles.Count);
            Assert.IsTrue(roles.Contains(rolEditor));
            Assert.IsTrue(roles.Contains(rolLector));
        }

        [TestMethod]
        public void MostrarInformacion_DebeFormatearSalidaCorrectamente()
        {
            // Arrange
            app.AgregarParticipacion(proyectoAlpha, rolEditor);

            // Act
            string resultado = app.MostrarInformacion();

            // Assert
            // Usamos StringAssert.Contains para verificar partes del texto
            StringAssert.Contains(resultado, "Laura Diaz");
            StringAssert.Contains(resultado, "Proyecto Alpha");
            StringAssert.Contains(resultado, "Editor");
        }

        /// <summary>
        /// Prueba DDT usando DataRow (Equivalente a TestCase en NUnit).
        /// Verifica que se añadan roles correctamente con diferentes datos.
        /// </summary>
        [DataTestMethod]
        [DataRow("Proyecto A", "RolAdmin", 1)]
        [DataRow("Proyecto B", "RolInvitado", 1)]
        public void DDT_AgregarParticipacion_VerificarConteo(string nombreProy, string nombreRol, int cantidadEsperada)
        {
            // Arrange
            var proy = new Proyecto(99, nombreProy, "P", "D", true, true);
            var rol = new Rol(nombreRol, "Desc");

            // Act
            app.AgregarParticipacion(proy, rol);

            // Assert
            Assert.AreEqual(cantidadEsperada, app.ObtenerRolesEnProyecto(proy).Count);
            Assert.AreEqual(nombreRol, app.ObtenerRolesEnProyecto(proy)[0].Nombre);
        }
    }
}
