using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        // Diccionario que asocia proyectos con los roles que el usuario tiene en ellos en esta sesión.
        // El uso de Dictionary garantiza que no haya proyectos duplicados como claves.
        private Dictionary<Proyecto, List<Rol>> _participaciones;

        /// <summary>
        /// Constructor que inicia la sesión para un usuario.
        /// </summary>
        /// <param name="usuario">El usuario que se conecta.</param>
        public App(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            this.UsuarioActivo = usuario;

            this.PermisosPorProyecto = new Dictionary<Proyecto, List<Permisos>>();
            this._participaciones = new Dictionary<Proyecto, List<Rol>>();
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

        /// <summary>
        /// Agrega un proyecto y un rol asociado a la sesión actual.
        /// Cumple los requisitos:
        /// - Agregar múltiples roles al mismo proyecto sin duplicarlos.
        /// - No duplicar proyectos en la lista.
        /// </summary>
        public void AgregarParticipacion(Proyecto proyecto, Rol rol)
        {
            if (proyecto == null) throw new ArgumentNullException(nameof(proyecto));
            if (rol == null) throw new ArgumentNullException(nameof(rol));

            // 1. Si el proyecto no existe en el diccionario, lo registramos
            if (!_participaciones.ContainsKey(proyecto))
            {
                _participaciones[proyecto] = new List<Rol>();
            }

            // 2. Obtenemos la lista de roles para ese proyecto
            List<Rol> rolesDelProyecto = _participaciones[proyecto];

            // 3. Verificamos si el rol ya existe para evitar duplicados
            // (Requiere que la clase Rol tenga Equals implementado correctamente, lo cual ya está hecho)
            if (!rolesDelProyecto.Contains(rol))
            {
                rolesDelProyecto.Add(rol);
            }
        }

        /// <summary>
        /// Obtiene la lista de proyectos en los que participa el usuario.
        /// </summary>
        public List<Proyecto> ObtenerProyectos()
        {
            return _participaciones.Keys.ToList();
        }

        /// <summary>
        /// Obtiene los roles del usuario en un proyecto específico.
        /// </summary>
        public List<Rol> ObtenerRolesEnProyecto(Proyecto proyecto)
        {
            if (proyecto != null && _participaciones.ContainsKey(proyecto))
            {
                return _participaciones[proyecto];
            }
            // Retorna lista vacía si no participa, para evitar NullReferenceException en quien lo llame
            return new List<Rol>();
        }

        /// <summary>
        /// Genera una representación textual del usuario con sus proyectos y roles.
        /// Formato esperado:
        /// Usuario: [Nombre]
        /// Proyectos:
        /// [Proyecto]: [Rol1], [Rol2]
        /// </summary>
        public string MostrarInformacion()
        {
            StringBuilder sb = new StringBuilder();
            // Usamos Nombre y Apellidos de la clase Usuario
            sb.AppendLine($"Usuario: {UsuarioActivo.Nombre} {UsuarioActivo.Apellidos}");

            if (_participaciones.Count > 0)
            {
                sb.AppendLine("Proyectos:");
                foreach (KeyValuePair<Proyecto, List<Rol>> entrada in _participaciones)
                {
                    string nombreProyecto = entrada.Key.Nombre;

                    // Usamos LINQ para obtener los nombres de los roles y string.Join para unirlos con comas
                    var nombresRoles = entrada.Value.Select(r => r.Nombre);
                    string rolesStr = string.Join(", ", nombresRoles);

                    sb.AppendLine($"{nombreProyecto}: {rolesStr}");
                }
            }
            else
            {
                sb.AppendLine("Sin participaciones activas.");
            }

            return sb.ToString();
        }

    }
}