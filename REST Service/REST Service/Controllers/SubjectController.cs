﻿using System;
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
        private BookingSystemDataContext _db;

        public SubjectController(BookingSystemDataContext context)
        {
            _db = context;
        }
        public SubjectController()
        {
            _db = new BookingSystemDataContext(_connectionString);
        }

        [HttpGet]
        public HttpResponseMessage Get() {
            var subjects = _db.Fags;

            HttpResponseMessage response = new HttpResponseMessage();

            response.OK(subjects.SerializeToJsonObject());

            return response;
        }
    }
}
