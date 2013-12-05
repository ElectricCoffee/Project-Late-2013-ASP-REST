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
        private const string RESPONSE_OK = "{\"Response\":\"OK\"}";
        /// <summary>
        /// Create connection to database
        /// </summary>
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private DataLayer.ManualBookingSystemDataContext _db;

        public ConcreteBookingController(DataLayer.ManualBookingSystemDataContext context)
        {
            _db = context;
        }
        public ConcreteBookingController()
        {
            _db = new DataLayer.ManualBookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// Method for postning to database with HttpResponseMessage. 
        /// </summary>
        /// <param name="concreteBooking">A model of the JSON response</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Models.ConcreteBooking concreteBooking) {

            // Set the number of Errors to 0
            var numberOfErrors = 0;

            // make new repository of the possible booking database table
            var possibleBookingRepo = new Repositories.PossibleBookingRepository(_db);

            // find the first possible booking that mathces the id in the concrete booking
            var possibleBooking = possibleBookingRepo.Single(pb => pb.Id == concreteBooking.PossibleBookingId);
            if (possibleBooking != null)
            {
                //Get subject from database if subject id exists
                var cls = _db.Subjects.FirstOrDefault(f => f.Id == possibleBooking.Subject.Id);
                if (cls != null)
                {
                    // create a student repository based on the student database table
                    var studentRepo = new Repositories.StudentRepository(_db);

                    // correct the subject field in the object
                    concreteBooking.Subject = _db.GetTable<Models.Subject>().FirstOrDefault(s => s.Name.Equals(concreteBooking.Subject.Name));

                    // correct the student field in the object
                    concreteBooking.Student = studentRepo.Single(s => s.Username.Equals(concreteBooking.Student.Username));

                    // insert the object to the table in the database
                    _db.GetTable<Models.ConcreteBooking>().InsertOnSubmit(concreteBooking);

                    // write the changes
                    _db.SafeSubmitChanges(ref numberOfErrors);
                }
            }
            else
                numberOfErrors++;

            //Create new HttpResponseMessage
            var message = new HttpResponseMessage();

            //If there are errors send forbidden else send OK
            if (numberOfErrors != 0)
                message.Forbidden("Errors: " + numberOfErrors);
            else message.OK(RESPONSE_OK);

            //Returns the HttpResponseMessage
            return message;
        }

        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int concreteBookingId)
        {
            var message = new HttpResponseMessage();
            var bookingRepo = new Repositories.BookingRepository(_db);

            var concBook = bookingRepo.Single(cb => cb.Id == concreteBookingId);

            if (concBook != null)
            {
                bookingRepo.DeleteOnSubmit(concBook);

                _db.SafeSubmitChanges();

                message.OK(RESPONSE_OK);
            }
            else 
                message.Forbidden("Den valgte række i tabellen kunne ikke findes");

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
    }
}
