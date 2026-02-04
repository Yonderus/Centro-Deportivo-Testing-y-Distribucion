using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CentroDeportivo;

namespace CentroDeportivo.Model.Repositorios
{
    /// <summary>
    /// Acceso a datos de actividades (cargar, crear, editar y eliminar).
    /// </summary>
    public class ActividadesRepositorio
    {
        /// <summary>
        /// Devuelve la lista de actividades guardadas en la base de datos.
        /// </summary>
        public List<Actividades> ObtenerActividades()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Actividades.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Guarda una nueva actividad.
        /// </summary>
        /// <param name="actividad">Actividad a insertar.</param>
        public void Insertar(Actividades actividad)
        {
            using (var db = new CentroDeportivoEntities())
            {
                db.Actividades.Add(actividad);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Actualiza una actividad existente.
        /// </summary>
        /// <param name="actividad">Datos nuevos de la actividad.</param>
        public void Editar(Actividades actividad)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var actividadBD = db.Actividades.Find(actividad.Id);
                if (actividadBD == null) return;

                actividadBD.Nombre = actividad.Nombre;
                actividadBD.AforoMaximo = actividad.AforoMaximo;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina la actividad indicada por id.
        /// </summary>
        /// <param name="id">Id de la actividad.</param>
        public void Eliminar(int id)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var actividadBD = db.Actividades.Find(id);
                if (actividadBD == null) return;

                db.Actividades.Remove(actividadBD);
                db.SaveChanges();
            }
        }
    }
}
