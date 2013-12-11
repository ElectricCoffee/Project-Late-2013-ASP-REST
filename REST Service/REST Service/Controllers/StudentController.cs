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

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var studentRepository = new Repositories.StudentRepository(_db); // Get student list
            var response = new HttpResponseMessage(); // Make new HttpResponse message

            var students = studentRepository.Where(s => s.Approved == false); //Get the students where approved is false
            response.OK(students.SerializeToJsonObject()); 

            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put([FromUri] int id, [FromBody] Messages.Approved approvedMessage)
        {
            var studentRepository = new Repositories.StudentRepository(_db);

            var student = studentRepository.GetById(id);
            student.Approved = approvedMessage.Approved;

            var response = HttpResponse.Try<SqlException>(
                action: () => _db.SubmitChanges(),
                success: "{\"Response\":\"Success\"}",
                failure: "Kunne ikke finde en studerende der matcher ID'et");

            return response;
        }
    }
}
