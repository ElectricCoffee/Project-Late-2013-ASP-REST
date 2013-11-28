using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    public enum UserType
    {
        Student,
        Teacher,
        Administrator
    }

    /// <summary>
    /// Linq to Sql mapping for the table Bruger
    /// </summary>
    [Table(Name = "dbo.Bruger")]
    public class User : IModel
    {
        // private Name _name;
        private EntityRef<Models.Name> _name;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        [Column(
            Name = "Brugernavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string Username { get; set; }

        [Column(
            Name = "Password",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public string Password { get; set; }

        [Column(
            Name = "Navn_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int NameId { get; set; }

        [Association(
            IsForeignKey = true,
            Name = "Bruger_Navn",
            ThisKey = "NameId")]
        public Name Name
        {
            get { return _name.Entity; }
            set { _name.Entity = value; }
        }
    }
}