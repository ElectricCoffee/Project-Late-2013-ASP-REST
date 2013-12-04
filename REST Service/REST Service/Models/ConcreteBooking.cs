using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public enum BookingType {
        Pending = 0,
        Approved = 1,
        Finished = 2
    }

    [Table(Name = "[Konkret Booking]")]
    public class ConcreteBooking : IModel
    {
        private EntityRef<PossibleBooking> _possibleBooking;
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
            Name = "Type",
            DbType = "TINYINT NOT NULL",
            CanBeNull = false)]
        private byte TypeNum { get; set; }

        public BookingType Type
        {
            get
            {
                return (BookingType) TypeNum;
            }
            set
            {
                TypeNum = (byte) value;
            }
        }

        [Column(
            Name = "Kommentar",
            DbType = "VARCHAR(150) NOT NULL",
            CanBeNull = false)]
        public string Comment { get; set; }

        [Column(
            Name = "[Status ændret]",
            DbType = "TINYINT NOT NULL",
            CanBeNull = false)]
        private byte StatusChangedNum { get; set; }

        public bool StatusChanged
        {
            get
            {
                if (StatusChangedNum == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    StatusChangedNum = 1;
                else
                    StatusChangedNum = 0;
            }
        }

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
                return Booking.EndTime;
            }
            set
            {
                EnsureBookingExists();
                Booking.EndTime = value;
            }
        }

        public Subject Subject
        {
            get { return _booking.Entity.Subject; }
            set
            {
                EnsureBookingExists();
                Booking.Subject = value;
            }
        }

        [Column(
            Name = "Mulig_booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int PossibleBookingId { get; set; }

        #region this part annoys me
        [Association(
            Storage = "_possibleBooking",
            IsForeignKey = true,
            Name = "[FK_Konkret Booking_Mulig Booking]",
            ThisKey = "PossibleBookingId",
            OtherKey = "Id")]
        public PossibleBooking PossibleBooking
        {
            get { return _possibleBooking.Entity; }
            set { _possibleBooking.Entity = value; }
        }
        #endregion

        [Column(
            Name = "Booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int BookingId { get; set; }

        [Association(
            Storage = "_booking",
            IsForeignKey = true,
            Name = "[FK_Konkret Booking_Booking]",
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