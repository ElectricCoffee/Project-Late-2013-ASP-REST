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
    /// Linq to Sql mapping for the table Hold
    /// </summary>
    [Table(Name = "Hold")]
    public class HomeRoomClass : IModel
    {
        EntitySet<Student> _students;
        EntityRef<HomeRoomSubject> _homeRoomSubject;

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
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the value of the column Navn
        /// </summary>
        [Column(
            Name = "Navn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated Student entityset
        /// </summary>
        [JsonIgnore]
        [Association(
            IsForeignKey = false,
            Name = "FK_Studerende_Hold",
            ThisKey = "Id",
            OtherKey = "HomeRoomClassId")]
        public EntitySet<Student> Students
        {
            get { return _students; }
            set { _students = value; }
        }

        /// <summary>
        /// Gets or sets the associated HomeRoomSubject entity
        /// </summary>
        [JsonIgnore]
        [Association(
            Storage = "_homeRoomSubject",
            IsForeignKey = false,
            Name = "HoldFag_Hold",
            ThisKey = "Id",
            OtherKey = "HomeRoomClassId")]
        public HomeRoomSubject HomeRoomSubject
        {
            get { return _homeRoomSubject.Entity; }
            set { _homeRoomSubject.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the associated HomeRoomClass entitySet
        /// </summary>
        [JsonIgnore]
        public EntitySet<Subject> Subjects
        {
            get
            {
                EnsureHomeRoomSubjectExists();
                return HomeRoomSubject.Subjects;
            }
            set
            {
                EnsureHomeRoomSubjectExists();
                HomeRoomSubject.Subjects = value;
            }
        }

        /// <summary>
        /// Makes sure that an instance of HomeRoomSubject Exists
        /// </summary>
        private void EnsureHomeRoomSubjectExists()
        {
            if (HomeRoomSubject == null)
                HomeRoomSubject = new HomeRoomSubject();
        }
    }
}