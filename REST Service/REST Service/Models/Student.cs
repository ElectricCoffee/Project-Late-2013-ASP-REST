using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Web;

namespace REST_Service.Models
{
    [Table(Name = "dbo.Studerende")]
    public class Student
    {
        // private HomeRoomClass homeRoomClass;

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL PRIMARY KEY IDENTITY",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        public int Id { get; private set; }

        public Name Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

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

        private byte ApprovedNum;

        public string HomeRoomClass { get; set; }

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        [Association(
            IsForeignKey = true,
            Name = "FK_Studerende_Bruger",
            ThisKey = "Bruger_id",
            OtherKey = "_id")]
        private int HomeRoomClassId { get; set; }

        [Column(
            Name = "_id",
            DbType = "INT NOT NULL",
            AutoSync = AutoSync.OnInsert,
            CanBeNull = false,
            IsPrimaryKey = true,
            IsDbGenerated = true)]
        [Association(
            IsForeignKey = true,
            Name = "FK_Studerende_Bruger",
            ThisKey = "Bruger_id",
            OtherKey = "_id")]
        public int UserId { get; set; }
    }
}