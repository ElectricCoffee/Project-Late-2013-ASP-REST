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

        public KeyValuePair<string, string> Get([FromUri] string username, [FromUri] string password)
        {
            BookingSystemDataContext db = new BookingSystemDataContext();

            if (_db.Users.FirstOrDefault(b => b.Username == username && b.Password == password) != null)
            {
                var auth = new Models.Authentification(username, password);
                _authRepo.Add(auth);

                Debug.WriteLine(string.Format(
                    "User logged in with credentials \"{0}\" and \"{1}\" at {2}", auth.Username, auth.Password, DateTime.Now));

                return new KeyValuePair<string, string>("access-token", auth.AccessToken);
            }
            else
                return new KeyValuePair<string, string>("error", "invalid credentials supplied");
        }
    }
}