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
    [Table(Name = "Fag")]
    public class Subject : IModel
    {
        private EntityRef<Semester> _semester;
        private EntityRef<Teacher> _teacher;
        private EntityRef<HomeRoomSubject> _homeRoomSubject;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "Navn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string Name { get; set; }

        [Column(
            Name = "Semester_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int SemesterId { get; set; }

        //[JsonIgnore]
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

        [Column(
            Name = "Lærer_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int TeacherId { get; set; }

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

        [JsonIgnore]
        [Association(
            IsForeignKey = false,
            Name = "FK_Booking_Fag",
            ThisKey = "Id",
            OtherKey = "SubjectId")]
        public EntitySet<Booking> Bookings { get; set; }

        private void EnsureHomeRoomSubjectExists()
        {
            if (HomeRoomSubject == null)
                HomeRoomSubject = new HomeRoomSubject();
        }
    }
}