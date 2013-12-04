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
        private const string DATE_FORMAT = "yyyy-MM-dd-HH-mm-ss";
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private BookingSystemDataContext _db;
        private int _numberOfErrors = 0;

        private Models.PossibleBookingMessage _bookingModel = null;

        /// <summary>
        /// Creates a new instance of the Data Context to use locally
        /// </summary>
        public PossibleBookingController()
        {
            _db = new BookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// Uses a specified Data Context to use locally
        /// </summary>
        /// <param name="db"></param>
        public PossibleBookingController(BookingSystemDataContext db)
        {
            _db = db;
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
            
            // Takes care of the try-catch boilerplate by wrapping it in an action
            Action<BookingSystemDataContext> submitChanges = db =>
            {
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("DEBUG: " + e.Message);
                }
            };
            var message = new HttpResponseMessage();
            // gets the subject from the database
            var subject = _db.GetTable<Models.Subject>().SingleOrDefault(s => s.Name == possibleBooking.Subject.Name);

            if (subject != null)
            {
                // write it to the database
                _db.GetTable<Models.PossibleBooking>().InsertOnSubmit(possibleBooking);

                // commit changes to the database
                submitChanges(_db);
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
            var bookings = _db.GetTable<Models.PossibleBooking>().AsEnumerable();

            response.OK(bookings.SerializeToJsonObject());

            return response;
        }
    }
}
