using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository specialized for ConcreteBooking entites
    /// </summary>
    public class ConcreteBookingRepository : SimpleRepository<Models.ConcreteBooking>
    {
        private Table<Models.Booking> _bookings;

        /// <summary>
        /// Creates a new ConcreteBookingRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public ConcreteBookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            : base(dataContext)
        {
            _bookings = _dataContext.Bookings;
        }

        /// <summary>
        /// Enqueues a ConcreteBooking entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// If the associated Name entity was only used by the deleted ConcreteBooking entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="concreteBooking">The ConcreteBooking entity to be deleted</param>
        public override void DeleteOnSubmit(Models.ConcreteBooking concreteBooking)
        {
            base.DeleteOnSubmit(concreteBooking);

            var booking = _bookings.SingleOrDefault(b => b.Id == concreteBooking.BookingId);
            _bookings.DeleteOnSubmit(booking);
        }
    }
}