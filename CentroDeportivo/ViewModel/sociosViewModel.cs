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
    /// <summary>
    /// ViewModel para gestionar socios (listado y formulario).
    /// </summary>
    public class SociosViewModel : INotifyPropertyChanged
    {
        private readonly SociosRepositorio _repo = new SociosRepositorio();

        private ObservableCollection<Socios> _lista = new ObservableCollection<Socios>();

        /// <summary>
        /// Lista de socios mostrada en la vista.
        /// </summary>
        public ObservableCollection<Socios> Lista
        {
            get => _lista;
            set { _lista = value; OnPropertyChanged(nameof(Lista)); }
        }

        private int _id;
        private string _nombre;
        private string _email;
        private bool _activo = true;

        /// <summary>
        /// Id del socio (se usa al editar).
        /// </summary>
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        /// <summary>
        /// Nombre del socio en el formulario.
        /// </summary>
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        /// <summary>
        /// Email del socio en el formulario.
        /// </summary>
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        /// <summary>
        /// Indica si el socio está activo.
        /// </summary>
        public bool Activo
        {
            get => _activo;
            set { _activo = value; OnPropertyChanged(nameof(Activo)); }
        }

        private Socios _seleccionado;

        /// <summary>
        /// Socio seleccionado en la tabla; al cambiar, rellena el formulario.
        /// </summary>
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

        /// <summary>
        /// Recarga socios desde la base de datos.
        /// </summary>
        public ICommand CargarCommand { get; }

        /// <summary>
        /// Crea un socio nuevo con los datos del formulario.
        /// </summary>
        public ICommand InsertarCommand { get; }

        /// <summary>
        /// Guarda los cambios del socio seleccionado.
        /// </summary>
        public ICommand EditarCommand { get; }

        /// <summary>
        /// Elimina el socio seleccionado.
        /// </summary>
        public ICommand EliminarCommand { get; }

        /// <summary>
        /// Limpia el formulario y quita la selección.
        /// </summary>
        public ICommand LimpiarCommand { get; }

        /// <summary>
        /// Inicializa comandos y carga los datos iniciales.
        /// </summary>
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

        /// <summary>
        /// Se dispara cuando cambia una propiedad para refrescar la vista.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
