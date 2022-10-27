using Bellatrix.Web;
using Bellatrix.Web.NUnit;
using SeleniumVsBellatrixAutomatedTests.Tests.PageObjects.BellatrixPages;

namespace SeleniumVsBellatrixAutomatedTests.Tests.PageObjects
{
    public class BellatrixTests : WebTest
    {
        #region credentials
        private const string password = "@purISQzt%%DYBnLCIhaoG6$";
        private const string email = "info@berlinspaceflowers.com";
        #endregion

        public override void TestInit()
        {
            base.TestInit();
            App.Navigation.Navigate("https://demos.bellatrix.solutions/my-account/");
        }

        [Test]
        public void SuccessfullyLoginToMyAccount()
        {
            DemosAccountPage accountPage = new DemosAccountPage();
            accountPage.UserNameField.SetText(email);
            accountPage.PasswordField.SetPassword(password);
            accountPage.LoginButton.Click();

            accountPage.MyAccountContentDiv.ValidateInnerTextContains("Hello Berlin Spaceflowers");

            accountPage.LogoutLink.ValidateIsVisible();
            accountPage.LogoutLink.Click();

            accountPage.AssertUrlPath("/my-account/");

            // Assert Logout Link Not Visible
            accountPage.LogoutLink.ValidateIsNotVisible();
        }
    }
}