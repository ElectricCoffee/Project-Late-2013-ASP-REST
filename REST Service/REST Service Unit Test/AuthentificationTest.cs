using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

using Rest = REST_Service;

namespace REST_Service_Unit_Test
{
    [TestClass]
    public class AuthentificationTest
    {
        private TestInit _init;

        [TestInitialize]
        public void Init()
        {
            _init = new TestInit();
            _init.Context.Users.Attach(
                new Rest.User
                {
                    Username = "johndoe",
                    Password = "12345678"
                }
            );
        }

        [TestMethod, Description("An accesstoken should be received if succesful")]
        public void Authentification_Succesful()
        {
            var expected = "";
            var loginCtrl = new Rest.Controllers.LoginController();
            var actual = loginCtrl.Get("johndoe", "12345678");

        }

        [TestMethod]
        public void Authentification_Unsuccesful()
        {
            
        }
    }
}
