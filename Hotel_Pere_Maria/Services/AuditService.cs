using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hotel_Pere_Maria.Models;

namespace Hotel_Pere_Maria.Services
{
    public static class AuditService
    {
        public static async Task<List<BookingAuditLog>> getBookingAudit(string reservationId)
        {
            try
            {
                var respuesta = await ApiService._httpClient.GetAsync(ApiService.BaseUrl + $"bookings/{reservationId}/audit");
                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    List<BookingAuditLog> lista = JsonSerializer.Deserialize<List<BookingAuditLog>>(contenido, opciones);
                    return lista;
                }
                return new List<BookingAuditLog>();
            }
            catch
            {
                return null;
            }
        }

    }
}
