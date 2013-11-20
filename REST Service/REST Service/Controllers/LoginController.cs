using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_Service.Controllers
{
    public class LoginController : ApiController
    {
        public KeyValuePair<string, string> Get([FromUri] string username, [FromUri] string password)
        {
            var user = new Models.User(username, password);
            return new KeyValuePair<string, string>("token-key", user.TokenKey);
        }
    }
}