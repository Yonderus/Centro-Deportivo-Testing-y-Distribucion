using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CentroDeportivo.ViewModel;

namespace CentroDeportivo.Views
{
    /// <summary>
    /// Lógica de interacción para sociosW.xaml
    /// </summary>
    public partial class sociosW : Window
    {
        public sociosW()
        {
            InitializeComponent();
            DataContext = new SociosViewModel();
        }
    }
}
