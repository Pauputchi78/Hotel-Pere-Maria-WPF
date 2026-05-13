using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Pere_Maria.Models
{
    public class Invoice
    {
        public string reservation_id { get; set; }
        public string room_id { get; set; }
        public string user_id { get; set; }
        private DateTime _check_in { get; set; }
        private DateTime _check_out { get; set; }
        public double price { get; set; }

        //La fecha de cancelación puede ser nula 
        private DateTime? _cancelation_date { get; set; }

        private DateTime _invoice_date { get; set; }
        public string invoice_number { get; set; }
        public string user_name { get; set; }
        public string user_surname { get; set; }
        public string user_dni { get; set; }
        public string user_city { get; set; }

        //Al obtener una fecha la convertimos a la hora del equipo local ya que la base de datos la guarda en formato universal
        public DateTime check_in
        {
            get => _check_in.ToLocalTime();
            set => _check_in = value;
        }

        public DateTime check_out
        {
            get => _check_out.ToLocalTime();
            set => _check_out = value;
        }

        public DateTime? cancelation_date
        {
            get => _cancelation_date?.ToLocalTime();
            set => _cancelation_date = value;
        }
        public DateTime invoice_date { 
            get => _invoice_date.ToLocalTime();
            set => _invoice_date = value;
        }
    }
}
