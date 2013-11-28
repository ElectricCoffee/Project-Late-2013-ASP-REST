﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.Fag")]
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

        [Association(
            IsForeignKey = true,
            Name = "Fag_Semester",
            ThisKey = "SemesterId")]
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
            IsForeignKey = true,
            Name = "Fag_Lærer",
            ThisKey = "TeacherId")]
        public Teacher Teacher
        {
            get { return _teacher.Entity; }
            set { _teacher.Entity = value; }
        }

        [Association(
            IsForeignKey = true,
            Name = "HoldFag_Fag",
            ThisKey = "Id")]
        public HomeRoomSubject HomeRoomSubject
        {
            get { return _homeRoomSubject.Entity; }
            set { _homeRoomSubject.Entity = value; }
        }

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

        [Association(
            IsForeignKey = true,
            Name = "FK_Booking_Fag",
            ThisKey = "Id")]
        public EntitySet<Booking> Bookings { get; set; }

        private void EnsureHomeRoomSubjectExists()
        {
            if (_homeRoomSubject.Equals(null))
                _homeRoomSubject = new EntityRef<HomeRoomSubject>();
        }
    }
}