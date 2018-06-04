using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class home_page
    {
        private IWebDriver driver;

        [FindsBy(How = How.ClassName, Using = "login")]
        public IWebElement sign_in { get; set; }

    }
}