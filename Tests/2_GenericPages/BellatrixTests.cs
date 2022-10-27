using Bellatrix.Web;
using Bellatrix.Web.NUnit;
using SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.BellatrixPages;
using SeleniumVsBellatrixAutomatedTests.Tests._2_GenericPages.BellatrixPages.Models;

namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages
{
    public class BellatrixTests : WebTest
    {
        #region credentials
        private const string password = "@purISQzt%%DYBnLCIhaoG6$";
        private const string email = "info@berlinspaceflowers.com";
        #endregion

        public DemosAccountPage DemosAccountPage => new DemosAccountPage();

        public override void TestInit()
        {
            base.TestInit();
            DemosAccountPage.Open();
        }

        [Test]
        public void SuccessfullyLoginToMyAccount()
        {
            var loginPageFormModel = new LoginPageFormModel()
            {
                Email = email,
                Password = password,
            };
            
            DemosAccountPage.SubmitForm(loginPageFormModel);

            DemosAccountPage.ValidateUserGreeting("Hello Berlin Spaceflowers");

            DemosAccountPage.LogoutLink.ValidateIsVisible();
            DemosAccountPage.LogoutLink.Click();

            DemosAccountPage.LogoutLink.ValidateIsNotVisible();
        }
    }
}