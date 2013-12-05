using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Semester
    /// </summary>
    [Table(Name = "Semester")]
    public class Semester : IModel
    {
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
    }
}