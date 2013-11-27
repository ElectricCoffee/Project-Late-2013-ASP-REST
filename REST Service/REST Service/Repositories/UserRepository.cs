using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class UserRepository : IRepository<Models.User>
    {
        private BookingSystemDataContext _dataContext;
        private Table<Models.Name> _nameTable;
        private Table<Models.User> _userTable;
        private Table<Models.Student> _studentTable;
        // private Table<Models.Teacher> _teacherTable;
        // private Table<Models.Administrator> _administratorTable;
        
        public UserRepository(BookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _nameTable = dataContext.GetTable<Models.Name>();
            _userTable = dataContext.GetTable<Models.User>();
            _studentTable = dataContext.GetTable<Models.Student>();
            // _teacherTable = dataContext.GetTable<Models.Teacher>();
            // _administratorTable = dataContext.GetTable<Models.Administrator>();
        }

        public void InsertOnSubmit(Models.User user)
        {
            _nameTable.InsertOnSubmit(user.Name);
            _dataContext.SubmitChanges();

            user.Name = user.Name; // fill in the foreign key

            _userTable.InsertOnSubmit(user);
        }

        public void DeleteOnSubmit(Models.User user)
        {
            var student = _studentTable.SingleOrDefault(s => s.UserId == user.Id);
            if (student != null)
                _studentTable.DeleteOnSubmit(student);

            /* var teacher = _teacherTable.SingleOrDefault(t => t.UserId == user.Id);
            if (teacher != null)
                _teacherTable.DeleteOnSubmit(teacher); */

            /* var administrator = _administratorTable.SingleOrDefault(a => a.UserId == user.Id);
            if (administrator != null)
                _administratorTable.DeleteOnSubmit(administrator); */

            _userTable.Attach(user);
            _userTable.DeleteOnSubmit(user);

            if (_nameTable.Count(n => n == user.Name) == 1)
            {
                var name = _nameTable.SingleOrDefault(n => n == user.Name);
                _nameTable.Attach(name);
                _nameTable.DeleteOnSubmit(name);
            }
        }

        public IQueryable<Models.User> SearchFor(Expression<Func<Models.User, bool>> predicate)
        {
            return _userTable.Where(predicate);
        }

        public IQueryable<Models.User> GetAll()
        {
            return _userTable;
        }

        public Models.User GetById(int id)
        {
            return _userTable.Single(m => m.Id.Equals(id));
        }
    }
}