using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public class Authentification
    {
        public Authentification(string username, string password)
        {
            Username = username;
            Password = password;
            var guid = Guid.NewGuid();
            AccessToken = Convert.ToBase64String(guid.ToByteArray());
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessToken { get; private set; }
    }

    public class AuthentificationResponse
    {
        public string AccessToken { get; set ; }

        public AuthentificationResponse()
        {
            
        }
    }
}