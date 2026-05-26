using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hotel_Pere_Maria.Models;
using Hotel_Pere_Maria.Services;

namespace Hotel_Pere_Maria.ViewModels
{
    public class AjustesCancelacionViewModel :BaseViewModel
    {
        private Hotelconfig _configuracionActual;
        private int _porcentajeMas7Dias = 100;
        public int PorcentajeMas7Dias
        {
            get => _porcentajeMas7Dias;
            set
            {
                if (value < 0 || value > 100)
                {
                    MessageBox.Show("El porcentaje debe estar entre 0 y 100.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (value <= _porcentajeMas3Dias)
                {
                    MessageBox.Show("El porcentaje de más de 7 días debe ser estrictamente MAYOR que el de más de 3 días.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _porcentajeMas7Dias = value;
                OnPropertyChanged(nameof(PorcentajeMas7Dias));
            }
        }

        private int _porcentajeMas3Dias = 50;
        public int PorcentajeMas3Dias
        {
            get => _porcentajeMas3Dias;
            set
            {
                if (value < 0 || value > 100)
                {
                    MessageBox.Show("El porcentaje debe estar entre 0 y 100.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (value >= _porcentajeMas7Dias)
                {
                    MessageBox.Show("El porcentaje de más de 3 días debe ser estrictamente MENOR que el de más de 7 días.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _porcentajeMas3Dias = value;
                OnPropertyChanged(nameof(PorcentajeMas3Dias));
            }
        }
        public ICommand GuardarAjustesCommand { get; }
        public ICommand CancelarAjustesCommand { get; }

        public AjustesCancelacionViewModel()
        {
            GuardarAjustesCommand = new RelayCommand(async () => await GuardarAjustesApi());
            CargarAjustesDesdeApi();
        }
        public void ExecuteCancelarAjustes() {
            CerrarVentana();
        }
        private void CerrarVentana()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
        private async void CargarAjustesDesdeApi()
        {
            Hotelconfig config = await HotelConfigService.getConfig();

            if (config != null)
            {
                _configuracionActual = config;

                _porcentajeMas7Dias = config.canMas7Dias;
                _porcentajeMas3Dias = config.canMas3Dias;

                OnPropertyChanged(nameof(PorcentajeMas7Dias));
                OnPropertyChanged(nameof(PorcentajeMas3Dias));
            }
            else
            {
                MessageBox.Show("Error crítico: No se pudo establecer conexión con el servicio de configuración del hotel.",
                                "Error de Red", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GuardarAjustesApi()
        {
            if (PorcentajeMas3Dias >= PorcentajeMas7Dias)
            {
                MessageBox.Show("Incoherencia en las políticas:\nEl porcentaje de devolución para más de 3 días debe ser estrictamente MENOR que el de más de 7 días.",
                                "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_configuracionActual == null)
            {
                _configuracionActual = new Hotelconfig();
            }

            _configuracionActual.canMas7Dias = PorcentajeMas7Dias;
            _configuracionActual.canMas3Dias = PorcentajeMas3Dias;

            bool exito = await HotelConfigService.ActualizarConfiguracion(_configuracionActual);

            if (exito)
            {
                MessageBox.Show("¡Ajustes de cancelación guardados con éxito!",
                                "Configuración Guardada", MessageBoxButton.OK, MessageBoxImage.Information);
                CerrarVentana();
            }

        }
    }
}
