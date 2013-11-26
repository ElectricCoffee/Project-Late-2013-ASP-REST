using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_Service.Controllers
{
    public class LoginController : ApiController
    {
        private Repositories.AuthentificationRepository _authRepo;
        private BookingSystemDataContext _db;

        public LoginController(BookingSystemDataContext context)
        {
            _db = context;
        }

        public LoginController()
        {
            _authRepo = Repositories.AuthentificationRepository.Instance;
            _db = new BookingSystemDataContext();
        }

        /// <summary>
        /// Generates a Get response containing an access level and a GUID.
        /// This response is based off of login credentials of a user trying to access the system.
        /// </summary>
        /// <param name="username">The Username of the person logging in</param>
        /// <param name="password">The Password of the person logging in</param>
        /// <returns>Nested Key-Value pairs that represent the access and access level of the person logging in.</returns>
        public KeyValuePair<string, KeyValuePair<string,string>> Get([FromUri] string username, [FromUri] string password)
        {
            Bruger br = _db.Brugers.FirstOrDefault(b => b.Brugernavn == username && b.Password == password);

            var accessLevel = 0;
            
            if (br != null)
            {
                // gets the access level depending on which table the user exists in
                accessLevel = 
                    _db.Administrators.FirstOrDefault(a => a.Bruger_id == br._id) != null ? 3 :
                    _db.Lærers.FirstOrDefault        (l => l.Bruger_id == br._id) != null ? 2 :
                    _db.Studerendes.FirstOrDefault   (s => s.Bruger_id == br._id) != null ? 1 : 0;
            
                var auth = new Models.Authentification(username, password);
                
                _authRepo.Add(auth);

                Debug.WriteLine(string.Format(
                    "User logged in with credentials \"{0}\" and \"{1}\" at {2}", auth.Username, auth.Password, DateTime.Now));


                return Models.AuthentificationResponse.GenerateResponse(auth.AccessToken, accessLevel);
            }
            else
                return Models.AuthentificationResponse.GenerateResponse("", 0);
        }
    }
}