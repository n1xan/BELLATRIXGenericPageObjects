using OpenQA.Selenium;

namespace SeleniumVsBellatrixAutomatedTests.Tests.PageObjects.SeleniumPages
{
    public class DemosAccountPage
    {
        private IWebDriver _webDriver;
        public DemosAccountPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public IWebElement UserNameField => _webDriver.FindElement(By.Id("username"));
        public IWebElement PasswordField => _webDriver.FindElement(By.Id("password"));
        public IWebElement LoginButton => _webDriver.FindElement(By.XPath("//button[@name='login']"));
        public IWebElement MyAccountContentDiv => _webDriver.FindElement(By.ClassName("woocommerce-MyAccount-content"));
        public IWebElement LogoutLink => _webDriver.FindElement(By.PartialLinkText("Log out"));
    }
}