using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository specialized for Teacher entites
    /// </summary>
    public class TeacherRepository : SimpleRepository<Models.Teacher>
    {
        private Table<Models.Name> _names;
        private Table<Models.User> _users;

        /// <summary>
        /// Creates a new TeacherRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public TeacherRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            : base(dataContext)
        {
            _names = _dataContext.Names;
            _users = _dataContext.Users;
        }

        /// <summary>
        /// Enqueues a Teacher entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// If the associated Name entity was only used by the deleted teacher entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="teacher">The Teacher entity to be deleted</param>
        public override void DeleteOnSubmit(Models.Teacher teacher)
        {
            base.DeleteOnSubmit(teacher);

            var user = _users.SingleOrDefault(u => u.Id == teacher.UserId);
            _users.DeleteOnSubmit(user);

            if (_names.Count(n => n == teacher.Name) == 1)
            {
                var name = _names.SingleOrDefault(n => n == teacher.Name);
                _names.DeleteOnSubmit(name);
            }
        }
    }
}