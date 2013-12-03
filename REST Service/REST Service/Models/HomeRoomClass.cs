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
    [Table(Name = "Hold")]
    public class HomeRoomClass : IModel
    {
        EntitySet<Student> _students;
        EntityRef<HomeRoomSubject> _homeRoomSubject;

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

        [JsonIgnore]
        [ScriptIgnore]
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

        private void EnsureHomeRoomSubjectExists()
        {
            if (HomeRoomSubject == null)
                HomeRoomSubject = new HomeRoomSubject();
        }
    }
}