using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class PossibleBookingRepository : IRepository<Models.PossibleBooking>
    {
        private DataLayer.ManualBookingSystemDataContext _dataContext;
        private Table<Models.Booking> _bookingTable;
        private Table<Models.PossibleBooking> _possibleBookingTable;

        public PossibleBookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _bookingTable = dataContext.Bookings;
            _possibleBookingTable = dataContext.PossibleBookings;
        }

        public void InsertOnSubmit(Models.PossibleBooking possibleBooking)
        {
            _possibleBookingTable.InsertOnSubmit(possibleBooking);
        }

        public void DeleteOnSubmit(Models.PossibleBooking possibleBooking)
        {
            _possibleBookingTable.Attach(possibleBooking);
            _possibleBookingTable.DeleteOnSubmit(possibleBooking);

            var booking = _bookingTable.SingleOrDefault(u => u.Id == possibleBooking.BookingId);

            _bookingTable.Attach(booking);
            _bookingTable.DeleteOnSubmit(booking);
        }

        public IQueryable<Models.PossibleBooking> SearchFor(Expression<Func<Models.PossibleBooking, bool>> predicate)
        {
            return _possibleBookingTable.Where(predicate);
        }

        public IQueryable<Models.PossibleBooking> GetAll()
        {
            return _possibleBookingTable;
        }

        public Models.PossibleBooking GetById(int id)
        {
            return _possibleBookingTable.Single(m => m.Id.Equals(id));
        }
    }
}