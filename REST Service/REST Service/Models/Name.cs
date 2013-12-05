using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Navn
    /// </summary>
    [Table(Name = "Navn")]
    public class Name
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
        /// Gets or sets the value of the column Fornavn
        /// </summary>
        [Column(
            Name = "Fornavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string FirstName { get; set; }


        /// <summary>
        /// Gets the value of the column Efternavn
        /// </summary>
        [Column(
            Name = "Efternavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string LastName { get; set; }
    }
}