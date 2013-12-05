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
    /// Linq to Sql mapping for the table Fag
    /// </summary>
    [Table(Name = "Fag")]
    public class Subject : IModel
    {
        private EntityRef<Semester> _semester;
        private EntityRef<Teacher> _teacher;
        private EntityRef<HomeRoomSubject> _homeRoomSubject;

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
        /// Gets or sets the value of the column Semester_id
        /// </summary>
        [Column(
            Name = "Semester_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int SemesterId { get; set; }

        /// <summary>
        /// Gets or sets the associated Semester entity
        /// </summary>
        [Association(
            Storage = "_semester",
            IsForeignKey = true,
            Name = "Fag_Semester",
            ThisKey = "SemesterId",
            OtherKey = "Id")]
        public Semester Semester
        {
            get { return _semester.Entity; }
            set { _semester.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the value of the column Lærer_id
        /// </summary>
        [Column(
            Name = "Lærer_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the associated Teacher entity
        /// </summary>
        [Association(
            Storage = "_teacher",
            IsForeignKey = true,
            Name = "Fag_Lærer",
            ThisKey = "TeacherId",
            OtherKey = "Id")]
        public Teacher Teacher
        {
            get { return _teacher.Entity; }
            set { _teacher.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the associated HomeRoomSubject entity
        /// </summary>
        [JsonIgnore]
        [Association(
            Storage = "_homeRoomSubject",
            Name = "HoldFag_Fag",
            ThisKey = "Id",
            OtherKey = "SubjectId")]
        public HomeRoomSubject HomeRoomSubject
        {
            get { return _homeRoomSubject.Entity; }
            set { _homeRoomSubject.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the associated HomeRoomClass entitySet
        /// </summary>
        [JsonIgnore]
        public EntitySet<HomeRoomClass> HomeRoomClasses
        {
            get
            {
                EnsureHomeRoomSubjectExists();
                return HomeRoomSubject.HomeRoomClasses;
            }
            set
            {
                EnsureHomeRoomSubjectExists();
                HomeRoomSubject.HomeRoomClasses = value;
            }
        }

        /// <summary>
        /// Gets or sets the associated Booking entityset
        /// </summary>
        [JsonIgnore]
        [Association(
            IsForeignKey = false,
            Name = "FK_Booking_Fag",
            ThisKey = "Id",
            OtherKey = "SubjectId")]
        public EntitySet<Booking> Bookings { get; set; }

        /// <summary>
        /// Makes sure that an instance of HomeRoomSubject exists
        /// </summary>
        private void EnsureHomeRoomSubjectExists()
        {
            if (HomeRoomSubject == null)
                HomeRoomSubject = new HomeRoomSubject();
        }
    }
}