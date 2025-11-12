using System.Linq; 
using System.Text.RegularExpressions; 

namespace MiLogica.Utils
{
    /// <summary>
    /// Proporciona métodos de utilidad estáticos para validaciones complejas de formatos
    /// </summary>
    public static class Valid
    {
        /// <summary>
        /// Valida si una contraseña cumple con los requisitos mínimos de seguridad.
        /// </summary>
        /// <param name="password">La contraseña en texto plano.</param>
        /// <returns>True si es segura, False si no cumple algún requisito.</returns>
        public static bool ValidarPassword(string password)
        {
            // --- Reglas de Validación (Requisitos de Seguridad) ---

            // Requisito 1: Longitud Mínima (12 caracteres).
            // Este es el primer filtro y una de las medidas más importantes contra ataques de fuerza bruta.
            if (password.Length < 12) return false;

            // Requisito 2: Al menos una letra Mayúscula.
            // Utiliza LINQ para comprobar si existe algún carácter que sea mayúscula.
            if (!password.Any(char.IsUpper)) return false;

            // Requisito 3: Al menos una letra Minúscula.
            if (!password.Any(char.IsLower)) return false;

            // Requisito 4: Al menos un Dígito numérico (0-9).
            if (!password.Any(char.IsDigit)) return false;

            // Requisito 5: Al menos un Carácter Especial (no alfanumérico).
            // Comprueba si existe algún carácter que NO sea una letra o un dígito.
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;

            // Si pasa todos los filtros, la contraseña es considerada segura.
            return true;
        }

        /// <summary>
        /// Valida si una cadena de texto tiene un formato de email básico.
        /// Esta es una validación de "caja blanca": conocemos las reglas
        /// internas (debe tener '@', '.', no '..', etc.) y las probamos.
        /// </summary>
        /// <param name="email">El email a validar.</param>
        /// <returns>True si el formato es aceptable, False si no lo es.</returns>
        public static bool ValidarEmail(string email)
        {
            // --- Prueba de Caja Negra (Valor Límite) ---
            // Caso 1: ¿Qué pasa si el email es nulo, vacío o solo espacios?
            // 'IsNullOrWhiteSpace' es la forma más robusta de comprobar esto.
            if (string.IsNullOrWhiteSpace(email)) return false;

            // --- Prueba de Caja Blanca (Caso Específico Inválido) ---
            // Caso 2: Un email no puede tener dos puntos seguidos (ej. "test@gmail..com")
            if (email.Contains("..")) return false;

            // --- Lógica de Particiones de Equivalencia ---
            // Un email válido se parte en 3: [local]@[dominio].[tld]
            // Buscamos la posición del '@' y del ÚLTIMO '.'

            // Caso 3: Encontrar el '@'.
            int atIndex = email.IndexOf('@');

            // Caso 4: Encontrar el ÚLTIMO '.' (para manejar subdominios ej. "test@mail.google.com")
            int dotIndex = email.LastIndexOf('.');

            // --- Validación de Reglas/Estructura ---
            // 1. (atIndex > 0): Debe haber un '@' y no puede ser el primer caracter.
            // 2. (dotIndex > atIndex + 1): Debe haber un '.' DESPUÉS del '@' y no justo pegado (ej. "test@.com").
            // 3. (dotIndex < email.Length - 1): No puede ser el último caracter (ej. "test@gmail.").
            return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.Length - 1;
        }

        /// <summary>
        /// Valida que una cadena de nombre no esté vacía y no contenga dígitos.
        /// </summary>
        /// <param name="input">La cadena a validar (nombre o apellido).</param>
        /// <returns>True si es un nombre/apellido válido, False en caso contrario.</returns>
        public static bool Nombre(string input)
        {
            // 1. Comprobación de existencia
            if (string.IsNullOrWhiteSpace(input)) return false;

            // 2. Comprobación de dígitos
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return false; // Contiene un número
                }
            }

            return true;

        }

    }
}
