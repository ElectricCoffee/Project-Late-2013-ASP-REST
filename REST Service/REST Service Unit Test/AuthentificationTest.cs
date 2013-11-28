using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

using Rest = REST_Service;
using System.Collections.Generic;

namespace REST_Service_Unit_Test
{
    /// <summary>
    /// Defines a set of tests for the authentification
    /// </summary>
    [TestClass]
    public class AuthentificationTest : DataTest
    {
        /// <summary>
        /// Inserts a known user into the test database
        /// </summary>
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            _context.Users.Attach(
                new Rest.Models.User
                {
                    Username = "johndoe",
                    Password = "12345678"
                }
            );
        }

        /// <summary>
        /// Tests a succesful authentification
        /// </summary>
        [TestMethod, Description("An accesstoken should be received if succesful")]
        public void Authentification_Succesful()
        {
            var expectedKey = "AccessToken";

            var loginCtrl = new Rest.Controllers.LoginController();
            var actual = loginCtrl.Get("johndoe", "12345678");

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(Dictionary<string, string>));
            Assert.IsNotNull(actual.Key);
            Assert.AreEqual(expectedKey, actual.Key);
            Assert.IsNotNull(actual.Value);
        }

        /// <summary>
        /// Tests an unsuccesful authentification
        /// </summary>
        [TestMethod]
        public void Authentification_Unsuccesful()
        {
            var expectedKey = "error";
            var expectedValue = "";

            var loginCtrl = new Rest.Controllers.LoginController();
            var actual = loginCtrl.Get("johndoe", "12345678");

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, typeof(Dictionary<string, string>));
            Assert.IsNotNull(actual.Key);
            Assert.AreEqual(expectedKey, actual.Key);
            Assert.IsNotNull(actual.Value);
            Assert.AreEqual(expectedValue, actual.Value);
        }
    }
}
