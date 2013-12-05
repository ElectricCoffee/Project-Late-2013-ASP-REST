using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    /// <summary>
    /// Linq to Sql mapping for the table Studerende
    /// </summary>
    [Table(Name = "Studerende")]
    public class Student : User
    {
        private EntityRef<User> _user;
        private EntityRef<HomeRoomClass> _homeRoomClass;

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
        public override int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the associated Name entity
        /// </summary>
        public override Name Name
        {
            get { return User.Name; }
            set
            {
                EnsureUserExists();
                User.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Brugernavn of the disjointed table Bruger
        /// </summary>
        public override string Username {
            get { return User.Username; }
            set
            {
                EnsureUserExists();
                User.Username = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Password of the disjointed table Bruger
        /// </summary>
        public override string Password
        {
            get { return User.Password; }
            set
            {
                EnsureUserExists();
                User.Password = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Godkendt
        /// </summary>
        [Column(
            Name = "Godkendt",
            DbType = "TINYINT NOT NULL",
            CanBeNull = false)]
        private byte ApprovedNum { get; set; }

        /// <summary>
        /// Gets or sets the associated value of the column Godkendt
        /// </summary>
        public bool Approved
        {
            get
            {
                if (ApprovedNum == 1)
                    return true;
                else
                    return false;
            }
            set {
                if (value)
                    ApprovedNum = 1;
                else
                    ApprovedNum = 0;
            }
        }

        /// <summary>
        /// Gets or sets the value of the column Hold_id
        /// </summary>
        [Column(
            Name = "Hold_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        private int HomeRoomClassId { get; set; }

        /// <summary>
        /// Gets or sets the associated HomeRoomClass entity
        /// </summary>
        [Association(
            Storage = "_homeRoomClass",
            IsForeignKey = true,
            Name = "FK_Studerende_Hold",
            ThisKey = "HomeRoomClassId",
            OtherKey = "Id")]
        public HomeRoomClass HomeRoomClass
        {
            get { return _homeRoomClass.Entity; }
            set { _homeRoomClass.Entity = value; }
        }

        /// <summary>
        /// Gets or sets the value of the column Bruger_id
        /// </summary>
        [Column(
            Name = "Bruger_id",
            DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the associated User entity
        /// </summary>
        [Association(
            Storage = "_user",
            IsForeignKey = true,
            Name = "FK_Studerende_Bruger",
            ThisKey = "UserId",
            OtherKey = "Id")]
        private User User
        {
            get { return _user.Entity; }
            set { _user.Entity = value; }
        }

        /// <summary>
        /// Makes sure that an instance of User Exists
        /// </summary>
        private void EnsureUserExists()
        {
            if (User == null)
                User = new User();
        }
    }
}