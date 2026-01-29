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
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand AbrirSocio { get; }
        public ICommand AbrirActividades { get; }
        public ICommand AbrirReservas { get; }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }   
}
