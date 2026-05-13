using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hotel_Pere_Maria.Models;

namespace Hotel_Pere_Maria.Services
{
    public static class InvoiceService
    {
        public static async Task<List<Invoice>> getAllIvoice()
        {
            try
            {
                var respuesta = await ApiService._httpClient.GetAsync(ApiService.BaseUrl + "invoice/all");
                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    List<Invoice> lista = JsonSerializer.Deserialize<List<Invoice>>(contenido, opciones);
                    return lista;
                }
                return new List<Invoice>();
            }
            catch
            {
                return null;
            }
        }

    }
}
