using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public class PossibleBookingMessages
    {
        public List<PossibleBookingMessage> Messages { get; set; }

        public PossibleBookingMessages(System.Data.Linq.Table<REST_Service.Booking> bookings, System.Data.Linq.Table<REST_Service.Mulig_Booking> possibleBookings)
        {
            Messages = new List<PossibleBookingMessage>();
            var bs = bookings.ToList();
            var pbs = possibleBookings.ToList();

            for (int i = 0; i < possibleBookings.Count(); i++)
            {
                Messages.Add(new PossibleBookingMessage(bs[i], pbs[i]));
            }
        }
    }
    public class PossibleBookingMessage
    {
        public PossibleBookingMessage(REST_Service.Booking booking, Mulig_Booking possible)
        {
            if (possible.Booking_id == booking._id)
            {
                Id = possible._id;
                BookingId = possible.Booking_id;
                Subject = new InnerSubject { Id = booking.Fag._id, Name = booking.Fag.Navn };
                StartTime = booking.StartTid;
                EndTime = booking.SlutTid;
            }
        }

        public int Id { get; set; }
        public int BookingId { get; set; }
        public InnerSubject Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public class InnerSubject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}