﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.MuligBooking")]
    public class PossibleBooking
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

        public EntityRef<Subject> Subject
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
            IsForeignKey = true,
            Name = "FK_Mulig Booking_Booking",
            ThisKey = "BookingId")]
        private Booking Booking
        {
            get { return _booking.Entity; }
            set { _booking.Entity = value; }
        }

        private void EnsureBookingExists()
        {
            if (_booking.Equals(null))
                _booking = new EntityRef<Booking>(new Booking());
        }
    }
}