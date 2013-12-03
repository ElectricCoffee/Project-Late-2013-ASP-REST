using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace REST_Service.Repositories
{
    public interface IRepository<TModel> where TModel : Models.IModel
    {
        void InsertOnSubmit(TModel entity);
        void DeleteOnSubmit(TModel entity);
        IEnumerable<TModel> Where(Func<TModel, bool> predicate);
        IEnumerable<TModel> GetAll();
        TModel Single(Func<TModel, bool> predicate);
        TModel GetById(int id);
    }
}