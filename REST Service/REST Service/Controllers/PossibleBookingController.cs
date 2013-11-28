using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using REST_Service.Utils;

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

        public KeyValuePair<string, KeyValuePair<string, string>> Response(string pSubject, string pStart, string pEnd){
            return new KeyValuePair<string, KeyValuePair<string, string>>(pSubject, new KeyValuePair<string, string>(pStart, pEnd));
        }

        /// <summary>
        /// Posts the creation of the "Mulig Booking" to the server, with all related fields.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="inputSubject"></param>
        [HttpPost]
        public void Post([FromBody]string json)
        {
            _bookingModel = json.DeserializeJson<Models.PossibleBookingMessage>();
            
            // Takes care of the try-catch boilerplate by wrapping it in an action
            Action<BookingSystemDataContext> submitChanges = db =>
            {
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    _numberOfErrors++;
                    Debug.WriteLine("DEBUG: " + e.Message);
                }
            };
            // creates DateTimes based on the data in the int arrays
            DateTime
                start = _bookingModel.StartDate,
                end = _bookingModel.EndTime;

            // gets the subject from the database
            var subject = _db.Fags.SingleOrDefault(f => f.Navn.Equals(_bookingModel.Subject));

            // if the subject isn't null, and the number of errors is 0, then
            if (subject != null && _numberOfErrors == 0)
            {
                // create a new booking
                Booking booking = new Booking
                {
                    StartTid = start,
                    SlutTid = end,
                    Fag_id = subject._id
                };

                // write it to the database
                _db.Bookings.InsertOnSubmit(booking);

                // commit the changes to the database
                submitChanges(_db);

                // create a new possible booking
                Mulig_Booking possible = new Mulig_Booking
                {
                    Booking_id = booking._id
                };

                // write it to the database
                _db.Mulig_Bookings.InsertOnSubmit(possible);

                // commit changes to the database
                submitChanges(_db);
            }
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

            var message = new Models.PossibleBookingMessages(_db.Bookings, _db.Mulig_Bookings);

            response.OK(message.SerializeToJsonObject());

            return response;
        }
    }
}
