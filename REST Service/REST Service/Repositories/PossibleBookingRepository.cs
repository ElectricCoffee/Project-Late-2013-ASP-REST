using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository specialized for PossibleBooking entites
    /// </summary>
    public class PossibleBookingRepository : SimpleRepository<Models.PossibleBooking>
    {
        private Table<Models.Booking> _bookingTable;

        /// <summary>
        /// Creates a new PossibleBookingRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public PossibleBookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            : base(dataContext)
        {
            _bookingTable = _dataContext.Bookings;
        }

        /// <summary>
        /// Enqueues a PossibleBooking entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// If the associated Name entity was only used by the deleted PossibleBooking entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="possibleBooking">The PossibleBooking entity to be deleted</param>
        public override void DeleteOnSubmit(Models.PossibleBooking possibleBooking)
        {
            base.DeleteOnSubmit(possibleBooking);

            var booking = _bookingTable.SingleOrDefault(u => u.Id == possibleBooking.BookingId);
            _bookingTable.DeleteOnSubmit(booking);
        }
    }
}