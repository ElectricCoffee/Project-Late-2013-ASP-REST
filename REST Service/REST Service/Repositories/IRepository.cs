using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public interface IRepository<TModel>
    {
        void InsertOnSubmit(TModel entity);
        void DeleteOnSubmit(TModel entity);
        IQueryable<TModel> SearchFor(Expression<Func<TModel, bool>> predicate);
        IQueryable<TModel> GetAll();
        TModel GetById(int id);
    }
}