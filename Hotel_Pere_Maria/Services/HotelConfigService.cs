using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Hotel_Pere_Maria.Models;

namespace Hotel_Pere_Maria.Services
{
    public static class HotelConfigService
    {
        public static async Task<Hotelconfig> getConfig()
        {
            try
            {
                var respuesta = await ApiService._httpClient.GetAsync(ApiService.BaseUrl + "config/getconfig");
                if (respuesta.IsSuccessStatusCode)
                {
                    string contenido = await respuesta.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Hotelconfig config = JsonSerializer.Deserialize<Hotelconfig>(contenido, opciones);
                    return config;
                }
                return new Hotelconfig();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> ActualizarConfiguracion(Hotelconfig config)
        {
            try
            {
                // Serializamos el objeto. PropertyNamingPolicy = null asegura 
                // que se envíen los nombres exactos de la clase (nombreHotel, nif, etc.)
                var opciones = new JsonSerializerOptions { PropertyNamingPolicy = null };
                string json = JsonSerializer.Serialize(config, opciones);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviamos la petición PUT
                var respuesta = await ApiService._httpClient.PutAsync(ApiService.BaseUrl + "config/update",content);

                if (!respuesta.IsSuccessStatusCode) {
                    string errorJson = await respuesta.Content.ReadAsStringAsync();

                    // Mostrar el mensaje real que definiste en JS (res.status(500).json({ error: ... }))
                    MessageBox.Show($"Error del servidor: {respuesta.StatusCode}\nDetalle: {errorJson}",
                                    "Error al Guardar", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return respuesta.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
    
}
