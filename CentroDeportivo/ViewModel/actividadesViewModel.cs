using CentroDeportivo;
using CentroDeportivo.Model.Repositorios;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CentroDeportivo.ViewModel
{
    /// <summary>
    /// ViewModel para gestionar el listado de actividades y el formulario de edición.
    /// </summary>
    public class ActividadesViewModel : INotifyPropertyChanged
    {
        private readonly ActividadesRepositorio _repo = new ActividadesRepositorio();

        private ObservableCollection<Actividades> _lista = new ObservableCollection<Actividades>();

        /// <summary>
        /// Lista mostrada en la vista.
        /// </summary>
        public ObservableCollection<Actividades> Lista
        {
            get => _lista;
            set { _lista = value; OnPropertyChanged(nameof(Lista)); }
        }

        private int _id;
        private string _nombre;
        private int _aforoMaximo = 10;

        /// <summary>
        /// Id de la actividad (se usa al editar).
        /// </summary>
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        /// <summary>
        /// Nombre de la actividad en el formulario.
        /// </summary>
        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        /// <summary>
        /// Aforo máximo de la actividad en el formulario.
        /// </summary>
        public int AforoMaximo
        {
            get => _aforoMaximo;
            set { _aforoMaximo = value; OnPropertyChanged(nameof(AforoMaximo)); }
        }

        private Actividades _seleccionada;

        /// <summary>
        /// Actividad seleccionada en la tabla; al cambiar, rellena el formulario.
        /// </summary>
        public Actividades Seleccionada
        {
            get => _seleccionada;
            set
            {
                _seleccionada = value;
                OnPropertyChanged(nameof(Seleccionada));

                if (value != null)
                {
                    Id = value.Id;
                    Nombre = value.Nombre;
                    AforoMaximo = value.AforoMaximo;
                }
            }
        }

        /// <summary>
        /// Carga/reCarga la lista desde la base de datos.
        /// </summary>
        public ICommand CargarCommand { get; }

        /// <summary>
        /// Crea una actividad nueva con los datos del formulario.
        /// </summary>
        public ICommand InsertarCommand { get; }

        /// <summary>
        /// Guarda los cambios de la actividad seleccionada.
        /// </summary>
        public ICommand EditarCommand { get; }

        /// <summary>
        /// Elimina la actividad seleccionada.
        /// </summary>
        public ICommand EliminarCommand { get; }

        /// <summary>
        /// Limpia el formulario y quita la selección.
        /// </summary>
        public ICommand LimpiarCommand { get; }

        /// <summary>
        /// Inicializa los comandos y carga los datos iniciales.
        /// </summary>
        public ActividadesViewModel()
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
                Lista = new ObservableCollection<Actividades>(_repo.ObtenerActividades());
            }
            catch (Exception)
            {
                MessageBox.Show("Error cargando actividades");
                Lista = new ObservableCollection<Actividades>();
            }
        }

        private void Insertar()
        {
            if (!ValidarActividad()) return;

            _repo.Insertar(new Actividades
            {
                Nombre = Nombre.Trim(),
                AforoMaximo = AforoMaximo
            });

            Cargar();
            Limpiar();
        }

        private void Editar()
        {
            if (Seleccionada == null)
            {
                MessageBox.Show("Selecciona una actividad para editar");
                return;
            }

            if (!ValidarActividad()) return;

            _repo.Editar(new Actividades
            {
                Id = Id,
                Nombre = Nombre.Trim(),
                AforoMaximo = AforoMaximo
            });

            Cargar();
            Limpiar();
        }

        private void Eliminar()
        {
            if (Seleccionada == null)
            {
                MessageBox.Show("Selecciona una actividad para eliminar");
                return;
            }

            _repo.Eliminar(Seleccionada.Id);
            Cargar();
            Limpiar();
        }

        private bool ValidarActividad()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío");
                return false;
            }

            if (AforoMaximo <= 0)
            {
                MessageBox.Show("El aforo máximo debe ser mayor que 0");
                return false;
            }

            return true;
        }

        private void Limpiar()
        {
            Seleccionada = null;
            Id = 0;
            Nombre = "";
            AforoMaximo = 10;
        }

        /// <summary>
        /// Se dispara cuando cambia una propiedad para refrescar la vista.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
