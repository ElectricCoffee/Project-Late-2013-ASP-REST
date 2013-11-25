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

        public KeyValuePair<string, KeyValuePair<string,string>> Get([FromUri] string username, [FromUri] string password)
        {
            Bruger br = _db.Brugers.FirstOrDefault(b => b.Brugernavn == username && b.Password == password);

            var accessLevel = 0;

            if (br != null)
            {
                accessLevel = 
                    _db.Administrators.FirstOrDefault(a => a.Bruger_id == br._id) != null ? 3 :
                    _db.Lærers.FirstOrDefault        (l => l.Bruger_id == br._id) != null ? 2 :
                    _db.Studerendes.FirstOrDefault   (s => s.Bruger_id == br._id) != null ? 1 : 0;
            }

            if (_db.Brugers.FirstOrDefault(b => b.Brugernavn == username && b.Password == password) != null)
            {
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