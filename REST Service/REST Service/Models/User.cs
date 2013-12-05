using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Bruger
    /// </summary>
    [Table(Name = "Bruger")]
    public class User : IModel
    {
        private EntityRef<Models.Name> _name;

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
        public virtual int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the value of the column Brugernavn
        /// </summary>
        [Column(
            Name = "Brugernavn",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public virtual string Username { get; set; }

        /// <summary>
        /// Gets or sets the value of the column Password
        /// </summary>
        [Column(
            Name = "Password",
            DbType = "VARCHAR(50) NOT NULL",
            CanBeNull = false)]
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the value of the column Navn_id
        /// </summary>
        [Column(
            Name = "Navn_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int NameId { get; set; }

        /// <summary>
        /// Gets or sets the associated Name entity
        /// </summary>
        [Association(
            Storage = "_name",
            IsForeignKey = true,
            Name = "Bruger_Navn",
            ThisKey = "NameId",
            OtherKey = "Id")]
        public virtual Name Name
        {
            get { return _name.Entity; }
            set { _name.Entity = value; }
        }
    }
}