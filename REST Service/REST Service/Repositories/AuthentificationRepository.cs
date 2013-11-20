using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST_Service.Repositories
{
    public class AuthentificationRepository
    {
        private static AuthentificationRepository _instance;
        private List<Models.Authentification> _list;

        private AuthentificationRepository()
        {
            _list = new List<Models.Authentification>();
        }

        public static AuthentificationRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AuthentificationRepository();
                return _instance;
            }
        }

        public void Add(Models.Authentification authentification)
        {
            _list.Add(authentification);
        }

        public IEnumerable<Models.Authentification> Where(Func<Models.Authentification,bool> predicate)
        {
            return _list.Where(predicate);
        }

        public Models.Authentification Single(Func<Models.Authentification, bool> predicate)
        {
            return _list.Single(predicate);
        }

        public bool Exists(string tokenKey)
        {
            return _list.Exists(a => a.TokenKey == tokenKey);
        }
    }
}