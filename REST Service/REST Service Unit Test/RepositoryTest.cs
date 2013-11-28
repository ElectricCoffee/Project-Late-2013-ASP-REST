using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Models = REST_Service.Models;
using Repositories = REST_Service.Repositories;
using System.Linq;

namespace REST_Service_Unit_Test
{
    [TestClass]
    public class RepositoryTest : DataTest
    {
        [TestMethod]
        public void CreateUser()
        {
            var repo = new Repositories.UserRepository(_context);

            var name = new Models.Name
            {
                FirstName = "Kaj",
                LastName = "Bromose"
            };

            var user = new Models.User
            {
                Username = "kbr",
                Password = "1234",
                Name = name
            };

            repo.InsertOnSubmit(user);
            _context.SubmitChanges();

            var actual = (repo.SearchFor(u => u == user)).SingleOrDefault();

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(Models.User));
            Assert.AreEqual(actual, user);

            Assert.IsNotNull(actual.Name);
            Assert.IsInstanceOfType(actual.Name, typeof(Models.Name));
            Assert.AreEqual(name, actual.Name);
        }

        [TestMethod]
        public void CreateStudent()
        {
            var users = new Repositories.UserRepository(_context);
            var students = new Repositories.StudentRepository(_context);

            var name = new Models.Name
            {
                FirstName = "Trine",
                LastName = "Thune"
            };

            var student = new Models.Student
            {
                Username = "trin",
                Password = "1234",
                Approved = false,
                Name = name,
                HomeRoomClass = new Models.HomeRoomClass
                {
                    Name = "Dat12x"
                }
            };

            students.InsertOnSubmit(student);
            _context.SubmitChanges();

            var actualUser = (users.SearchFor(u => u.Id == student.UserId))
                .SingleOrDefault();
            var actualStudent = (students.SearchFor(s => s == student))
                .SingleOrDefault();

            Assert.IsNotNull(actualUser);
            Assert.IsInstanceOfType(actualUser, typeof(Models.User));
            
            Assert.IsNotNull(actualStudent);
            Assert.IsInstanceOfType(actualStudent, typeof(Models.Student));
            Assert.AreEqual(actualStudent, student);

            Assert.IsNotNull(actualStudent.Name);
            Assert.IsInstanceOfType(actualStudent.Name, typeof(Models.Name));
            Assert.AreEqual(name, actualStudent.Name);
        }
    }
}
