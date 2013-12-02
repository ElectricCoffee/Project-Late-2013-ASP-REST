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
    public class ConcreteBookingController : ApiController
    {
        /// <summary>
        /// Create connection to database
        /// </summary>
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private BookingSystemDataContext _db;

        public ConcreteBookingController(BookingSystemDataContext context)
        {
            _db = context;
        }
        public ConcreteBookingController()
        {
            _db = new BookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// Method for postning to database with HttpResponseMessage. 
        /// </summary>
        /// <param name="concreteBooking">A model of the JSON response</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Models.ConcreteBooking concreteBooking) {

            var numberOfErrors = 0;

            //Get possoble booking from database if it exist else returns null
            var c = _db.Mulig_Bookings.FirstOrDefault(mb => mb._id == concreteBooking.PossibleBookingId);
            if (c != null)
            {
                //Get Bookking from database if id matches
                var x = _db.Bookings.FirstOrDefault(b => b._id == concreteBooking.BookingId);

                if (x != null)
                {
                    //Get subject from database if subject id exists
                    var cls = _db.Fags.FirstOrDefault(f => f._id == x._id);

                    if (cls != null)
                    {
                        var classId = cls._id;

                        //Create obejct of model for the booking
                        Models.Booking b = new Models.Booking
                        {
                            StartTime = concreteBooking.StartTime,
                            EndTime = concreteBooking.EndTime,
                            Subject = concreteBooking.Subject
                        };

                        //Add to databse
                        _db.GetTable<Models.Booking>().InsertOnSubmit(b);

                        try
                        {
                            //Write to database
                            _db.SubmitChanges();
                        }

                        catch (Exception e)
                        {
                            Debug.WriteLine("DEBUG: " + e.Message);
                            numberOfErrors++;
                        }

                        Models.ConcreteBooking kb = new Models.ConcreteBooking
                        {
                            BookingId = b.Id,
                            Comment = concreteBooking.Comment,
                            Type = concreteBooking.Type,
                            StatusChanged = concreteBooking.StatusChanged

                        };

                        _db.GetTable<Models.ConcreteBooking>().InsertOnSubmit(kb);

                        try
                        {
                            _db.SubmitChanges();
                        }

                        catch (Exception e)
                        {
                            Debug.WriteLine("DEBUG: " + e.Message);
                            numberOfErrors++;
                        }
                    }
                }
            }

            else
                numberOfErrors++;

            //Create new HttpResponseMessage
            var message = new HttpResponseMessage();

            //If there are errors send forbidden else send OK
            if (numberOfErrors != 0)
                message.Forbidden("Errors: " + numberOfErrors);
            else message.OK("{\"Response\":\"OK\"}");

            //Returns the HttpResponseMessage
            return message;
        }
    }
}
