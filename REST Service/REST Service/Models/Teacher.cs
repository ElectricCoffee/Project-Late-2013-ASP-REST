using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Lærer
    /// </summary>
    [Table(Name = "Lærer")]
    public class Teacher : IModel
    {
        private EntityRef<User> _user;
        private EntitySet<Subject> _subjects;

        /// <summary>
        /// Gets the value of the column _id
        /// </summary>
        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the associated Name entity
        /// </summary>
        public Name Name
        {
            get
            {
                EnsureUserExists();
                return User.Name;
            }
            set
            {
                EnsureUserExists();
                User.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Brugernavn of the disjointed table Bruger
        /// </summary>
        public string Username
        {
            get { return User.Username; }
            set
            {
                EnsureUserExists();
                User.Username = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Password of the disjointed table Bruger
        /// </summary>
        public string Password
        {
            get { return User.Password; }
            set
            {
                EnsureUserExists();
                User.Password = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Bruger_id
        /// </summary>
        [Column(
            Name = "Bruger_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the associated User entity
        /// </summary>
        [Association(
            Storage = "_user",
            IsForeignKey = true,
            Name = "Lærer_Bruger",
            ThisKey = "UserId",
            OtherKey = "Id")]
        private User User
        {
            get { return _user.Entity; }
            set { _user.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the associated Subjects entityset
        /// </summary>
        [ScriptIgnore][JsonIgnore]
        [Association(
            IsForeignKey = false,
            Name = "Fag_Lærer",
            ThisKey = "Id",
            OtherKey = "TeacherId")]
        public EntitySet<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }

        /// <summary>
        /// Makes sure that an instance of User Exists
        /// </summary>
        private void EnsureUserExists()
        {
            if (User == null)
                User = new User();
        }
    }
}