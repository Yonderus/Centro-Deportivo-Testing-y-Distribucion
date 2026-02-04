using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CentroDeportivo.Views;

namespace CentroDeportivo.ViewModel
{
    /// <summary>
    /// ViewModel principal del menú: abre las distintas ventanas.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Abre la ventana de socios.
        /// </summary>
        public ICommand AbrirSocio { get; }

        /// <summary>
        /// Abre la ventana de actividades.
        /// </summary>
        public ICommand AbrirActividades { get; }

        /// <summary>
        /// Abre la ventana de reservas.
        /// </summary>
        public ICommand AbrirReservas { get; }

        /// <summary>
        /// Inicializa los comandos del menú.
        /// </summary>
        public MainViewModel()
        {
            AbrirSocio = new RelayCommand(AbrirVentanaSocio);
            AbrirActividades = new RelayCommand(AbrirVentanaActividades);
            AbrirReservas = new RelayCommand(AbrirVentanaReservas);
        }

        private void AbrirVentanaSocio()
        {
            sociosW winS = new sociosW();
            winS.ShowDialog();
        }

        private void AbrirVentanaActividades()
        {
            actividadesW winA = new actividadesW();
            winA.ShowDialog();
        }

        private void AbrirVentanaReservas()
        {
            reservasW winR = new reservasW();
            winR.ShowDialog();
        }

        /// <summary>
        /// Se dispara cuando cambia una propiedad para refrescar la vista.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
