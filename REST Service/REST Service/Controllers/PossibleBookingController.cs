using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_Service.Controllers
{
    public class PossibleBookingController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private BookingSystemDataContext _db;
        private int _numberOfErrors = 0;

        private string
            _subject = "",
            _startDate = "",
            _endDate = "";

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
        public void Post([FromUri]string startDate, [FromUri]string endDate, [FromUri]string inputSubject)
        {
            // set the private fields to contain the data from our input parameters
            _startDate = startDate;
            _endDate = endDate;
            _subject = inputSubject;

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

            // splits the date strings into string arrays
            string[] 
                splitStart = _startDate.Split('-'), 
                splitEnd = _endDate.Split('-');

            // specifies int arrays of the size 6, to be filled with date data
            int[] 
                startArray = new int[6], 
                endArray = new int[6];

            // if the lengths don't match, increment the number of errors
            if (splitStart.Length < 6 && splitEnd.Length < 6) 
                _numberOfErrors++;

            // fill the integer arrays with the string data, and catch any errors
            try
            {
                for (int i = 0; i < splitStart.Length; i++)
                {
                    startArray[i] = Convert.ToInt32(splitStart[i]);
                    endArray[i] = Convert.ToInt32(splitEnd[i]);
                }
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Debug.WriteLine("DEBUG: " + aoore);
                _numberOfErrors++;
            }
            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e);
                _numberOfErrors++;
            }

            // creates DateTimes based on the data in the int arrays
            DateTime 
                start = new DateTime(startArray[0], startArray[1], startArray[2], startArray[3], startArray[4], startArray[5]),
                end = new DateTime(endArray[0], endArray[1], endArray[2], endArray[3], endArray[4], endArray[5]);

            // gets the subject from the database
            var subject = _db.Fags.SingleOrDefault(f => f.Navn.Equals(_subject));

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
        public KeyValuePair<string, KeyValuePair<string, string>> Get()
        {
            if (_numberOfErrors != 0)
                return Response("Error", "N/A", "N/A");
            else
                return Response(_subject, _startDate, _endDate);
        }
    }
}
