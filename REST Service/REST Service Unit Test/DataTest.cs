using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using REST_Service;
using System.Diagnostics;

namespace REST_Service_Unit_Test
{
    /// <summary>
    /// Defines a basic template for database dependent tests
    /// </summary>
    [TestClass]
    public class DataTest
    {
        protected TransactionScope _transactionScope;
        protected string _connectionString;
        protected BookingSystemDataContext _context;

        /// <summary>
        /// Prepares a test database
        /// </summary>
        [TestInitialize]
        public virtual void Initialize() {
            _connectionString = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;

            // Circumventing Linq |DataDirectory| bug
            if (_connectionString.Contains("|DataDirectory|"))
            {
                var dataDirectory = AppDomain.CurrentDomain.GetData("APPBASE").ToString();
                _connectionString = _connectionString.Replace("|DataDirectory|", dataDirectory);
            }

            EnsureDataExists();
        }

        /// <summary>
        /// Ensures a test database with known scheme is present
        /// and instantiates a new TransactionScope to contain further data operations
        /// </summary>
        public void EnsureDataExists()
        {
            _context = new BookingSystemDataContext(_connectionString);

            // attempting to hunt down the error source
            Debug.WriteLine(_context.Mapping.DatabaseName);
            foreach (var invalidChar in System.IO.Path.GetInvalidPathChars())
                Debug.Write(invalidChar);
            foreach (var invalidChar in System.IO.Path.GetInvalidFileNameChars())
                Debug.Write(invalidChar);

            if (!_context.DatabaseExists())
                _context.CreateDatabase();

            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        /// <summary>
        /// Disposes the TransactionScope
        /// </summary>
        [TestCleanup]
        public virtual void Cleanup()
        {
            _transactionScope.Dispose();
        }
    }
}
