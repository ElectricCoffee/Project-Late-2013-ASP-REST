using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Diagnostics;

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

        public void InsertOnSubmit(Models.Booking booking)
        {
            throw (new MethodAccessException(
                "Attempt to create basic Bookings are discouraged," +
                "go through the specializations instead"));
        }

        public void DeleteOnSubmit(Models.Booking booking)
        {
            var concreteBooking = _concreteBookingTable.SingleOrDefault(s => s.Id == booking.Id);
            if (concreteBooking != null)
                _concreteBookingTable.DeleteOnSubmit(concreteBooking);

            var possibleBooking = _possibleBookingTable.SingleOrDefault(t => t.Id == booking.Id);
            if (possibleBooking != null)
                _possibleBookingTable.DeleteOnSubmit(possibleBooking);

            try
            {
                //_bookingTable.Attach(booking);
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
            }
            _bookingTable.DeleteOnSubmit(booking);
        }

        public IEnumerable<Models.Booking> Where(Func<Models.Booking, bool> predicate)
        {
            return _bookingTable.AsEnumerable().Where(predicate);
        }

        public IEnumerable<Models.Booking> GetAll()
        {
            return _bookingTable;
        }

        public Models.Booking Single(Func<Models.Booking, bool> predicate)
        {
            return _bookingTable.AsEnumerable().FirstOrDefault(predicate);
        }

        public Models.Booking GetById(int id)
        {
            return _bookingTable.Single(m => m.Id.Equals(id));
        }
    }
}