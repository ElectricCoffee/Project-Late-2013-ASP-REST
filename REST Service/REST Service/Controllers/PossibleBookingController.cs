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
        /// 
        /// </summary>
        public PossibleBookingController()
        {
            _db = new BookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="inputSubject"></param>
        /// <returns></returns>
        public void Post([FromUri]string startDate, [FromUri]string endDate, [FromUri]string inputSubject)
        {
            _startDate = startDate;
            _endDate = endDate;
            _subject = inputSubject;

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

            string[] 
                splitStart = _startDate.Split('-'), 
                splitEnd = _endDate.Split('-');

            int[] 
                startArray = new int[6], 
                endArray = new int[6];

            if (splitStart.Length < 6 && splitEnd.Length < 6) 
                _numberOfErrors++;

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

            var start = new DateTime(startArray[0], startArray[1], startArray[2], startArray[3], startArray[4], startArray[5]);
            var end = new DateTime(endArray[0], endArray[1], endArray[2], endArray[3], endArray[4], endArray[5]);

            var subject = _db.Fags.SingleOrDefault(f => f.Navn.Equals(_subject));

            if (subject != null && _numberOfErrors == 0)
            {
                Booking booking = new Booking
                {
                    StartTid = start,
                    SlutTid = end,
                    Fag_id = subject._id
                };

                _db.Bookings.InsertOnSubmit(booking);

                submitChanges(_db);

                Mulig_Booking possible = new Mulig_Booking
                {
                    Booking_id = booking._id
                };

                _db.Mulig_Bookings.InsertOnSubmit(possible);

                submitChanges(_db);
            }
        }

        public KeyValuePair<string, KeyValuePair<string, string>> Get()
        {
            if (_numberOfErrors != 0)
                return Response("Error", "N/A", "N/A");
            else
                return Response(_subject, _startDate, _endDate);
        }
    }
}
