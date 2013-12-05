using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
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

        public Table<Models.Teacher> Teachers
        {
            get { return GetTable<Models.Teacher>(); }
        }

        public Table<Models.Administrator> Administrators
        {
            get { return GetTable<Models.Administrator>(); }
        }

        public Table<Models.Subject> Subjects
        {
            get { return GetTable<Models.Subject>(); }
        }

        public Table<Models.HomeRoomClass> HomeRoomClasses
        {
            get { return GetTable<Models.HomeRoomClass>(); }
        }

        public Table<Models.Semester> Semesters
        {
            get { return GetTable<Models.Semester>(); }
        }

        public Table<Models.HomeRoomSubject> HomeRoomSubjects
        {
            get { return GetTable<Models.HomeRoomSubject>(); }
        }

        public Table<Models.Booking> Bookings
        {
            get { return GetTable<Models.Booking>(); }
        }

        public Table<Models.ConcreteBooking> ConcreteBookings
        {
            get { return GetTable<Models.ConcreteBooking>(); }
        }

        public Table<Models.PossibleBooking> PossibleBookings
        {
            get { return GetTable<Models.PossibleBooking>(); }
        }

        public void SafeSubmitChanges(ref int errorCounter)
        {
            try
            {
                SubmitChanges();
            }

            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
                errorCounter++;
            }
        }

        public void SafeSubmitChanges()
        {
            try
            {
                SubmitChanges();
            }

            catch (Exception e)
            {
                Debug.WriteLine("DEBUG: " + e.Message);
            }
        }
    }
}