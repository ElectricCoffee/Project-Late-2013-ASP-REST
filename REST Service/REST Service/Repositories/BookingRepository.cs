using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Linq;
using System.Web;

namespace REST_Service.Repositories
{
    public class BookingRepository : IRepository<Models.Booking>
    {
        private DataLayer.ManualBookingSystemDataContext _dataContext;
        private Table<Models.Booking> _bookingTable;
        private Table<Models.ConcreteBooking> _concreteBookingTable;
        private Table<Models.PossibleBooking> _possibleBookingTable;


        public BookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _bookingTable = dataContext.Bookings;
            _concreteBookingTable = dataContext.ConcreteBookings;
            _possibleBookingTable = dataContext.PossibleBookings;
        }

        public void InsertOnSubmit(Models.Booking bookings)
        {
            throw (new MethodAccessException(
                "Attempt to create basic Bookings are discouraged," +
                "go through the specializations instead"));
        }

        public void DeleteOnSubmit(Models.Booking bookings)
        {
            var concreteBooking = _concreteBookingTable.SingleOrDefault(s => s.Id == bookings.Id);
            if (concreteBooking != null)
                _concreteBookingTable.DeleteOnSubmit(concreteBooking);

            var possibleBooking = _possibleBookingTable.SingleOrDefault(t => t.Id == bookings.Id);
            if (possibleBooking != null)
                _possibleBookingTable.DeleteOnSubmit(possibleBooking);

            _bookingTable.Attach(bookings);
            _bookingTable.DeleteOnSubmit(bookings);
        }
        public IQueryable<Models.Booking> SearchFor(Expression<Func<Models.Booking, bool>> predicate)
        {
            return _bookingTable.Where(predicate);
        }

        public IQueryable<Models.Booking> GetAll()
        {
            return _bookingTable;
        }

        public Models.Booking GetById(int id)
        {
            return _bookingTable.Single(m => m.Id.Equals(id));
        }

    }
}