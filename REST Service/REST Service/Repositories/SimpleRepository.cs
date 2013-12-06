using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace REST_Service.Repositories
{
    /// <summary>
    /// A simple implementation of IRepository compatible with all IModel entites
    /// </summary>
    /// <typeparam name="TModel">The model type for which this repository should interface with</typeparam>
    public class SimpleRepository<TModel> : IRepository<TModel> where TModel : class, Models.IModel
    {
        protected DataLayer.ManualBookingSystemDataContext _dataContext;
        protected Table<TModel> _mainEntities;

        /// <summary>
        /// Creates a new SimpleRepository instance
        /// </summary>
        /// <param name="dataContext">An instance of ManualBookingSystemDataContext</param>
        public SimpleRepository(DataLayer.ManualBookingSystemDataContext dataContext)
        {
            _dataContext = dataContext;
            try
            {
                _mainEntities = dataContext.GetTable<TModel>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Gets or sets a specific entity delimited by the id
        /// </summary>
        /// <param name="id">The id to delimit by</param>
        /// <returns>The entity whoose id matches the delimiter</returns>
        public TModel this[int id]
        {
            get { return _mainEntities.SingleOrDefault(e => e.Id == id); }
            set
            {
                TModel entity = _mainEntities.SingleOrDefault(e => e.Id == id);
                entity = value;
            }
        }

        /// <summary>
        /// Gets the entire table of entities
        /// </summary>
        public IEnumerable<TModel> Entities
        {
            get { return _mainEntities.AsEnumerable(); }
        }

        /// <summary>
        /// Enqueues an entity to be inserted in the table on submit
        /// </summary>
        /// <param name="entity">The entity to be inserted</param>
        public virtual void InsertOnSubmit(TModel entity)
        {
            _mainEntities.InsertOnSubmit(entity);
        }

        /// <summary>
        /// Enqueues an entity to be deleted from the table on submit
        /// </summary>
        /// <param name="entity">The entity to be deleted</param>
        public virtual void DeleteOnSubmit(TModel entity)
        {
            _mainEntities.DeleteOnSubmit(entity);
        }

        /// <summary>
        /// Searches the table of entities for a set of entities using the specified predicate
        /// </summary>
        /// <param name="predicate">The delimiters to search with</param>
        /// <returns>The delimited set of entites</returns>
        public virtual IEnumerable<TModel> Where(Func<TModel, bool> predicate)
        {
            return _mainEntities.AsEnumerable().Where(predicate);
        }

        /// <summary>
        /// Gets the entire table of entities
        /// </summary>
        public virtual IEnumerable<TModel> GetAll()
        {
            return _mainEntities.AsEnumerable();
        }

        /// <summary>
        /// Searches the table of entities for a single entity using the specified predicate
        /// </summary>
        /// /// <param name="predicate">The delimiters to search with</param>
        /// <returns>The matching entity</returns>
        public virtual TModel Single(Func<TModel, bool> predicate)
        {
            return _mainEntities.AsEnumerable().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets a specific entity delimited by the id
        /// </summary>
        public virtual TModel GetById(int id)
        {
            return _mainEntities.SingleOrDefault(e => e.Id.Equals(id));
        }

        /// <summary>
        /// Checks if an entity matching the specified predicate exists
        /// </summary>
        /// <param name="predicate">The delimiters to check with</param>
        /// <returns>A boolean indicating whether or not the entity exists</returns>
        public bool Exists(Func<TModel, bool> predicate)
        {
            if (_mainEntities.AsEnumerable().FirstOrDefault(predicate) != null)
                return true;
            else
                return false;
        }
    }
}