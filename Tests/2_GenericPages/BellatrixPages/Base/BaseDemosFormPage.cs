using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bellatrix.Web;
using SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers;

namespace SeleniumVsBellatrixAutomatedTests.Tests._2_GenericPages.BellatrixPages
{
    public class BaseDemosFormPage<TFormModel> : WebPage
         where TFormModel : class, new()
    {
        private readonly FormDataHandler<TFormModel> _formHandler;
        public override string Url => "https://demos.bellatrix.solutions/";

        public BaseDemosFormPage() => _formHandler = new FormDataHandler<TFormModel>();

        public virtual void FillForm(TFormModel item) => _formHandler.FillForm(item);

        public virtual void SubmitForm(TFormModel item)
        {
            FillForm(item);
            SubmitButton.Click();
        }

        public virtual TFormModel ExtractFormData() => _formHandler.ExtractFormData();

        public Button SubmitButton => App.Components.CreateByNameEndingWith<Button>("login");
    }
}
