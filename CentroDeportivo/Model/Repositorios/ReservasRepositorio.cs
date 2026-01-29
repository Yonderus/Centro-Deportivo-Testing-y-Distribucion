using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CentroDeportivo;

namespace CentroDeportivo.Model.Repositorios
{
    public class ReservasRepositorio
    {
        public List<Reservas> ObtenerReservas()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Reservas.AsNoTracking().ToList();
            }
        }

        public void Insertar(Reservas reserva)
        {
            using (var db = new CentroDeportivoEntities())
            {
                db.Reservas.Add(reserva);
                db.SaveChanges();
            }
        }

        public void Editar(Reservas reserva)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var reservaBD = db.Reservas.Find(reserva.Id);
                if (reservaBD == null) return;

                reservaBD.SocioId = reserva.SocioId;
                reservaBD.ActividadId = reserva.ActividadId;
                reservaBD.Fecha = reserva.Fecha;

                db.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            using (var db = new CentroDeportivoEntities())
            {
                var reservaBD = db.Reservas.Find(id);
                if (reservaBD == null) return;

                db.Reservas.Remove(reservaBD);
                db.SaveChanges();
            }
        }

        public int ContarReservasActividadDia(int actividadId, DateTime fecha)
        {
            var dia = fecha.Date;

            using (var db = new CentroDeportivoEntities())
            {
                return db.Reservas.AsNoTracking()
                    .Count(r => r.ActividadId == actividadId && DbFunctions.TruncateTime(r.Fecha) == dia);
            }
        }
        public bool FechaActualDisponible(DateTime fecha)
        {
            return fecha.Date >= DateTime.Today;
        }

        public bool PuedeCrearOtraReserva(int aforoMaximo, int reservasExistentes)
        {
            if (aforoMaximo <= 0)
            {
                return true;
            }

            return reservasExistentes < aforoMaximo;
        }
    }
}