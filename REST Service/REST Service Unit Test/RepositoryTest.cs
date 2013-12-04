using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Models = REST_Service.Models;
using Repositories = REST_Service.Repositories;
using System.Linq;
using System.Collections.Generic;

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

            var actual = (repo.Single(u => u == user));

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
                HomeRoomClass = _context.HomeRoomClasses.SingleOrDefault(h => h.Name == "dat12x")
            };

            students.InsertOnSubmit(student);
            _context.SubmitChanges();

            var actualUser = (users.Single(u => u.Id == student.UserId));
            var actualStudent = (students.Single(s => s == student));

            Assert.IsNotNull(actualUser);
            Assert.IsInstanceOfType(actualUser, typeof(Models.User));
            
            Assert.IsNotNull(actualStudent);
            Assert.IsInstanceOfType(actualStudent, typeof(Models.Student));
            Assert.AreEqual(actualStudent, student);

            Assert.IsNotNull(actualStudent.Name);
            Assert.IsInstanceOfType(actualStudent.Name, typeof(Models.Name));
            Assert.AreEqual(name, actualStudent.Name);
        }

        [TestMethod]
        public void FindSubject()
        {
            var subjectName = "Android";

            var subjects = _context.Subjects;
            var actualSubject = subjects.SingleOrDefault(b => b.Name == subjectName);

            Assert.IsNotNull(actualSubject);
            Assert.IsInstanceOfType(actualSubject, typeof(Models.Subject));
            Assert.AreEqual(subjectName, actualSubject.Name);
        }

        [TestMethod]
        public void FindBooking()
        {
            var subjectName = "Android";

            var bookings = new Repositories.BookingRepository(_context);
            var actualBooking = bookings.Single(
                b => b.Subject.Name == subjectName);

            Assert.IsNotNull(actualBooking);
            Assert.IsInstanceOfType(actualBooking, typeof(Models.Booking));
            Assert.AreEqual(subjectName, actualBooking.Subject.Name);
        }

        [TestMethod]
        public void FindPossibleBooking()
        {
            var subjectName = "Android";

            var possibleBookings = new Repositories.PossibleBookingRepository(_context);
            var actualPossibleBooking = possibleBookings.Single(
                b => b.Subject.Name == subjectName);

            Assert.IsNotNull(actualPossibleBooking);
            Assert.IsInstanceOfType(actualPossibleBooking, typeof(Models.PossibleBooking));
            Assert.AreEqual(subjectName, actualPossibleBooking.Subject.Name);
        }

        [TestMethod]
        public void GetStudents()
        {
            var homeRoomClassName = "Dat12x";

            var students = new Repositories.StudentRepository(_context);
            var actualStudent = students.Single(s => s.HomeRoomClass.Name == homeRoomClassName);

            Assert.IsNotNull(actualStudent);
            Assert.IsInstanceOfType(actualStudent, typeof(Models.Student));
            Assert.AreEqual(homeRoomClassName, actualStudent.HomeRoomClass.Name);
        }

        [TestMethod]
        public void FindConcreteBooking()
        {
            var subjectName = "Android";

            var concreteBookings = new Repositories.ConcreteBookingRepository(_context);
            var actualConcreteBooking = concreteBookings.Single(b => b.Subject.Name == subjectName);

            Assert.IsNotNull(actualConcreteBooking);
            Assert.IsInstanceOfType(actualConcreteBooking, typeof(Models.ConcreteBooking));
            Assert.AreEqual(subjectName, actualConcreteBooking.Subject.Name);
        }
    }
}
