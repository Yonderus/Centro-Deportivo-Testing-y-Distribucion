using System;
using CentroDeportivo.Model.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace centroDeportivosTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Validacion_FormatoEmail()
        {
            var repo = new SociosRepositorio();

            Assert.IsTrue(repo.EmailValido("usuario@dominio.com"));
            Assert.IsFalse(repo.EmailValido("usuario.com"));
        }

        [TestMethod]
        public void Validacion_FechaReserva_NoPermite_AntesDeHoy()
        {
            var repo = new ReservasRepositorio();

            Assert.IsFalse(repo.FechaActualDisponible(DateTime.Today.AddDays(-1)));
            Assert.IsTrue(repo.FechaActualDisponible(DateTime.Today));
        }

        [TestMethod]
        public void Control_AforoMaximo_Aforo1_Rechaza_SegundaReserva()
        {
            var repo = new ReservasRepositorio();

            var aforoMaximo = 1;

            // Primera reserva (0 existentes) => debe permitir
            Assert.IsTrue(repo.PuedeCrearOtraReserva(aforoMaximo, 0));

            // Segunda reserva (1 existente) => debe rechazar
            Assert.IsFalse(repo.PuedeCrearOtraReserva(aforoMaximo, 1));
        }
    }
}