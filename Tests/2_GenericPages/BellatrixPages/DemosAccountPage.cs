using Bellatrix.Web;
using SeleniumVsBellatrixAutomatedTests.Tests._2_GenericPages.BellatrixPages;
using SeleniumVsBellatrixAutomatedTests.Tests._2_GenericPages.BellatrixPages.Models;

namespace SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.BellatrixPages
{
    public class DemosAccountPage: BaseDemosFormPage<LoginPageFormModel>
    {
        public override string Url => base.Url + "/my-account/";

        // Element mappings
        public Div MyAccountContentDiv => App.Components.CreateByClass<Div>("woocommerce-MyAccount-content");
        public Anchor LogoutLink => App.Components.CreateByInnerTextContaining<Anchor>("Log out");


        // Validations
        public void ValidateUserGreeting(string expectedGreeting)
        {
            MyAccountContentDiv.ValidateInnerTextContains(expectedGreeting);
        }
    }
}