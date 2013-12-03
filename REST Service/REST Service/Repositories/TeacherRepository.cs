using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class TeacherRepository : IRepository<Models.Teacher>
    {
        private DataLayer.ManualBookingSystemDataContext _dataContext;
        private Table<Models.Name> _nameTable;
        private Table<Models.User> _userTable;
        private Table<Models.Teacher> _teacherTable;
        
        public TeacherRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _nameTable = dataContext.Names;
            _userTable = dataContext.Users;
            _teacherTable = dataContext.Teachers;
        }

        public void InsertOnSubmit(Models.Teacher teacher)
        {
            _teacherTable.InsertOnSubmit(teacher);
        }

        public void DeleteOnSubmit(Models.Teacher teacher)
        {
            _teacherTable.Attach(teacher);
            _teacherTable.DeleteOnSubmit(teacher);

            var user = _userTable.SingleOrDefault(u => u.Id == teacher.UserId);

            _userTable.Attach(user);
            _userTable.DeleteOnSubmit(user);

            if (_nameTable.Count(n => n == teacher.Name) == 1)
            {
                var name = _nameTable.SingleOrDefault(n => n == teacher.Name);
                _nameTable.Attach(name);
                _nameTable.DeleteOnSubmit(name);
            }
        }

        public IEnumerable<Models.Teacher> Where(Func<Models.Teacher, bool> predicate)
        {
            return _teacherTable.AsEnumerable().Where(predicate);
        }

        public IEnumerable<Models.Teacher> GetAll()
        {
            return _teacherTable;
        }

        public Models.Teacher Single(Func<Models.Teacher, bool> predicate)
        {
            return _teacherTable.AsEnumerable().FirstOrDefault(predicate);
        }

        public Models.Teacher GetById(int id)
        {
            return _teacherTable.Single(m => m.Id.Equals(id));
        }
    }
}