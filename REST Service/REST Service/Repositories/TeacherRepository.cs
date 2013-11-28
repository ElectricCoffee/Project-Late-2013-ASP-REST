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
            _nameTable = dataContext.GetTable<Models.Name>();
            _userTable = dataContext.GetTable<Models.User>();
            _teacherTable = dataContext.GetTable<Models.Teacher>();
        }

        public void InsertOnSubmit(Models.Teacher administrator)
        {
            _teacherTable.InsertOnSubmit(administrator);
        }

        public void DeleteOnSubmit(Models.Teacher administrator)
        {
            _teacherTable.Attach(administrator);
            _teacherTable.DeleteOnSubmit(administrator);

            var user = _userTable.SingleOrDefault(u => u.Id == administrator.UserId);

            _userTable.Attach(user);
            _userTable.DeleteOnSubmit(user);

            if (_nameTable.Count(n => n == administrator.Name) == 1)
            {
                var name = _nameTable.SingleOrDefault(n => n == administrator.Name);
                _nameTable.Attach(name);
                _nameTable.DeleteOnSubmit(name);
            }
        }

        public IQueryable<Models.Teacher> SearchFor(Expression<Func<Models.Teacher, bool>> predicate)
        {
            return _teacherTable.Where(predicate);
        }

        public IQueryable<Models.Teacher> GetAll()
        {
            return _teacherTable;
        }

        public Models.Teacher GetById(int id)
        {
            return _teacherTable.Single(m => m.Id.Equals(id));
        }
    }
}