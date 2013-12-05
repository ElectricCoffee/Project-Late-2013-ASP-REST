using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A Repository specialized for handling Authentication instances
    /// </summary>
    public class AuthenticationRepository
    {
        private static AuthenticationRepository _instance;
        private List<Models.Authentification> _authentications;

        /// <summary>
        /// Creates a new AuthenticationRepository instance
        /// </summary>
        private AuthenticationRepository()
        {
            _authentications = new List<Models.Authentification>();
        }

        /// <summary>
        /// Returns a singleton instance of AuthenticationRepository
        /// </summary>
        public static AuthenticationRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AuthenticationRepository();
                return _instance;
            }
        }

        /// <summary>
        /// Gets the entire list of Authentication instances
        /// </summary>
        public List<Models.Authentification> Authentications
        {
            get { return _authentications; }
        }

        /// <summary>
        /// Adds the specified Authentication instance to the repository
        /// </summary>
        /// <param name="authentification">The Authentication instance to be added</param>
        public void Add(Models.Authentification authentification)
        {
            _authentications.Add(authentification);
        }

        /// <summary>
        /// Searches the repository for a set of Authentication instances using the specified predicate
        /// </summary>
        /// <param name="predicate">The delimiters to search with</param>
        /// <returns>The delimited set of Authentication instances</returns>
        public IEnumerable<Models.Authentification> Where(Func<Models.Authentification,bool> predicate)
        {
            return _authentications.Where(predicate);
        }

        /// <summary>
        /// Searches the repository for a single Authentication instance using the specified predicate
        /// </summary>
        /// /// <param name="predicate">The delimiters to search with</param>
        /// <returns>The matching Authentication instance</returns>
        public Models.Authentification Single(Func<Models.Authentification, bool> predicate)
        {
            return _authentications.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Checks if an Authentication instance matching the specified predicate exists
        /// </summary>
        /// <param name="predicate">The delimiters to check with</param>
        /// <returns>A boolean indicating whether or not the Authentication instance exists</returns>
        public bool Exists(string tokenKey)
        {
            return _authentications.Exists(a => a.AccessToken == tokenKey);
        }
    }
}