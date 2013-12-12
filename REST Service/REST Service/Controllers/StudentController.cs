using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using REST_Service.Utils;
using System.Data.SqlClient;

namespace REST_Service.Controllers
{
    public class StudentController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private DataLayer.ManualBookingSystemDataContext _db;

        public StudentController(DataLayer.ManualBookingSystemDataContext context)
        {
            _db = context;
        }
        public StudentController()
        {
            _db = new DataLayer.ManualBookingSystemDataContext(_connectionString);
        }

        /// <summary>
        /// Gets a list of students with the given approval level
        /// </summary>
        /// <param name="approved">A boolean that defines approval level</param>
        /// <returns>A HttpResponseMessage containing either an OK with the list or a Forbidden with an error</returns>
        [HttpGet]
        public HttpResponseMessage Get([FromUri] bool? approved = null)
        {
            var studentRepository = new Repositories.StudentRepository(_db); 

            return HttpResponse.Try<IEnumerable<Models.Student>, Exception>(
                successAction: () =>
                {
                    var students = approved == null ? // if approved is null
                        studentRepository.GetAll() :  // set students to the full list
                        studentRepository.Where(s => s.Approved == approved); // else get the students that match the approval

                    if (students.Count() < 1) 
                        throw new Exception("Could not find students");

                    return students; // returns the response if it doesnt't throw (converts to json automatically)
                },
                failure: "Kunne ikke hente listen af studerende");
        }

        /// <summary>
        /// Gets the amount of students with the given approval level
        /// </summary>
        /// <param name="approved">A boolean that defines approval level</param>
        /// <returns>A HttpResponseMessage containing either an OK with the count or a Forbidden with an error</returns>
        [HttpGet]
        public HttpResponseMessage Count([FromUri] bool? approved = null)
        {
            var studentRepository = new Repositories.StudentRepository(_db);

            var response = HttpResponse.Try<int, Exception>(
                successAction: () =>
                {
                    var students = approved == null ? // if approval is null
                        studentRepository.GetAll() :  // get the full list of students
                        studentRepository.Where(s => s.Approved == approved); // else 

                    return students.Count(); // returns the response if it doesnt't throw (converts to json automatically)
                },
                failure: "Kunne ikke hente antallet af studerende");

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

            return response;
        }

        /// <summary>
        /// Sets the approval level on a student based on the ID
        /// </summary>
        /// <param name="id">The ID of the student</param>
        /// <param name="approvedMessage">The approval level</param>
        /// <returns>Either an OK with a generic response message, or a Forbidden with an error</returns>
        [HttpPut]
        public HttpResponseMessage Put([FromUri] int id, [FromBody] Messages.Approve approvedMessage) //Put methode to approve students
        {
            var studentRepository = new Repositories.StudentRepository(_db); //get list of students, from database

            var student = studentRepository.GetById(id); //get student by id
            student.Approved = approvedMessage.Approved; //Set the student to approved

            var response = HttpResponse.Try<SqlException>( 
                action: () => _db.SubmitChanges(),
                success: "{\"Response\":\"Success\"}",
                failure: "Kunne ikke finde en studerende der matcher ID'et");

            return response;
        }

        /// <summary>
        /// Deletes a student with the given ID
        /// </summary>
        /// <param name="id">The ID of the student</param>
        /// <returns>Either an OK with a generic response message, or a Forbidden with an error</returns>
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id) //Methode to delete student by id
        {
            var studentRepository = new Repositories.StudentRepository(_db);
            var student = studentRepository.GetById(id);
            studentRepository.DeleteOnSubmit(student);

            var response = HttpResponse.Try<SqlException>(
                action: () => _db.SubmitChanges(),
                success: "{\"Response\":\"Success\"}",
                failure: "Kunne ikke finde en studerende der matcher ID'et");

            return response;
        }
    }
}
