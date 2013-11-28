using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public class AdministratorRepository : IRepository<Models.Administrator>
    {
        private DataLayer.ManualBookingSystemDataContext _dataContext;
        private Table<Models.Name> _nameTable;
        private Table<Models.User> _userTable;
        private Table<Models.Administrator> _administratorTable;
        
        public AdministratorRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            _nameTable = dataContext.GetTable<Models.Name>();
            _userTable = dataContext.GetTable<Models.User>();
            _administratorTable = dataContext.GetTable<Models.Administrator>();
        }

        public void InsertOnSubmit(Models.Administrator administrator)
        {
            _administratorTable.InsertOnSubmit(administrator);
        }

        public void DeleteOnSubmit(Models.Administrator administrator)
        {
            _administratorTable.Attach(administrator);
            _administratorTable.DeleteOnSubmit(administrator);

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

        public IQueryable<Models.Administrator> SearchFor(Expression<Func<Models.Administrator, bool>> predicate)
        {
            return _administratorTable.Where(predicate);
        }

        public IQueryable<Models.Administrator> GetAll()
        {
            return _administratorTable;
        }

        public Models.Administrator GetById(int id)
        {
            return _administratorTable.Single(m => m.Id.Equals(id));
        }
    }
}