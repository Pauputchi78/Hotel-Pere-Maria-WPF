using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel_Pere_Maria.Models;
using Hotel_Pere_Maria.Services;
using Hotel_Pere_Maria.Views;
using System.Windows.Input;
using System.Windows;

namespace Hotel_Pere_Maria.ViewModels
{
    public class ListaFacturasViewModel : BaseViewModel
    {
        private List<Invoice> _todasLasFacturas;
        private List<Invoice> _facturasFiltradas;

        // Comandos
        public ICommand LimpiarFiltrosCommand { get; }
        public ICommand DescargarFacturaCommand { get; }
        public ICommand SeleccionarClienteCommand { get; }
        public ICommand SeleccionarRoomCommand { get; }

        // Propiedades de Filtro
        private string _fnfactura;
        private string _rId;
        private string _fUser;
        private string _fRoom;
        private DateTime? _fechaFDesde;
        private DateTime? _fechaFHasta;
        private double _pMin = 0;
        private double _pMax = 10000;

        // Getters y Setters con OnPropertyChanged y llamada a Filtrar
        public string FiltroNFactura { get => _fnfactura; set { _fnfactura = value; OnPropertyChanged(); Filtrar(); } }
        public string FiltroId { get => _rId; set { _rId = value; OnPropertyChanged(); Filtrar(); } }
        public string FiltroUser { get => _fUser; set { _fUser = value; OnPropertyChanged(); Filtrar(); } }
        public string FiltroRoom { get => _fRoom; set { _fRoom = value; OnPropertyChanged(); Filtrar(); } }
        public DateTime? FechaFDesde { get => _fechaFDesde; set { _fechaFDesde = value; OnPropertyChanged(); Filtrar(); } }
        public DateTime? FechaFHasta { get => _fechaFHasta; set { _fechaFHasta = value; OnPropertyChanged(); Filtrar(); } }
        public double PrecioMin { get => _pMin; set { _pMin = value; OnPropertyChanged(); Filtrar(); } }
        public double PrecioMax { get => _pMax; set { _pMax = value; OnPropertyChanged(); Filtrar(); } }

        public List<Invoice> FacturasFiltradas
        {
            get => _facturasFiltradas;
            set { _facturasFiltradas = value; OnPropertyChanged(); }
        }

        private void Filtrar()
        {
            if (_todasLasFacturas == null) return;

            DateTime hoy = DateTime.Now;

            FacturasFiltradas = _todasLasFacturas.Where(r =>
            {
                bool cnfactura = string.IsNullOrEmpty(FiltroNFactura) || r.invoice_number.ToString().ToLower().Contains(FiltroNFactura.ToLower().Trim());
                bool cId = string.IsNullOrEmpty(FiltroId) || r.reservation_id.ToString().ToLower().Contains(FiltroId.ToLower().Trim());
                bool cUser = string.IsNullOrEmpty(FiltroUser) || r.user_id.ToLower().Contains(FiltroUser.ToLower().Trim());
                bool cRoom = string.IsNullOrEmpty(FiltroRoom) || r.room_id.ToLower().Contains(FiltroRoom.ToLower().Trim());
                bool cPrecio = r.price >= PrecioMin && r.price <= PrecioMax;
                bool cDesde = !FechaFDesde.HasValue || r.invoice_date.Date >= FechaFDesde.Value.Date;
                bool cHasta = !FechaFHasta.HasValue || r.invoice_date.Date <= FechaFHasta.Value.Date;

                return cnfactura && cId && cUser && cRoom && cPrecio && cDesde && cHasta;
            }).ToList();
        }

        private void ExecuteLimpiarFiltros()
        {
            _fnfactura = "";
            _rId = _fUser = _fRoom = "";
            _fechaFDesde = _fechaFHasta = null;
            _pMin = 0; _pMax = 10000;

            OnPropertyChanged(string.Empty); // Notifica todas las propiedades
            Filtrar();
        }


        public ListaFacturasViewModel() {

            LimpiarFiltrosCommand = new RelayCommand(ExecuteLimpiarFiltros);
            DescargarFacturaCommand = new RelayCommand<Invoice>(async (r) => await ExecuteDescargarFactura(r));
            SeleccionarClienteCommand = new RelayCommand(ExecuteSeleccionarCliente);
            SeleccionarRoomCommand = new RelayCommand(ExecuteSeleccionarRoom);
            _ = CargarFacturas();
        }

        private async Task CargarFacturas()
        {
            try
            {
                _todasLasFacturas = await InvoiceService.getAllIvoice();
                Filtrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task ExecuteDescargarFactura(Invoice res)
        {
            if (res == null) return;

            MessageBox.Show("Descargando facura");
        }

        private void ExecuteSeleccionarCliente()
        {
            GestionUsuarios selector = new GestionUsuarios();
            selector.Owner = System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (selector.ShowDialog() == true && selector.UsuarioSeleccionado != null)
            {
                FiltroUser = selector.UsuarioSeleccionado.user_id;
            }
        }

        private void ExecuteSeleccionarRoom()
        {
            var win = new listRoom(null, null);

            bool? result = win.ShowDialog();

            if (result == true && win.SelectedRoomResult != null)
            {
                FiltroRoom = win.SelectedRoomResult.RoomId;
            }
        }


    }
}
