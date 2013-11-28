using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace REST_Service.DataLayer
{
    public class ManualBookingSystemDataContext : DataContext
    {
        private const string DEFAULT_CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Jacob\Documents\GitHub\Project-Late-2013-ASP-REST\REST Service\REST Service\App_Data\DbTest.mdf;Integrated Security=True";
        
        public ManualBookingSystemDataContext() : this(DEFAULT_CONNECTION_STRING) { }

        public ManualBookingSystemDataContext(string connectionString) : base(connectionString) { }

        public Table<Models.Name> Names
        {
            get { return GetTable<Models.Name>(); }
        }

        public Table<Models.User> Users
        {
            get { return GetTable<Models.User>(); }
        }

        public Table<Models.Student> Students
        {
            get { return GetTable<Models.Student>(); }
        }
    }
}