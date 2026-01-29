using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CentroDeportivo.Model.Repositorios
{
    public class SociosRepositorio
    {
        public List<Socios> ObtenerSocios()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Socios.AsNoTracking().ToList();
            }
        }

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
        public bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}