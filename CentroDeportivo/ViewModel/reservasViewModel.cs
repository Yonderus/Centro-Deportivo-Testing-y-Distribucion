using CentroDeportivo;
using CentroDeportivo.Model.Repositorios;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CentroDeportivo.ViewModel
{
    /// <summary>
    /// ViewModel para gestionar reservas (selecciones de socio/actividad y fecha).
    /// </summary>
    public class ReservasViewModel : INotifyPropertyChanged
    {
        private readonly ReservasRepositorio _repoReservas = new ReservasRepositorio();
        private readonly SociosRepositorio _repoSocios = new SociosRepositorio();
        private readonly ActividadesRepositorio _repoActividades = new ActividadesRepositorio();

        private ObservableCollection<Reservas> _reservas = new ObservableCollection<Reservas>();

        /// <summary>
        /// Lista de reservas mostrada en la vista.
        /// </summary>
        public ObservableCollection<Reservas> Reservas
        {
            get => _reservas;
            set { _reservas = value; OnPropertyChanged(nameof(Reservas)); }
        }

        /// <summary>
        /// Lista de socios (normalmente solo activos) para el desplegable.
        /// </summary>
        public ObservableCollection<Socios> Socios { get; private set; } = new ObservableCollection<Socios>();

        /// <summary>
        /// Lista de actividades para el desplegable.
        /// </summary>
        public ObservableCollection<Actividades> Actividades { get; private set; } = new ObservableCollection<Actividades>();

        private int _id;

        /// <summary>
        /// Id de la reserva (se usa al editar).
        /// </summary>
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        private Socios _socioSeleccionado;

        /// <summary>
        /// Socio elegido para la reserva.
        /// </summary>
        public Socios SocioSeleccionado
        {
            get => _socioSeleccionado;
            set { _socioSeleccionado = value; OnPropertyChanged(nameof(SocioSeleccionado)); }
        }

        private Actividades _actividadSeleccionada;

        /// <summary>
        /// Actividad elegida para la reserva.
        /// </summary>
        public Actividades ActividadSeleccionada
        {
            get => _actividadSeleccionada;
            set { _actividadSeleccionada = value; OnPropertyChanged(nameof(ActividadSeleccionada)); }
        }

        private DateTime _fecha = DateTime.Today;

        /// <summary>
        /// Fecha de la reserva.
        /// </summary>
        public DateTime Fecha
        {
            get => _fecha;
            set { _fecha = value; OnPropertyChanged(nameof(Fecha)); }
        }

        private Reservas _seleccionada;

        /// <summary>
        /// Reserva seleccionada en la tabla; al cambiar, rellena el formulario.
        /// </summary>
        public Reservas Seleccionada
        {
            get => _seleccionada;
            set
            {
                _seleccionada = value;
                OnPropertyChanged(nameof(Seleccionada));

                if (value != null)
                {
                    Id = value.Id;
                    Fecha = value.Fecha.Date;
                    SocioSeleccionado = Socios.FirstOrDefault(s => s.Id == value.SocioId);
                    ActividadSeleccionada = Actividades.FirstOrDefault(a => a.Id == value.ActividadId);
                }
            }
        }

        /// <summary>
        /// Recarga socios, actividades y reservas.
        /// </summary>
        public ICommand CargarCommand { get; }

        /// <summary>
        /// Crea una reserva nueva con los datos del formulario.
        /// </summary>
        public ICommand InsertarCommand { get; }

        /// <summary>
        /// Guarda los cambios de la reserva seleccionada.
        /// </summary>
        public ICommand EditarCommand { get; }

        /// <summary>
        /// Elimina la reserva seleccionada.
        /// </summary>
        public ICommand EliminarCommand { get; }

        /// <summary>
        /// Limpia el formulario y quita la selección.
        /// </summary>
        public ICommand LimpiarCommand { get; }

        /// <summary>
        /// Inicializa comandos y carga los datos iniciales.
        /// </summary>
        public ReservasViewModel()
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
                Socios = new ObservableCollection<Socios>(_repoSocios.ObtenerSocios().Where(s => s.Activo));
                OnPropertyChanged(nameof(Socios));

                Actividades = new ObservableCollection<Actividades>(_repoActividades.ObtenerActividades());
                OnPropertyChanged(nameof(Actividades));

                Reservas = new ObservableCollection<Reservas>(_repoReservas.ObtenerReservas());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando datos\n" + ex.Message);
            }
        }

        private void Insertar()
        {
            if (!ValidarReserva()) return;

            _repoReservas.Insertar(new Reservas
            {
                SocioId = SocioSeleccionado.Id,
                ActividadId = ActividadSeleccionada.Id,
                Fecha = Fecha.Date
            });

            Cargar();
            Limpiar();
        }

        private void Editar()
        {
            if (Seleccionada == null)
            {
                MessageBox.Show("Selecciona una reserva para editar");
                return;
            }

            if (!ValidarReserva()) return;

            _repoReservas.Editar(new Reservas
            {
                Id = Id,
                SocioId = SocioSeleccionado.Id,
                ActividadId = ActividadSeleccionada.Id,
                Fecha = Fecha.Date
            });

            Cargar();
            Limpiar();
        }

        private void Eliminar()
        {
            if (Seleccionada == null)
            {
                MessageBox.Show("Selecciona una reserva para eliminar");
                return;
            }

            _repoReservas.Eliminar(Seleccionada.Id);
            Cargar();
            Limpiar();
        }

        private bool ValidarReserva()
        {
            if (SocioSeleccionado == null)
            {
                MessageBox.Show("Debes seleccionar un socio");
                return false;
            }

            if (ActividadSeleccionada == null)
            {
                MessageBox.Show("Debes seleccionar una actividad");
                return false;
            }

            if (Fecha.Date < DateTime.Today)
            {
                MessageBox.Show("La fecha no puede ser anterior a hoy");
                return false;
            }

            int reservasDia = Reservas.Count(r =>
                r.ActividadId == ActividadSeleccionada.Id &&
                r.Fecha.Date == Fecha.Date &&
                (Seleccionada == null || r.Id != Seleccionada.Id)
            );

            if (reservasDia >= ActividadSeleccionada.AforoMaximo)
            {
                MessageBox.Show("No se puede reservar, aforo máximo superado");
                return false;
            }

            return true;
        }

        private void Limpiar()
        {
            Seleccionada = null;
            Id = 0;
            SocioSeleccionado = null;
            ActividadSeleccionada = null;
            Fecha = DateTime.Today;
        }

        /// <summary>
        /// Se dispara cuando cambia una propiedad para refrescar la vista.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
