using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class authentication_page
    {

        [FindsBy(How = How.CssSelector, Using = "#center_column > h1")]
        public IWebElement page_heading { get; set; }

        [FindsBy(How = How.Name, Using = "email")]
        public IWebElement email { get; set; }

        [FindsBy(How = How.Name, Using = "passwd")]
        public IWebElement password { get; set; }

        [FindsBy(How = How.Id, Using = "SubmitLogin")]
        public IWebElement login_button { get; set; }
    }
}
