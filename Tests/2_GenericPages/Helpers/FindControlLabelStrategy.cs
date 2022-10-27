using Bellatrix.Web;

namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers
{
    public class FindControlLabelStrategy : FindStrategy
    {
        private readonly string _xpathSuffix;
        private readonly string _xpathPrefix;

        public FindControlLabelStrategy(string value, string xpathSuffix, string? xpathPrefix = null)
            : base(value)
        {
            _xpathSuffix = xpathSuffix;
            _xpathPrefix = xpathPrefix ?? string.Empty;
        }

        public override OpenQA.Selenium.By Convert()
        {
            var xpath = $"{_xpathPrefix}//*[@id=(//label[contains(text(), '{Value}')]/@for)]{_xpathSuffix}";
            return OpenQA.Selenium.By.XPath(xpath);
        }

        public override string ToString() => $"Control Label = {Value}";
    }
}