using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModeloDatos;
using System.Collections.Generic;

namespace ModeloDatosTest
{
    [TestClass]
    public class AppTest
    {
        private static Usuario usuario;
        private static Proyecto proyectoAlpha;
        private static Rol rolEditor;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // 1. Configuración de datos previos (Usuario, Rol, Proyecto)
            usuario = new Usuario(1, "Laura", "Diaz", "laura@test.com", "@Pass1234", "Calle A");
            rolEditor = new Rol("Editor", "Edita casos de prueba");
            rolEditor.AñadirPermiso(Permisos.VerCasosPrueba);
            rolEditor.AñadirPermiso(Permisos.EditarCasosPruebaEjecutados);

            proyectoAlpha = new Proyecto(1, "Proyecto Alpha", "PA", "Desc", true, false);
            proyectoAlpha.AñadirRolAsignable(rolEditor);

            // Asignamos el rol al usuario en este proyecto
            proyectoAlpha.AsignarRol(usuario, rolEditor);
        }

        // 1. PRUEBA: Constructor
        [TestMethod]
        public void Constructor_IniciaSesionConUsuario()
        {
            // Act
            // ¡ROJO! La clase App no existe
            App app = new App(usuario);

            // Assert
            Assert.IsNotNull(app, "La instancia de App no debe ser nula.");
            Assert.AreEqual(usuario, app.UsuarioActivo, "El usuario activo no se asignó correctamente.");
            Assert.IsNotNull(app.PermisosPorProyecto, "El diccionario de permisos debe inicializarse.");
        }

        // 2. PRUEBA: CargarPermisos(Proyecto) - Calcula y almacena permisos
        // Este método cumple el requisito: "Se almacenará los derechos y privilegios para cada uno de los proyectos"
        [TestMethod]
        public void CargarPermisos_CalculaYAlmacenaPermisosDelProyecto()
        {
            // Arrange
            App app = new App(usuario);

            // Act
            // ¡ROJO! El método CargarPermisos no existe
            app.CargarPermisos(proyectoAlpha);

            // Assert
            Assert.IsTrue(app.PermisosPorProyecto.ContainsKey(proyectoAlpha), "El proyecto debería estar en el diccionario de permisos.");
            List<Permisos> permisos = app.PermisosPorProyecto[proyectoAlpha];
            Assert.AreEqual(2, permisos.Count, "Debería haber cargado los 2 permisos del rol Editor.");
            Assert.IsTrue(permisos.Contains(Permisos.VerCasosPrueba));
        }

        // 3. PRUEBA: TienePermiso(Proyecto, Permiso) - Verificación directa
        // Cumple el requisito: "Determinar los permisos y derechos que un usuario tiene en un proyecto determinado"
        [TestMethod]
        public void TienePermiso_VerificaSiUsuarioTienePermisoEnProyecto()
        {
            // Arrange
            App app = new App(usuario);
            app.CargarPermisos(proyectoAlpha); // Pre-condición: permisos cargados

            // Act & Assert
            // ¡ROJO! El método TienePermiso no existe
            Assert.IsTrue(app.TienePermiso(proyectoAlpha, Permisos.EditarCasosPruebaEjecutados), "Debería tener permiso de Editar.");
            Assert.IsFalse(app.TienePermiso(proyectoAlpha, Permisos.GestionUsuarios), "NO debería tener permiso de Gestión de Usuarios.");
        }

        // 4. PRUEBA: TienePermiso - Proyecto no cargado o usuario sin roles
        [TestMethod]
        public void TienePermiso_ProyectoNoCargado_RetornaFalse()
        {
            // Arrange
            App app = new App(usuario);
            Proyecto proyectoNuevo = new Proyecto(2, "Beta", "PB", "D", true, true);

            // Act & Assert
            // Si no se han cargado permisos para este proyecto, debe retornar false (seguridad por defecto)
            Assert.IsFalse(app.TienePermiso(proyectoNuevo, Permisos.VerCasosPrueba));
        }
    }
}
