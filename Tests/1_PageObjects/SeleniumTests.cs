using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumVsBellatrixAutomatedTests.Tests.PageObjects.SeleniumPages;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumVsBellatrixAutomatedTests.Tests.BasPageObjectsic
{
    public class SeleniumTests
    {
        #region credentials
        private const string password = "@purISQzt%%DYBnLCIhaoG6$";
        private const string email = "info@berlinspaceflowers.com";
        #endregion

        private WebDriver _webDriver;

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();
            _webDriver.Navigate().GoToUrl("https://demos.bellatrix.solutions/my-account/");
        }

        [TearDown]
        public void TestCleanup()
        {
            _webDriver.Close();
        }

        [Test]
        public void SuccessfullyLoginToMyAccount()
        {
            DemosAccountPage accountPage = new DemosAccountPage(_webDriver);

            accountPage.UserNameField.SendKeys(email);
            accountPage.PasswordField.SendKeys(password);
            accountPage.LoginButton.Click();

            Assert.That(accountPage.MyAccountContentDiv.Text, Contains.Substring("Hello Berlin Spaceflowers"), "Welcome info does not match expected data.");

            Assert.IsTrue(accountPage.LogoutLink.Displayed, "LogoutLink is not visible");
            accountPage.LogoutLink.Click();

            Assert.That(_webDriver.Url.ToString(), Is.EqualTo("https://demos.bellatrix.solutions/my-account/"));

            #region AssertLogoutLinkNotVisible
            // Assert.That(accountPage.LogoutLink.Displayed, Is.False);
            // Throws OpenQA.Selenium.StaleElementReferenceException : stale element reference: element is not attached to the page document
            #endregion
        }
    }
}