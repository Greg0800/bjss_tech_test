using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class dresses_page
    {
        private IWebDriver driver;

        [FindsBy(How = How.ClassName, Using = "product_list")]
        public IList<IWebElement> product_list { get; set; }

        [FindsBy(How = How.ClassName, Using = "quick-view")]
        public IWebElement quick_view { get; set; }

    }
}
