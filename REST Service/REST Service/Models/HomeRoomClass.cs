﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.Hold")]
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
            IsForeignKey = true,
            Name = "FK_Studerende_Hold",
            ThisKey = "Id")]
        public EntitySet<Student> Student
        {
            get { return _students; }
            set { _students = value; }
        }

        [Association(
            IsForeignKey = true,
            Name = "HoldFag_Hold",
            ThisKey = "Id")]
        public HomeRoomSubject HomeRoomSubject
        {
            get { return _homeRoomSubject.Entity; }
            set { _homeRoomSubject.Entity = value; }
        }

        public EntitySet<Subject> Subjects
        {
            get
            {
                if (_homeRoomSubject.Equals(null))
                    _homeRoomSubject = new EntityRef<HomeRoomSubject>();
                return HomeRoomSubject.Subjects;
            }
            set
            {
                if (_homeRoomSubject.Equals(null))
                    _homeRoomSubject = new EntityRef<HomeRoomSubject>();
                HomeRoomSubject.Subjects = value;
            }
        }
    }
}