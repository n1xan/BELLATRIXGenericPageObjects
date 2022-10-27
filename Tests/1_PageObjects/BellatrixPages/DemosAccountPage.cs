using Bellatrix.Web;

namespace SeleniumVsBellatrixAutomatedTests.Tests.PageObjects.BellatrixPages
{
    public class DemosAccountPage_Labels : WebPage
    {
        public override string Url => "https://demos.bellatrix.solutions/my-account/";
        public TextField UserNameField => App.Components.CreateByLabel<TextField>("Username or email address");
        public Password PasswordField => App.Components.CreateByLabel<Password>("Password");

        public Button LoginButton => App.Components.CreateByXpath<Button>("//button[@name='login']");
        public Div MyAccountContentDiv => App.Components.CreateByClass<Div>("woocommerce-MyAccount-content");
        public Anchor LogoutLink => App.Components.CreateByInnerTextContaining<Anchor>("Log out");
    }
}