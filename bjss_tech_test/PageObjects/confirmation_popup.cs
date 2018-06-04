using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class confirmation_popup
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='layer_cart']/div[1]/div[1]/h2")]
        public IWebElement product_added { get; set; }

        [FindsBy(How = How.Id, Using = "layer_cart_product_attributes")]
        public IWebElement colour_size { get; set; }

        [FindsBy(How = How.Id, Using = "layer_cart_product_title")]
        public IWebElement product_title { get; set; }

        [FindsBy(How = How.Id, Using = "layer_cart_product_quantity")]
        public IWebElement quantity { get; set; }

        [FindsBy(How = How.Id, Using = "layer_cart_product_price")]
        public IWebElement value { get; set; }
        

    }
}
