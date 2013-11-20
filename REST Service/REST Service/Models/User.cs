using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public class User
    {
        public User(string username, string password)
        {
            var guid = Guid.NewGuid();
            TokenKey = Convert.ToBase64String(guid.ToByteArray());
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string TokenKey { get; private set; }
    }
}