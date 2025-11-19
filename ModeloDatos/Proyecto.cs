using System.Collections.Generic;
using System.Linq;

namespace ModeloDatos
{
    public class Proyecto
    {
        public int _idProyecto;
        public string _nombre;
        public string _prefijo;
        public string _descripcion;

        public int IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Prefijo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public bool Publico { get; set; }
        public List<Rol> RolesAsignables { get; private set; }
        public Dictionary<Usuario, List<Rol>> UsuariosEnRoles { get; private set; }

        /// <summary>
        /// Constructor del Proyecto.
        /// </summary>
        public Proyecto(int idProyecto, string nombre, string prefijo, string descripcion, bool activo, bool publico)
        {
            this.IdProyecto = idProyecto;
            this.Nombre = nombre;
            this.Prefijo = prefijo;
            this.Descripcion = descripcion;

            this.Activo = activo;
            this.Publico = publico;
            this.RolesAsignables = new List<Rol>();
            this.UsuariosEnRoles = new Dictionary<Usuario, List<Rol>>();
        }

        /// <summary>
        /// Añade un rol a la lista de roles que pueden asignarse en este proyecto.
        /// </summary>
        public void AñadirRolAsignable(Rol rol)
        {
            if (rol != null && !RolesAsignables.Contains(rol))
            {
                this.RolesAsignables.Add(rol);
            }
        }

        /// <summary>
        /// Asigna un rol a un usuario en el contexto de este proyecto.
        /// Solo asigna si el rol es 'asignable' en este proyecto.
        /// </summary>
        public void AsignarRol(Usuario usuario, Rol rol)
        {
            // Solo se puede asignar un rol si está permitido en este proyecto (RolesAsignables)
            if (usuario == null || rol == null || !RolesAsignables.Contains(rol))
            {
                return;
            }

            // Inicializar la lista si es la primera asignación al usuario
            if (!UsuariosEnRoles.ContainsKey(usuario))
            {
                UsuariosEnRoles[usuario] = new List<Rol>();
            }

            // Evitar asignar el mismo rol múltiples veces
            if (!UsuariosEnRoles[usuario].Contains(rol))
            {
                UsuariosEnRoles[usuario].Add(rol);
            }
        }

        /// <summary>
        /// Método clave que calcula la suma de todos los permisos únicos de un usuario en el proyecto.
        /// (Cumple el Requisito 5 y prepara la lógica para la clase App).
        /// </summary>
        /// <param name="usuario">El usuario cuyos permisos se van a calcular.</param>
        /// <returns>Lista de Permisos únicos (la unión de los permisos de todos sus roles).</returns>
        public List<Permisos> ObtenerPermisosFinales(Usuario usuario)
        {
            // Verificar si el usuario tiene roles asignados
            if (usuario != null && UsuariosEnRoles.ContainsKey(usuario))
            {
                // Utiliza LINQ:
                // 1. Selecciona todos los permisos (Permisos) de todos los roles (UsuariosEnRoles[usuario])
                // 2. Utiliza Distinct() para obtener solo los permisos ÚNICOS (la 'suma' o unión).
                return UsuariosEnRoles[usuario]
                       .SelectMany(rol => rol.Permisos)
                       .Distinct()
                       .ToList();
            }
            return new List<Permisos>();
        }


    }
}