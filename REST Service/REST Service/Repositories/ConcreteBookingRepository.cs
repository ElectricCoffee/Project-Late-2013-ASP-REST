﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class ConcreteBookingRepository : IRepository<Models.ConcreteBooking>
    {
        private DataLayer.ManualBookingSystemDataContext _dataContext;
        private Table<Models.Booking> _bookingTable;
        private Table<Models.ConcreteBooking> _concreteBookingTable;

        public ConcreteBookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _bookingTable = dataContext.Bookings;
            _concreteBookingTable = dataContext.ConcreteBookings;
        }

        public void InsertOnSubmit(Models.ConcreteBooking concreteBooking)
        {
            _concreteBookingTable.InsertOnSubmit(concreteBooking);
        }

        public void DeleteOnSubmit(Models.ConcreteBooking concreteBooking)
        {
            _concreteBookingTable.Attach(concreteBooking);
            _concreteBookingTable.DeleteOnSubmit(concreteBooking);

            var booking = _bookingTable.SingleOrDefault(u => u.Id == concreteBooking.BookingId);

            _bookingTable.Attach(booking);
            _bookingTable.DeleteOnSubmit(booking);
        }

        public IQueryable<Models.ConcreteBooking> SearchFor(Expression<Func<Models.ConcreteBooking, bool>> predicate)
        {
            return _concreteBookingTable.Where(predicate);
        }

        public IQueryable<Models.ConcreteBooking> GetAll()
        {
            return _concreteBookingTable;
        }

        public Models.ConcreteBooking GetById(int id)
        {
            return _concreteBookingTable.Single(m => m.Id.Equals(id));
        }
    }
}