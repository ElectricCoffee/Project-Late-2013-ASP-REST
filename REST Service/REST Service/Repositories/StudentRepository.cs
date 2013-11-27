using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class StudentRepository : IRepository<Models.Student>
    {
        private BookingSystemDataContext _context;
        private Table<Models.Name> _nameTable;
        private Table<Models.User> _userTable;
        private Table<Models.Student> _studentTable;
        
        public StudentRepository(BookingSystemDataContext context)
        {
            _context = context;
        }

        public void InsertOnSubmit(Models.Student student)
        {
            var name = new Models.Name
            {
                FirstName = student.Name.FirstName,
                LastName = student.Name.LastName
            };

            _nameTable.InsertOnSubmit(name);
            _context.SubmitChanges();

            var user = new Models.User
            {
                Username = student.UserName,
                Password = student.Password,
                Name = name
            };

            _userTable.InsertOnSubmit(user);
            _context.SubmitChanges();

            student.UserId = user.Id;

            _studentTable.InsertOnSubmit(student);
        }

        public void DeleteOnSubmit(Models.Student student)
        {
            _studentTable.Attach(student);
            _studentTable.DeleteOnSubmit(student);

            var user = _userTable.SingleOrDefault(u => u.Id == student.UserId);

            _userTable.Attach(user);
            _userTable.DeleteOnSubmit(user);

            if (_nameTable.Count(n => n == student.Name) == 1)
            {
                var name = _nameTable.SingleOrDefault(n => n == student.Name);
                _nameTable.Attach(name);
                _nameTable.DeleteOnSubmit(name);
            }
        }

        public IQueryable<Models.Student> SearchFor(Expression<Func<Models.Student, bool>> predicate)
        {
            return _studentTable.Where(predicate);
        }

        public IQueryable<Models.Student> GetAll()
        {
            return _studentTable;
        }

        public Models.Student GetById(int id)
        {
            return _studentTable.Single(m => m.Id.Equals(id));
        }
    }
}