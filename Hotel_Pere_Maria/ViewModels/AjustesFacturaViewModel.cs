using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hotel_Pere_Maria.Models;
using Hotel_Pere_Maria.Services;
using System.Linq;

namespace Hotel_Pere_Maria.ViewModels
{
    public class AjustesFacturaViewModel : BaseViewModel
    {
        private string _nombreHotel;
        public string NombreHotel
        {
            get => _nombreHotel;
            set { _nombreHotel = value; OnPropertyChanged(); }
        }

        private string _nif;
        public string Nif
        {
            get => _nif;
            set { _nif = value; OnPropertyChanged(); }
        }

        private string _direccion;
        public string Direccion
        {
            get => _direccion;
            set { _direccion = value; OnPropertyChanged(); }
        }

        private string _telefono;
        public string Telefono
        {
            get => _telefono;
            set {
                string soloNumeros = new string(value.Where(c => char.IsDigit(c)).ToArray());

                _telefono = soloNumeros;
                OnPropertyChanged();
            }
        }

        private string _codigoPostal;
        public string CodigoPostal
        {
            get => _codigoPostal;
            set {
                string soloNumeros = new string(value.Where(c => char.IsDigit(c)).ToArray());

                _codigoPostal = soloNumeros;
                OnPropertyChanged();
            }
        }

        private string _ciudad;
        public string Ciudad
        {
            get => _ciudad;
            set { _ciudad = value; OnPropertyChanged(); }
        }

        private string _provincia;
        public string Provincia
        {
            get => _provincia;
            set { _provincia = value; OnPropertyChanged(); }
        }
        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AjustesFacturaViewModel()
        {

            // Inicializar comandos
            GuardarCommand = new RelayCommand(async () => await ExecuteGuardar());
            CancelarCommand = new RelayCommand(ExecuteCancelar);

            // Cargar datos actuales al abrir la ventana
            _ = CargarDatos();
        }

        private async Task CargarDatos()
        {
            try
            {
                var config = await HotelConfigService.getConfig();
                if (config != null)
                {
                    NombreHotel = config.nombreHotel;
                    Nif = config.nif;
                    Direccion = config.direccion;
                    Telefono = config.telefono;
                    CodigoPostal = config.cp; // Mapeo de 'cp' a 'CodigoPostal'
                    Ciudad = config.ciudad;
                    Provincia = config.provincia;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar configuración: {ex.Message}");
            }
        }
        private async Task ExecuteGuardar()
        {
            // Validación simple
            if (string.IsNullOrWhiteSpace(NombreHotel) || string.IsNullOrWhiteSpace(Nif) || 
                string.IsNullOrWhiteSpace(Direccion) || string.IsNullOrWhiteSpace(Telefono) ||
                string.IsNullOrWhiteSpace(CodigoPostal) || string.IsNullOrWhiteSpace(Ciudad) ||
                string.IsNullOrWhiteSpace(Provincia))
            {
                MessageBox.Show("Todos los campos son obligatorios");
                return;
            }

            try
            {
                Hotelconfig nuevaConfig = new Hotelconfig();
                nuevaConfig.nombreHotel = NombreHotel;
                nuevaConfig.nif = Nif;
                nuevaConfig.direccion = Direccion;
                nuevaConfig.telefono = Telefono;
                nuevaConfig.cp = CodigoPostal;
                nuevaConfig.ciudad = Ciudad;
                nuevaConfig.provincia = Provincia;

                bool exito = await HotelConfigService.ActualizarConfiguracion(nuevaConfig);

                if (exito)
                {
                    MessageBox.Show("Configuración guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CerrarVentana();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        private void ExecuteCancelar()
        {
            CerrarVentana();
        }

        private void CerrarVentana()
        {
            // Busca la ventana actual y la cierra
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

    }

}
