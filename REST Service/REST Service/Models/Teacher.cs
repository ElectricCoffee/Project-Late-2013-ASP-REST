using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.Lærer")]
    public class Teacher : IModel
    {
        private EntityRef<User> _user;
        private EntitySet<Subject> _subjects;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        public Name Name
        {
            get { return _user.Entity.Name; }
            set
            {
                if (_user.Entity == null)
                    _user.Entity = new User();
                _user.Entity.Name = value;
            }
        }

        public string Username
        {
            get
            {
                return _user.Entity.Username;
            }
            set
            {
                if (_user.Entity == null)
                    _user.Entity = new User();
                _user.Entity.Username = value;
            }
        }

        public string Password
        {
            get
            {
                return _user.Entity.Password;
            }
            set
            {
                if (_user.Entity == null)
                    _user.Entity = new User();
                _user.Entity.Password = value;
            }
        }

        [Column(
            Name = "Bruger_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int UserId { get; set; }

        [Association(
            IsForeignKey = true,
            Name = "Lærer_Bruger",
            ThisKey = "UserId")]
        private User User
        {
            get { return _user.Entity; }
            set { _user.Entity = value; }
        }

        [Association(
            IsForeignKey = true,
            Name = "Fag_Lærer",
            ThisKey = "Id")]
        public EntitySet<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }
    }
}