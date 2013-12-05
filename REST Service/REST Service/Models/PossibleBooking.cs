using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace REST_Service.Models
{
    [Table(Name = "[Mulig Booking]")]
    public class PossibleBooking : IModel
    {
        private EntityRef<Booking> _booking;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "Varighed",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int Duration { get; set; }

        public DateTime StartTime
        {
            get
            {
                EnsureBookingExists();
                return _booking.Entity.StartTime;
            }
            set
            {
                EnsureBookingExists();
                _booking.Entity.StartTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                EnsureBookingExists();
                return _booking.Entity.EndTime;
            }
            set
            {
                EnsureBookingExists();
                _booking.Entity.EndTime = value;
            }
        }

        public Subject Subject
        {
            get
            {
                EnsureBookingExists();
                return _booking.Entity.Subject;
            }
            set
            {
                EnsureBookingExists();
                _booking.Entity.Subject = value;
            }
        }

        [Column(
            Name = "Booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int BookingId { get; set; }

        [Association(
            Storage = "_booking",
            IsForeignKey = true,
            Name = "[FK_Mulig Booking_Booking]",
            ThisKey = "BookingId",
            OtherKey = "Id")]
        private Booking Booking
        {
            get { return _booking.Entity; }
            set { _booking.Entity = value; }
        }

        private void EnsureBookingExists()
        {
            if (Booking == null)
                Booking = new Booking();
        }
    }
}