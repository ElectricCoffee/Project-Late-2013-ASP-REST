using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using REST_Service.Utils;

namespace REST_Service.Controllers
{
    public class SubjectController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private DataLayer.ManualBookingSystemDataContext _db;

        public SubjectController(DataLayer.ManualBookingSystemDataContext context)
        {
            _db = context;
        }

        public SubjectController()
        {
            _db = new DataLayer.ManualBookingSystemDataContext(_connectionString);
        }

        [HttpGet]
        public HttpResponseMessage Get() {
            var subjects = _db.Subjects;

            var response = new HttpResponseMessage();

            response.OK(subjects.SerializeToJsonObject());

            return response;
        }

        public HttpResponseMessage Get([FromUri]int teacherId)
        {
            var subjects = _db.Subjects;
            var response = new HttpResponseMessage();

            var subject = subjects.FirstOrDefault(s => s.Teacher.Id == teacherId);

            if (subject != null)
                response.OK(subject.SerializeToJsonObject());
            else
                response.Forbidden("Den pågældende lærer blev ikke fundet");

            return response;

        }
    }
}
