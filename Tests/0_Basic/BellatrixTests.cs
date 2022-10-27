using Bellatrix.Web;
using Bellatrix.Web.NUnit;

namespace SeleniumVsBellatrixAutomatedTests.Tests.Basic
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
            TextField userNameField = App.Components.CreateById<TextField>("username");
            Password passwordField = App.Components.CreateById<Password>("password");
            Button loginButton = App.Components.CreateByXpath<Button>("//button[@name='login']");

            userNameField.SetText(email);
            passwordField.SetPassword(password);
            loginButton.Click();

            Div myAccountContentDiv = App.Components.CreateByClass<Div>("woocommerce-MyAccount-content");
            myAccountContentDiv.ValidateInnerTextContains("Hello Berlin Spaceflowers");

            Anchor logoutLink = App.Components.CreateByInnerTextContaining<Anchor>("Log out");

            logoutLink.ValidateIsVisible();
            logoutLink.Click();
            App.Navigation.WaitForPartialUrl("/my-account/");

            // Assert Logout Link Not Visible
            logoutLink.ValidateIsNotVisible();
        }
    }
}