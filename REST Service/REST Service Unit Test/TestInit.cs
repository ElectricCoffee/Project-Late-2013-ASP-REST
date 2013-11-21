using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using REST_Service;
using System.Transactions;

namespace REST_Service_Unit_Test
{
    class TestInit
    {
        private TransactionScope _transactionScope;
        private readonly string _databaseName;

        public TestInit() {
            _databaseName = global::System.Configuration.ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
            EnsureDataExists();
        }

        public BookingSystemDataContext Context { get; private set; }

        public void EnsureDataExists()
        {
            Context = new BookingSystemDataContext(_databaseName);

            if (!Context.DatabaseExists())
                Context.CreateDatabase();

            _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transactionScope.Dispose();
        }
    }
}
