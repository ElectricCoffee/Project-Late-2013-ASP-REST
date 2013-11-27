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
    [Table(Name = "dbo.Navn")]
    public class Name
    {
        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "Fornavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string FirstName { get; set; }

        [Column(
            Name = "Efternavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string LastName { get; set; }
    }
}