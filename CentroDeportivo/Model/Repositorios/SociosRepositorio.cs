using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CentroDeportivo.Model.Repositorios
{
    /// <summary>
    /// Acceso a datos de socios y validaciones básicas relacionadas.
    /// </summary>
    public class SociosRepositorio
    {
        /// <summary>
        /// Devuelve la lista completa de socios.
        /// </summary>
        public List<Socios> ObtenerSocios()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Socios.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Inserta un nuevo socio (validando el email).
        /// </summary>
        /// <param name="socio">Socio a insertar.</param>
        public void Insertar(Socios socio)
        {
            if (socio == null) throw new ArgumentNullException(nameof(socio));

            if (!EmailValido(socio.Email))
            {
                throw new ArgumentException("Email inválido");
            }

            using (var db = new CentroDeportivoEntities())
            {
                db.Socios.Add(socio);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina el socio indicado por id.
        /// </summary>
        /// <param name="id">Id del socio.</param>
        public void Eliminar(int id)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var socio = db.Socios.Find(id);
                if (socio == null) return;
                db.Socios.Remove(socio);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Actualiza los datos del socio.
        /// </summary>
        /// <param name="socio">Datos nuevos del socio.</param>
        public void Editar(Socios socio)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var socioBD = db.Socios.Find(socio.Id);
                if (socioBD == null) return;

                socioBD.Nombre = socio.Nombre;
                socioBD.Email = socio.Email;
                socioBD.Activo = socio.Activo;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Comprueba si un email tiene un formato válido.
        /// </summary>
        /// <param name="email">Email a validar.</param>
        public bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}