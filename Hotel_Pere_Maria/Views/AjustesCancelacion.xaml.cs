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
using Hotel_Pere_Maria.ViewModels;

namespace Hotel_Pere_Maria.Views
{
    /// <summary>
    /// Lógica de interacción para AjustesCancelacion.xaml
    /// </summary>
    public partial class AjustesCancelacion : Window
    {
        public AjustesCancelacion()
        {
            InitializeComponent();
            this.DataContext = new AjustesCancelacionViewModel();
        }
    }
}
