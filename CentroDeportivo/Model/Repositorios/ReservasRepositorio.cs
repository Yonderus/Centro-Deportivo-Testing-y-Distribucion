using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CentroDeportivo;

namespace CentroDeportivo.Model.Repositorios
{
    /// <summary>
    /// Acceso a datos de reservas y pequeñas utilidades (fecha/aforo).
    /// </summary>
    public class ReservasRepositorio
    {
        /// <summary>
        /// Devuelve la lista completa de reservas.
        /// </summary>
        public List<Reservas> ObtenerReservas()
        {
            using (var db = new CentroDeportivoEntities())
            {
                return db.Reservas.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Guarda una nueva reserva.
        /// </summary>
        /// <param name="reserva">Reserva a insertar.</param>
        public void Insertar(Reservas reserva)
        {
            using (var db = new CentroDeportivoEntities())
            {
                db.Reservas.Add(reserva);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Actualiza una reserva existente.
        /// </summary>
        /// <param name="reserva">Datos nuevos de la reserva.</param>
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

        /// <summary>
        /// Elimina la reserva indicada por id.
        /// </summary>
        /// <param name="id">Id de la reserva.</param>
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

        /// <summary>
        /// Cuenta cuántas reservas hay para una actividad en un día concreto.
        /// </summary>
        /// <param name="actividadId">Id de la actividad.</param>
        /// <param name="fecha">Día a comprobar.</param>
        public int ContarReservasActividadDia(int actividadId, DateTime fecha)
        {
            var dia = fecha.Date;

            using (var db = new CentroDeportivoEntities())
            {
                return db.Reservas.AsNoTracking()
                    .Count(r => r.ActividadId == actividadId && DbFunctions.TruncateTime(r.Fecha) == dia);
            }
        }

        /// <summary>
        /// Indica si la fecha es hoy o posterior.
        /// </summary>
        /// <param name="fecha">Fecha a comprobar.</param>
        public bool FechaActualDisponible(DateTime fecha)
        {
            return fecha.Date >= DateTime.Today;
        }

        /// <summary>
        /// Indica si se puede crear otra reserva según el aforo y las reservas ya existentes.
        /// </summary>
        /// <param name="aforoMaximo">Aforo máximo permitido.</param>
        /// <param name="reservasExistentes">Reservas ya realizadas.</param>
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