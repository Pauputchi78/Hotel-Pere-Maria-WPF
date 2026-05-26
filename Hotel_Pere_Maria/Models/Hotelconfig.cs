using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Pere_Maria.Models
{
    public class Hotelconfig
    {
        public string nombreHotel { get; set; }
        public string nif { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string cp { get; set; }
        public string ciudad { get; set; }
        public string provincia { get; set; }
        public int canMas7Dias { get; set; }
        public int canMas3Dias { get; set; }
    }
}
