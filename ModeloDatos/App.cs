using System.Collections.Generic;

namespace ModeloDatos
{
    /// <summary>
    /// Representa la sesión de la aplicación para un usuario conectado.
    /// Almacena el estado y los permisos cacheados por proyecto.
    /// </summary>
    public class App
    {
        // Usuario conectado actualmente
        public Usuario UsuarioActivo { get; private set; }

        // Almacén de estado: Diccionario que guarda la lista de permisos para cada proyecto accedido.
        // Clave: Proyecto, Valor: Lista de Permisos calculados.
        public Dictionary<Proyecto, List<Permisos>> PermisosPorProyecto { get; private set; }

        /// <summary>
        /// Constructor que inicia la sesión para un usuario.
        /// </summary>
        /// <param name="usuario">El usuario que se conecta.</param>
        public App(Usuario usuario)
        {
            this.UsuarioActivo = usuario;
            this.PermisosPorProyecto = new Dictionary<Proyecto, List<Permisos>>();
        }

        /// <summary>
        /// Calcula y almacena (cachea) los permisos del usuario activo para un proyecto específico.
        /// Utiliza la lógica de negocio de Proyecto.ObtenerPermisosFinales.
        /// </summary>
        /// <param name="proyecto">El proyecto del cual cargar permisos.</param>
        public void CargarPermisos(Proyecto proyecto)
        {
            if (proyecto != null && UsuarioActivo != null)
            {
                // Delegamos al Proyecto el cálculo de los permisos (Suma de roles)
                List<Permisos> permisosCalculados = proyecto.ObtenerPermisosFinales(UsuarioActivo);

                // Guardamos el resultado en el diccionario de la App
                if (PermisosPorProyecto.ContainsKey(proyecto))
                {
                    PermisosPorProyecto[proyecto] = permisosCalculados;
                }
                else
                {
                    PermisosPorProyecto.Add(proyecto, permisosCalculados);
                }
            }
        }

        /// <summary>
        /// Verifica si el usuario activo tiene un permiso específico en un proyecto dado.
        /// </summary>
        /// <param name="proyecto">El proyecto contexto.</param>
        /// <param name="permiso">El permiso a verificar.</param>
        /// <returns>True si tiene el permiso, False en caso contrario.</returns>
        public bool TienePermiso(Proyecto proyecto, Permisos permiso)
        {
            // 1. Verificar si el proyecto está en nuestro "caché" de sesión
            if (proyecto != null && PermisosPorProyecto.ContainsKey(proyecto))
            {
                // 2. Buscar el permiso en la lista almacenada
                return PermisosPorProyecto[proyecto].Contains(permiso);
            }

            return false; // Acceso denegado por defecto si no se han cargado permisos
        }
    }
}