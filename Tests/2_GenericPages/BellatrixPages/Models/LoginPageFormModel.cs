using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bellatrix.Web;
using SeleniumVsBellatrixAutomatedTests.Tests.GenericPages.Helpers;

namespace SeleniumVsBellatrixAutomatedTests.Tests._2_GenericPages.BellatrixPages.Models
{
    public class LoginPageFormModel
    {
        [LabelName("Username or email address")]
        public string? Email { get; set; }

        [ControlType(typeof(Password))]
        public string? Password { get; set; }

        [ControlType(typeof(CheckBox))]
        [LabelName("Remember me")]
        public bool? RememberMe { get; set; }
    }
}
