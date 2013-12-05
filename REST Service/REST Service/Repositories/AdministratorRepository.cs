using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository specialized for Administrator entites
    /// </summary>
    public class AdministratorRepository : SimpleRepository<Models.Administrator>
    {
        private Table<Models.Name> _names;
        private Table<Models.User> _users;

        /// <summary>
        /// Creates a new AdministratorRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public AdministratorRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            : base(dataContext)
        {
            _names = _dataContext.GetTable<Models.Name>();
            _users = _dataContext.GetTable<Models.User>();
        }

        /// <summary>
        /// Enqueues an Administrator entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// If the associated Name entity was only used by the deleted Administrator entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="administrator">The Administrator entity to be deleted</param>
        public override void DeleteOnSubmit(Models.Administrator administrator)
        {
            base.DeleteOnSubmit(administrator);

            var user = _users.SingleOrDefault(u => u.Id == administrator.UserId);

            _users.DeleteOnSubmit(user);

            if (_names.Count(n => n == administrator.Name) == 1)
            {
                var name = _names.SingleOrDefault(n => n == administrator.Name);
                _names.DeleteOnSubmit(name);
            }
        }
    }
}