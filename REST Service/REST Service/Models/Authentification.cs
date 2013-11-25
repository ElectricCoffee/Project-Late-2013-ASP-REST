﻿using System;
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
        /// <summary>
        /// Generates a nested Key-Value Pair containing the response, access level, and UID of the user who logged in
        /// </summary>
        /// <param name="token">The UID generated by the Authentification Class</param>
        /// <param name="accessLevel">The access level of the retreived person</param>
        /// <returns></returns>
        public static KeyValuePair<string, KeyValuePair<string, string>> GenerateResponse(string token, int accessLevel)
        {
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