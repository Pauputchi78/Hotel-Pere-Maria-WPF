using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Pere_Maria.Models
{
    public class BookingAuditLog
    {
        public string id { get; set; }
        public string reservation_id { get; set; }
        public string action { get; set; }
        public string user_id { get; set; }
        public string role { get; set; }
        public Reservation previous_state { get; set; }
        public Reservation new_state { get; set; }

        public DateTime _timestamp;

        public DateTime timestamp
        {
            get => _timestamp.ToLocalTime();
            set => _timestamp = value;
        }
    }
}
