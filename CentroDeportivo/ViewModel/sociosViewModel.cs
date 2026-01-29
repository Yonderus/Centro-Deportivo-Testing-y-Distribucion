using CentroDeportivo;
using CentroDeportivo.Model.Repositorios;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;

namespace CentroDeportivo.ViewModel
{
    public class SociosViewModel : INotifyPropertyChanged
    {
        private readonly SociosRepositorio _repo = new SociosRepositorio();

        private ObservableCollection<Socios> _lista = new ObservableCollection<Socios>();
        public ObservableCollection<Socios> Lista
        {
            get => _lista;
            set { _lista = value; OnPropertyChanged(nameof(Lista)); }
        }

        private int _id;
        private string _nombre;
        private string _email;
        private bool _activo = true;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public bool Activo
        {
            get => _activo;
            set { _activo = value; OnPropertyChanged(nameof(Activo)); }
        }

        private Socios _seleccionado;
        public Socios Seleccionado
        {
            get => _seleccionado;
            set
            {
                _seleccionado = value;
                OnPropertyChanged(nameof(Seleccionado));

                if (value != null)
                {
                    Id = value.Id;
                    Nombre = value.Nombre;
                    Email = value.Email;
                    Activo = value.Activo;
                }
            }
        }

        public ICommand CargarCommand { get; }
        public ICommand InsertarCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public SociosViewModel()
        {
            CargarCommand = new RelayCommand(Cargar);
            InsertarCommand = new RelayCommand(Insertar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            LimpiarCommand = new RelayCommand(Limpiar);

            Cargar();
        }

        private void Cargar()
        {
            try
            {
                Lista = new ObservableCollection<Socios>(_repo.ObtenerSocios());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando socios\n" + ex.Message);
                Lista = new ObservableCollection<Socios>();
            }
        }

        private void Insertar()
        {
            if (!ValidarSocio()) return;

            _repo.Insertar(new Socios
            {
                Nombre = Nombre.Trim(),
                Email = Email.Trim(),
                Activo = Activo
            });

            Cargar();
            Limpiar();
        }

        private void Editar()
        {
            if (Seleccionado == null)
            {
                MessageBox.Show("Selecciona un socio para editar");
                return;
            }

            if (!ValidarSocio()) return;

            _repo.Editar(new Socios
            {
                Id = Id,
                Nombre = Nombre.Trim(),
                Email = Email.Trim(),
                Activo = Activo
            });

            Cargar();
            Limpiar();
        }

        private void Eliminar()
        {
            if (Seleccionado == null)
            {
                MessageBox.Show("Selecciona un socio para eliminar");
                return;
            }

            _repo.Eliminar(Seleccionado.Id);
            Cargar();
            Limpiar();
        }

        private bool ValidarSocio()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("El email no puede estar vacío");
                return false;
            }

            try
            {
                var _ = new MailAddress(Email.Trim());
            }
            catch
            {
                MessageBox.Show("El email no tiene formato válido");
                return false;
            }

            return true;
        }

        private void Limpiar()
        {
            Seleccionado = null;
            Id = 0;
            Nombre = "";
            Email = "";
            Activo = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
