﻿using System;
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
                //Get subject from database if subject id exists
                var cls = _db.Fags.FirstOrDefault(f => f._id == c.Booking.Fag_id);

                if (cls != null)
                {
                    var classId = cls._id;

                    var subject = _db.GetTable<Models.Subject>().SingleOrDefault(s => s.Name == concreteBooking.Subject.Name);

                    concreteBooking.Subject = subject;

                    _db.GetTable<Models.ConcreteBooking>().InsertOnSubmit(concreteBooking);

                    SubmitChanges(_db, ref numberOfErrors);
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

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            var bookings = _db.GetTable<Models.ConcreteBooking>();

            response.OK(bookings.AsEnumerable().SerializeToJsonObject());

            return response;
        }

        private void SubmitChanges(BookingSystemDataContext db, ref int errors)
        {
            try
            {
                db.SubmitChanges();
            }

            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
                errors++;
            }
        }
    }
}
