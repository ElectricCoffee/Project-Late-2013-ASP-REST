using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Booking
    /// </summary>
    [Table(Name = "Booking")]
    public class Booking : IModel
    {
        private EntityRef<Subject> _subject;

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
        /// Gets or sets the value of the column StartTid
        /// </summary>
        [Column(
            Name = "StartTid",
            DbType = "DATETIME NOT NULL",
            CanBeNull = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the value of the column SlutTid
        /// </summary>
        [Column(
            Name = "SlutTid",
            DbType = "DATETIME NOT NULL",
            CanBeNull = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the value of the column Fag_id
        /// </summary>
        [Column(
            Name = "Fag_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the associated Subject entity
        /// </summary>
        [Association(
            Storage = "_subject",
            IsForeignKey = true,
            Name = "FK_Booking_Fag",
            ThisKey = "SubjectId",
            OtherKey = "Id")]
        public Subject Subject
        {
            get { return _subject.Entity; }
            set { _subject.Entity = value; }
        }
    }
}