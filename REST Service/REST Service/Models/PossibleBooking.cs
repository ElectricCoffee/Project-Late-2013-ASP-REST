using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Mulig Booking
    /// </summary>
    [Table(Name = "[Mulig Booking]")]
    public class PossibleBooking : IModel
    {
        private EntityRef<Booking> _booking;

        /// <summary>
        /// Gets the value of the column _id
        /// </summary>
        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the value of the column Varighed
        /// </summary>
        [Column(
            Name = "Varighed",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the value of the column StartTid of the disjointed table Booking
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the column SlutTid of the disjointed table Booking
        /// </summary>
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

        /// <summary>
        /// Gets or sets the associated Subject entity of the disjointed table Booking
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the column Bruger_id
        /// </summary>
        [Column(
            Name = "Booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int BookingId { get; set; }

        /// <summary>
        /// Gets or sets the associated Booking entity
        /// </summary>
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

        /// <summary>
        /// Makes sure that an instance of Booking Exists
        /// </summary>
        private void EnsureBookingExists()
        {
            if (Booking == null)
                Booking = new Booking();
        }
    }
}