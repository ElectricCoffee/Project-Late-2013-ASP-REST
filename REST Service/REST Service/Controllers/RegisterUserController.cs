using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_Service.Controllers
{
    public class RegisterUserController : ApiController
    {
        private BookingSystemDataContext _db;
        
        public RegisterUserController(BookingSystemDataContext context)
        {
            _db = context;
        }
        public RegisterUserController()
        {
            _db = new BookingSystemDataContext();
        }

        public KeyValuePair<string, KeyValuePair<string, string>> Get(
            [FromUri] string firstname, [FromUri] string lastname, [FromUri] string email, [FromUri] string password, [FromUri] string homeroomClass)
        {
            var numberOfErrors = 0;

            Func<string, KeyValuePair<string, KeyValuePair<string, string>>> response = str => 
                new KeyValuePair<string, KeyValuePair<string, string>>(str, new KeyValuePair<string,string>(null,null));

            Action<BookingSystemDataContext> submitChanges = db =>
            {
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    numberOfErrors++;
                    Debug.WriteLine(e.Message);
                }
            };

            var classId = 0;
            var cls = _db.Holds.FirstOrDefault(h => h.Navn == homeroomClass);
            if (cls != null)
            {
                classId = cls._id;
            }
            else numberOfErrors++;

            submitChanges(_db);

            Navn name = new Navn
            {
                Fornavn = firstname,
                Efternavn = lastname
            };

            _db.Navns.InsertOnSubmit(name);

            submitChanges(_db);

            Bruger user = new Bruger
            {
                Navn_id = name._id,
                Brugernavn = email,
                Password = password
            };

            _db.Brugers.InsertOnSubmit(user);

            submitChanges(_db);

            Studerende stud = new Studerende
            {
                Godkendt = 0,
                Bruger_id = user._id,
                Hold_id = classId
            };

            _db.Studerendes.InsertOnSubmit(stud);

            submitChanges(_db);

            if (numberOfErrors != 0)
                return response("Errors: " + numberOfErrors);
            else return response("OK");
        }
    }
}
