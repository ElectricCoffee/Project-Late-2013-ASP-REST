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

    public static class AuthentificationResponse
    {
        public static KeyValuePair<string, KeyValuePair<string, string>> GenerateResponse(string token, int accessLevel)
        {
            // {key: ... , value: {key: ..., value: ...}}
            // {key: Authentification Response, value: {key: access level, value: uid}}
            string response, level, uid;

            if(accessLevel == 0) {
                response = "response failed";
                level = null;
                uid = null;
            }
            else {
                response = "response succeeded";
                level = accessLevel.ToString();
                uid = token;
            }

            var value = new KeyValuePair<string,string>(level, uid);
            return new KeyValuePair<string,KeyValuePair<string,string>>(response, value);
        }

    }
}