using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TechTalk.SpecFlow;

namespace bjss_tech_test.PageObjects
{
    class site_wide_objects
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='block_top_menu']/ul/li[1]/a")]
        public IWebElement women { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='block_top_menu']/ul/li[2]/a")]
        public IWebElement dresses { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='block_top_menu']/ul/li[3]/a")]
        public IWebElement tshirts { get; set; }

        public IWebElement get_image(int chosen_item, IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //String chosen_xpath = "# center_column > ul > li:nth-child(" + chosen_item + ") > div > div.left-block > div > a.quick-view"
            List<IWebElement> image_list = driver.FindElements(By.ClassName("product-image-container")).ToList();
            //chosen_item--;
            return image_list[chosen_item];
        }

        public IWebElement quick_view_item(int chosen_item, IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //String chosen_xpath = "# center_column > ul > li:nth-child(" + chosen_item + ") > div > div.left-block > div > a.quick-view"
            List<IWebElement> products_list = driver.FindElements(By.ClassName("quick-view-mobile")).ToList();
            //chosen_item--;
            return products_list[chosen_item];
        }

        public List<IWebElement> get_product_name(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //String chosen_xpath = "# center_column > ul > li:nth-child(" + chosen_item + ") > div > div.left-block > div > a.quick-view"
            List<IWebElement> product_name_list = driver.FindElements(By.XPath("//*[contains(@class,'right-block')]")).ToList();

            //chosen_item--;
            return product_name_list;
            //*[@id="center_column"]/ul/li[1]/div/div[2]/h5/a
        }



        public static Dictionary<string, string> ToDictionary(Table table)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }
            return dictionary;
        }
    }
}
