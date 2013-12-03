using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.Booking")]
    public class Booking : IModel
    {
        private EntityRef<Subject> _subject;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "StartTid",
            DbType = "DATETIME NOT NULL",
            CanBeNull = false)]
        public DateTime StartTime { get; set; }

        [Column(
            Name = "SlutTid",
            DbType = "DATETIME NOT NULL",
            CanBeNull = false)]
        public DateTime EndTime { get; set; }

        [Column(
            Name = "Fag_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int SubjectId { get; set; }

        [Association(
            IsForeignKey = true,
            Name = "FK_Booking_Fag",
            ThisKey = "SubjectId")]
        public EntityRef<Subject> Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
    }
}