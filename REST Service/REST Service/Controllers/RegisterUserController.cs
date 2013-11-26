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
    public class RegisterUserController : ApiController
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DummyConnection"].ConnectionString;
        private BookingSystemDataContext _db;
        
        public RegisterUserController(BookingSystemDataContext context)
        {
            _db = context;
        }
        public RegisterUserController()
        {
            _db = new BookingSystemDataContext(_connectionString);
        }
        /// <summary>
        /// Creates a user in the database
        /// </summary>
        /// <param name="firstname">Specifies the user's first name</param>
        /// <param name="lastname">Specifies the user's last name</param>
        /// <param name="email">Specifies the uers's email address</param>
        /// <param name="password">Specifies the user's password</param>
        /// <param name="homeroomClass">Specifies the user's homeroom class</param>
        /// <returns>JSON Object with either an OK or an Error</returns>
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
                    Debug.WriteLine("DEBUG: " + e.Message);
                }
            };

            var classId = 0;
            var cls = _db.Holds.FirstOrDefault(h => h.Navn == homeroomClass);

            // checks if cls exits in table 
            if (cls != null)
            {
                // checks if it is an eal mail and if it exists in the database
                if (email.Contains("@edu.eal.dk") && _db.Brugers.FirstOrDefault(b => b.Brugernavn == email) == null)
                {
                    classId = cls._id;
            
                    submitChanges(_db);
                    Debug.WriteLine("Homeroom name: " + homeroomClass);
                    Debug.WriteLine("Finished searching through holds, number of errors: " + numberOfErrors);

                    Navn name = new Navn
                    {
                        Fornavn = firstname,
                        Efternavn = lastname
                    };

                    _db.Navns.InsertOnSubmit(name);
                
                    submitChanges(_db);
                    Debug.WriteLine("Finished adding a name, number of errors: " + numberOfErrors);

                    Bruger user = new Bruger
                    {
                        Navn_id = name._id,
                        Brugernavn = email,
                        Password = password
                    };

                    _db.Brugers.InsertOnSubmit(user);

                    submitChanges(_db);
                    Debug.WriteLine("Finished adding a user, number of errors: " + numberOfErrors);
                

                Studerende stud = new Studerende
                {
                    Godkendt = 0,
                    Bruger_id = user._id,
                    Hold_id = classId
                };

                _db.Studerendes.InsertOnSubmit(stud);

                submitChanges(_db);
                Debug.WriteLine("Finished adding a student, number of errors: " + numberOfErrors);
                }
                else numberOfErrors++;
            }
            else
            {
                numberOfErrors++;
                Debug.WriteLine("DEBUG: Couldn't find the specified homeroom");
            }

            if (numberOfErrors != 0)
                return response("Errors: " + numberOfErrors);
            else return response("OK");
        }
    }
}
