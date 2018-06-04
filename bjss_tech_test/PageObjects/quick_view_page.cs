using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class quick_view_page
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = "//*[@id='product']/div/div/div[2]/h1")]
        public IWebElement page_heading { get; set; }

        [FindsBy(How = How.Id, Using = "quantity_wanted")]
        public IWebElement quantity { get; set; }

        [FindsBy(How = How.Id, Using = "group_1")]
        public IWebElement size { get; set; }

        [FindsBy(How = How.Id, Using = "add_to_cart")]
        public IWebElement add_to_cart_button { get; set; }
    }
}
