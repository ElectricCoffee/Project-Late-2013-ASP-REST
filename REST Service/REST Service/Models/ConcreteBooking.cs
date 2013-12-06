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

    /// <summary>
    /// Linq to Sql mapping for the table Konkret Booking
    /// </summary>
    [Table(Name = "[Konkret Booking]")]
    public class ConcreteBooking : IModel
    {
        private EntityRef<Student> _student;
        private EntityRef<PossibleBooking> _possibleBooking;
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
        /// Gets or sets the value of the column Type
        /// </summary>
        [Column(
            Name = "Type",
            DbType = "TINYINT NOT NULL",
            CanBeNull = false)]
        private byte TypeNum { get; set; }

        /// <summary>
        /// Gets or sets the associated value of the column Type
        /// </summary>
        public BookingType Type
        {
            get { return (BookingType) TypeNum; }
            set { TypeNum = (byte) value; }
        }

        /// <summary>
        /// Gets or sets the value of the column Kommentar
        /// </summary>
        [Column(
            Name = "Kommentar",
            DbType = "VARCHAR(150) NOT NULL",
            CanBeNull = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the value of the column Status ændret
        /// </summary>
        [Column(
            Name = "[Status ændret]",
            DbType = "TINYINT NOT NULL",
            CanBeNull = false)]
        private byte StatusChangedNum { get; set; }

        /// <summary>
        /// Gets or sets the associated value of the column Status ændret
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the column Studerende_id
        /// </summary>
        [Column(
            Name = "Studerende_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int StudentId { get; set; }

        [Association(
            Storage = "_student",
            IsForeignKey = true,
            Name = "[FK_Konkret Booking_Studerende]",
            ThisKey = "StudentId",
            OtherKey = "Id")]
        public Student Student
        {
            get { return _student.Entity; }
            set { _student.Entity = value; }
        }

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
                return Booking.EndTime;
            }
            set
            {
                EnsureBookingExists();
                Booking.EndTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the associated Subject entity of the disjointed table Booking
        /// </summary>
        public Subject Subject
        {
            get { return _booking.Entity.Subject; }
            set
            {
                EnsureBookingExists();
                Booking.Subject = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Mulig_booking_id
        /// </summary>
        [Column(
            Name = "Mulig_booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int PossibleBookingId { get; set; }

        /// <summary>
        /// Gets or sets the associated PossibleBooking entity
        /// </summary>
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

        /// <summary>
        /// Gets or sets the value of the column Booking_id
        /// </summary>
        [Column(
            Name = "Booking_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int BookingId { get; set; }

        /// <summary>
        /// Gets or sets the associated Booking entiy
        /// </summary>
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