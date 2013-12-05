using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A specific implementation of IRepository compatible with User entites
    /// </summary>
    public class UserRepository : SimpleRepository<Models.User>
    {
        private Table<Models.Name> _names;
        private Table<Models.Student> _students;
        private Table<Models.Teacher> _teachers;
        private Table<Models.Administrator> _administrators;

        /// <summary>
        /// Creates a new UserRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public UserRepository(DataLayer.ManualBookingSystemDataContext dataContext)
            :base(dataContext)
        {
            _names = _dataContext.Names;
            _students = _dataContext.Students;
            _teachers = _dataContext.Teachers;
            _administrators = _dataContext.Administrators;
        }

        /// <summary>
        /// Creating basic User entities are discouraged, go through the specializations instead,
        /// i.e. AdministratorRepository, StudentRepository or TeacherRepository
        /// </summary>
        /// <param name="user">The User entity to be inserted</param>
        public override void InsertOnSubmit(Models.User user)
        {
            throw (new MethodAccessException(
                "Attempt to create basic Users are discouraged," +
                "go through the specializations instead"));
        }

        /// <summary>
        /// Enqueues a User entity to be deleted from the table on submit
        /// </summary>
        /// <remarks>
        /// Also checks that the User entity doesn't exist as a specialized entity,
        /// i.e. Administrator, Student or Teacher.
        /// If the User entity is found to be either of the specializations,
        /// the specialized entity is deleted before the User entity itself.
        /// Furthermore if the associated Name entity was only used by the deleted User entity,
        /// then that Name entity is also deleted
        /// </remarks>
        /// <param name="user">The User entity to be deleted</param>
        public override void DeleteOnSubmit(Models.User user)
        {
            var student = _students.SingleOrDefault(s => s.UserId == user.Id);
            if (student != null)
                _students.DeleteOnSubmit(student);

            var teacher = _teachers.SingleOrDefault(t => t.UserId == user.Id);
            if (teacher != null)
                _teachers.DeleteOnSubmit(teacher);

            var administrator = _administrators.SingleOrDefault(a => a.UserId == user.Id);
            if (administrator != null)
                _administrators.DeleteOnSubmit(administrator);

            base.DeleteOnSubmit(user);

            if (_names.Count(n => n == user.Name) == 1)
            {
                var name = _names.SingleOrDefault(n => n == user.Name);
                _names.DeleteOnSubmit(name);
            }
        }
    }
}