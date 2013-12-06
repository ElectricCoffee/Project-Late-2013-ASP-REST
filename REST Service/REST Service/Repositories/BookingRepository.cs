using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace REST_Service.Repositories
{
	/// <summary>
	/// A specific implementation of IRepository specialized for Booking entites
	/// </summary>
	public class BookingRepository : SimpleRepository<Models.Booking>
	{
		private Table<Models.ConcreteBooking> _concreteBookings;
		private Table<Models.PossibleBooking> _possibleBookings;

		/// <summary>
		/// Creates a new BookingRepository instance
		/// </summary>
		/// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
		public BookingRepository(DataLayer.ManualBookingSystemDataContext dataContext)
			: base(dataContext)
		{
			_concreteBookings = _dataContext.ConcreteBookings;
			_possibleBookings = _dataContext.PossibleBookings;
		}
		
		/// <summary>
		/// Creating basic Booking entities are discouraged, go through the specializations instead,
		/// i.e. ConcreteBookingRepository or PossibleBookingRepository
		/// </summary>
		/// <param name="booking">The Booking enity to be inserted</param>
		public override void InsertOnSubmit(Models.Booking booking)
		{
			throw (new MethodAccessException(
				"Attempt to create basic Bookings are discouraged," +
				"go through the specializations instead"));
		}

		/// <summary>
		/// Enqueues a Booking entity to be deleted from the table on submit
		/// </summary>
		/// <remarks>
		/// Also checks that the Booking entity doesn't exist as a specialized entity,
		/// i.e. ConcreteBooking or PossibleBooking.
		/// If the Booking entity is found to be either of the specializations,
		/// the specialized entity is deleted before the Booking entity itself.
		/// </remarks>
		/// <param name="booking">The Booking entity to be deleted</param>
		public override void DeleteOnSubmit(Models.Booking booking)
		{
			var concreteBooking = _concreteBookings.SingleOrDefault(s => s.Id == booking.Id);
			if (concreteBooking != null)
				_concreteBookings.DeleteOnSubmit(concreteBooking);
			
			var possibleBooking = _possibleBookings.SingleOrDefault(t => t.Id == booking.Id);
			if (possibleBooking != null)
				_possibleBookings.DeleteOnSubmit(possibleBooking);

			base.DeleteOnSubmit(booking);
		}
	}
}