using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "HoldFag")]
    public class HomeRoomSubject : IModel
    {
        private EntitySet<HomeRoomClass> _homeRoomClasses;
        private EntitySet<Subject> _subjects;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "Hold_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int HomeRoomClassId { get; set; }

        [Association(
            IsForeignKey = true,
            Name = "HoldFag_Hold",
            ThisKey = "HomeRoomClassId",
            OtherKey = "Id")]
        public EntitySet<HomeRoomClass> HomeRoomClasses
        {
            get { return _homeRoomClasses; }
            set { _homeRoomClasses = value; }
        }

        [Column(
            Name = "Fag_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int SubjectId { get; set; }

        [Association(
            IsForeignKey = true,
            Name = "HoldFag_Fag",
            ThisKey = "SubjectId",
            OtherKey = "Id")]
        public EntitySet<Subject> Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }
    }
}