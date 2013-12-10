using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using REST_Service.Utils;
using Newtonsoft.Json.Converters;

namespace REST_Service.Controllers
{
    public class PossibleBookingController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private DataLayer.ManualBookingSystemDataContext _db;
        private int _numberOfErrors = 0;

        private Models.PossibleBookingMessage _bookingModel = null;

        /// <summary>
        /// Creates a new instance of the Data Context to use locally
        /// </summary>
        public PossibleBookingController()
        {
            _db = new DataLayer.ManualBookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// Uses a specified Data Context to use locally
        /// </summary>
        /// <param name="db"></param>
        public PossibleBookingController(DataLayer.ManualBookingSystemDataContext db)
        {
            _db = db;
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]PossibleBookingDelay delay)
        {
            var message = new HttpResponseMessage();
            var booking = new Repositories.BookingRepository(_db).GetAll();
            Models.PossibleBooking posBook        = null;
            List<Models.ConcreteBooking> conBooks = null;
            Models.Booking bookPos                = null;
            List<Models.Booking> bookCons         = null;

            message.Try<NullReferenceException>(
                action: () =>
                {
                    posBook = new Repositories
                        .PossibleBookingRepository(_db)
                        .GetById(delay.Id);
                    if (posBook == null) throw new NullReferenceException();
                },
                success: "{\"Response\":\"Success\"}",
                failure: "Kunne ikke finde det korrekte id");

            if (posBook != null) 
            {
                bookPos = booking.Single(book => book.Id == posBook.BookingId);

                message.Try<Exception>(
                    action: () =>
                    {
                        conBooks = new Repositories
                            .ConcreteBookingRepository(_db)
                            .Where(conc => conc.PossibleBookingId == posBook.Id)
                            .ToList();
                        if (conBooks == null) throw new NullReferenceException();
                    },
                    success: "{\"Response\":\"Success\"}",
                    failure: "Kunne ikke finde konkrete bookinger");

                if (conBooks != null)
                {
                    bookCons = new List<Models.Booking>();
                    conBooks.ForEach(e => bookCons.Add(booking.Single(book => book.Id == e.BookingId)));
                }
            }


            return message;
        }

        /// <summary>
        /// Posts the creation of the "Mulig Booking" to the server, with all related fields.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="inputSubject"></param>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Models.PossibleBooking possibleBooking)
        {
            var message = new HttpResponseMessage();
            // gets the subject from the database
            var subject = _db.GetTable<Models.Subject>().SingleOrDefault(s => s.Name == possibleBooking.Subject.Name);

            if (subject != null)
            {
                // write it to the database
                _db.GetTable<Models.PossibleBooking>().InsertOnSubmit(possibleBooking);

                // commit changes to the database
                _db.SafeSubmitChanges();
                message.OK("{\"Response\":\"OK\"}");
            }
            else message.Forbidden("Faget blev ikke fundet");

            return message;
        }

        /// <summary>
        /// Gets the start and end date as well as the subject from the databse and wraps it neatly into a JSON object
        /// If, however there are errors, it'll return a JSON object containing the error code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            var bookings = new Repositories.PossibleBookingRepository(_db).GetAll();

            response.OK(bookings.SerializeToJsonObject());

            return response;
        }
    }

    public class PossibleBookingDelay {
        public int Id {get;set;}
        public TimeSpan Duration {get;set;}
    }
}
