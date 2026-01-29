using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CentroDeportivo;

namespace CentroDeportivo.Model.Repositorios
{
    public class ActividadesRepositorio
    {
        public List<Actividades> ObtenerActividades()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Actividades.AsNoTracking().ToList();
            }
        }

        public void Insertar(Actividades actividad)
        {
            using (var db = new CentroDeportivoEntities())
            {
                db.Actividades.Add(actividad);
                db.SaveChanges();
            }
        }

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
