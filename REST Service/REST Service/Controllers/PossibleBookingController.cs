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
using System.Transactions;

namespace REST_Service.Controllers
{
    public class PossibleBookingController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private DataLayer.ManualBookingSystemDataContext _db;
        private int _numberOfErrors = 0;

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
        public HttpResponseMessage Put([FromUri]int id, [FromBody]Messages.PossibleBookingDelay delay)
        {
            var successResponse = "{\"Response\":\"Success\"}";
            var message = new HttpResponseMessage();
            var bookings = new Repositories.BookingRepository(_db).GetAll();
            Models.PossibleBooking posBook        = null;
            List<Models.ConcreteBooking> conBooks = null;
            Models.Booking bookPos                = null;
            List<Models.Booking> bookCons         = null;

            message = HttpResponse.Try<NullReferenceException>(
                action: () =>
                {
                    posBook = new Repositories
                        .PossibleBookingRepository(_db)
                        .GetById(id);
                    if (posBook == null) throw new NullReferenceException("delay.Id did not match any stored IDs");
                },
                success: successResponse,
                failure: "Kunne ikke finde det korrekte id");

            if (posBook != null) 
            {
                Debug.WriteLine("posBook is not null");
                bookPos = bookings.Single(book => book.Id == posBook.BookingId);

                message = HttpResponse.Try<Exception>(
                    action: () =>
                    {
                        conBooks = new Repositories
                            .ConcreteBookingRepository(_db)
                            .Where(conc => conc.PossibleBookingId == posBook.Id)
                            .ToList();
                        if (conBooks == null || conBooks.Count == 0) throw new Exception("posBook.Id did not match any elements in the list");
                    },
                    success: successResponse,
                    failure: "Kunne ikke finde konkrete bookinger");

                if (conBooks != null && conBooks.Count > 0)
                {
                    Debug.WriteLine("conBooks is not null, and contains " + conBooks.Count + " items");
                    bool access = true;
                    message = HttpResponse.Try<Exception>(
                        action: () =>
                        {
                            bookCons = new List<Models.Booking>();
                            conBooks.ForEach(e => bookCons.Add(bookings.Single(book => book.Id == e.BookingId)));
                            Debug.WriteLine("conBooks length: " + conBooks.Count);
                            Debug.WriteLine("bookCons length: " + bookCons.Count);
                            if (bookCons.Count > conBooks.Count)
                            {
                                access = false;
                                throw new Exception("You have more bookings representing concrete bookings, than concrete bookings");
                            }
                            else if (bookCons.Count < conBooks.Count)
                            {
                                access = false;
                                throw new Exception("You have less bookings representing concrete bookings, than concrete bookings");
                            }
                        },
                        success: successResponse,
                        failure: "kunne ikke finde bookinger der matcher booking ID'et");

                    if (access)
                    {
                        Debug.WriteLine("Access granted");
                        bookPos.StartTime = bookPos.StartTime.Add(delay.Duration);

                        foreach (var e in bookCons)
                        {
                            e.StartTime = e.StartTime.Add(delay.Duration);
                            Debug.WriteLine("Start time: " + e.StartTime.ToString());
                            e.EndTime = e.EndTime.Add(delay.Duration);
                            Debug.WriteLine("End time: " + e.EndTime.ToString());
                        }

                        _db.SafeSubmitChanges();
                        Debug.WriteLine("Changes submitted");
                    }
                    else Debug.WriteLine("Access denied");
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
}
