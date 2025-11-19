using System;
using System.Collections.Generic;
using System.Linq; // Necesario para Linq

namespace ModeloDatos
{
    public class Rol
    {
        public string _nombre;
        public string _descripción;

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public List<Permisos> Permisos { get; private set; }

        /// <summary>
        /// Constructor del Rol.
        /// </summary>
        /// <param name="descripcion">Descripción del rol.</param>
        /// <param name="nombre">Nombre del rol.</param>
        public Rol(string nombre, string descripcion)
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.Permisos = new List<Permisos>(); // Inicializar la colección
        }

        /// <summary>
        /// Añade un nuevo permiso a la colección del rol.
        /// </summary>
        /// <param name="permiso">Permiso a añadir.</param>
        public void AñadirPermiso(Permisos permiso)
        {
            // Asegurarse de que el permiso no esté duplicado
            if (!Permisos.Contains(permiso))
            {
                this.Permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Verifica si el rol tiene un permiso específico.
        /// </summary>
        /// <param name="permiso">Permiso a verificar.</param>
        /// <returns>True si el permiso existe, False en caso contrario.</returns>
        public bool TienePermiso(Permisos permiso)
        {
            return this.Permisos.Contains(permiso);
        }

        /// <summary>
        /// Elimina un permiso de la colección del rol.
        /// </summary>
        /// <param name="permiso">Permiso a eliminar.</param>
        public void EliminarPermiso(Permisos permiso)
        {
            // List<T>.Remove() devuelve true si lo encuentra y elimina.
            this.Permisos.Remove(permiso);
        }

        /// <summary>
        /// Reemplaza un permiso existente por uno nuevo.
        /// </summary>
        /// <param name="anterior">Permiso a eliminar.</param>
        /// <param name="actual">Permiso a agregar.</param>
        /// <returns>True si el reemplazo fue exitoso (es decir, si el permiso anterior existía).</returns>
        public bool CambiarPermiso(Permisos anterior, Permisos actual)
        {
            if (this.Permisos.Contains(anterior))
            {
                // 1. Eliminar el anterior
                this.Permisos.Remove(anterior);
                // 2. Añadir el nuevo (AñadirPermiso manejará el caso de duplicados)
                this.AñadirPermiso(actual);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Devuelve la lista completa de permisos que tiene este rol.
        /// </summary>
        public List<Permisos> ObtenerPermisos()
        {
            return this.Permisos.ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is Rol rol &&
                   Nombre.Equals(rol.Nombre, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Nombre.GetHashCode();
        }

    }
}