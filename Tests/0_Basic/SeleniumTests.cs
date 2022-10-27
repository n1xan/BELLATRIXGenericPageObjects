using Bellatrix.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Unity.Injection;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SeleniumVsBellatrixAutomatedTests.Tests.Basic
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
            var config = new ChromeConfig();
            config.GetMatchingBrowserVersion();
            new DriverManager().SetUpDriver(config);
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
            IWebElement userNameField = _webDriver.FindElement(By.Id("username"));
            userNameField.SendKeys(email);
            
            IWebElement passwordField = _webDriver.FindElement(By.Id("password"));
            passwordField.SendKeys(password);
            
            IWebElement loginButton = _webDriver.FindElement(By.XPath("//button[@name='login']"));
            loginButton.Click();


            IWebElement myAccountContentDiv = _webDriver.FindElement(By.ClassName("woocommerce-MyAccount-content"));
            Assert.That(myAccountContentDiv.Text, Contains.Substring("Hello Berlin Spaceflowers"), "Welcome info does not match expected data.");

            IWebElement logoutLink = _webDriver.FindElement(By.PartialLinkText("Log out"));
            Assert.IsTrue(logoutLink.Displayed, "LogoutLink is not visible");
            logoutLink.Click();

            Assert.That(_webDriver.Url.ToString(), Is.EqualTo("https://demos.bellatrix.solutions/my-account/"));
            // Assert Logout Link Not Visible
        }
    }
}