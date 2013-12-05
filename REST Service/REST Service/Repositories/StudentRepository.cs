using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository specialized for Student entites
    /// </summary>
    public class StudentRepository : SimpleRepository<Models.Student>
    {
        private Table<Models.Name> _names;
        private Table<Models.User> _users;

        /// <summary>
        /// Creates a new StudentRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public StudentRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            : base(dataContext)
        {
            _names = _dataContext.Names;
            _users = _dataContext.Users;
        }

        /// <summary>
        /// Enqueues a Student entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// If the associated Name entity was only used by the deleted Student entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="student">The Student entity to be deleted</param>
        public override void DeleteOnSubmit(Models.Student student)
        {
            base.DeleteOnSubmit(student);

            var user = _users.SingleOrDefault(u => u.Id == student.UserId);
            _users.DeleteOnSubmit(user);

            if (_names.Count(n => n == student.Name) == 1)
            {
                var name = _names.SingleOrDefault(n => n == student.Name);
                _names.DeleteOnSubmit(name);
            }
        }
    }
}