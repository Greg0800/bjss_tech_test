using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class basket_page
    {

        [FindsBy(How = How.Id, Using = "total_product")]
        public IWebElement total_products_price { get; set; }

        [FindsBy(How = How.Id, Using = "total_shipping")]
        public IWebElement total_shipping { get; set; }

        [FindsBy(How = How.Id, Using = "total_price_without_tax")]
        public IWebElement total_pre_tax { get; set; }

        [FindsBy(How = How.Id, Using = "total_tax")]
        public IWebElement tax { get; set; }

        [FindsBy(How = How.Id, Using = "total_price")]
        public IWebElement total_inc_tax { get; set; }

        public Dictionary<String, String>[] Get_cart_items(IWebDriver driver)
        {
            //Check the product details
            Decimal total_of_cart_items = 0;
            IWebElement product_Table = driver.FindElement(By.TagName("tbody"));
            IList<IWebElement> product_Rows = product_Table.FindElements(By.TagName("tr"));
            IList<IWebElement> product_columns = product_Table.FindElements(By.TagName("td"));
            String Table_Rows_And_Columns = "Rows :" + product_Rows.Count + " Columns : " + product_columns.Count;
            String regex = "(?<=\\$)[^?]*";
            int x = 0;
            Dictionary<String, String>[] _product_list = new Dictionary<String, String>[product_Rows.Count];
            while (x < product_Rows.Count)
            {
                String product = product_Rows[x].Text.Replace("\r\n", "\n");
                string[] order = product.Split('\n');
                String product_title = order[0];
                String colour_size = order[2];
                String size = colour_size.Substring(colour_size.Length - 1, 1);
                regex = "(?<=Color : )[^,?]*";
                String colour = Regex.Match(colour_size, regex).Value;
                String unit_price_text = order[3];
                regex = "(?<=\\$)[^?]*";
                String unit_price = Regex.Match(unit_price_text, regex).Value;
                String total_price = order[4].Substring(1, order[4].Length - 1);
                total_of_cart_items = total_of_cart_items + Decimal.Parse(total_price);
                Decimal _quantity = Decimal.Parse(total_price) / Decimal.Parse(unit_price);
                String quantity = _quantity.ToString();
                item_in_cart_dict item_in_cart = new item_in_cart_dict();
                Dictionary<string, string> _dict = item_in_cart.item_to_dictionary(product_title, colour, size, quantity, total_price);
                _product_list[x] = _dict;
                x++;
            }

            //Check the price details

            String total_products_price_string = Regex.Match(total_products_price.Text, regex).Value;
            String total_shipping_string = Regex.Match(total_shipping.Text, regex).Value;
            String total_tax_string = Regex.Match(tax.Text, regex).Value;
            String total_inc_tax_string = Regex.Match(total_inc_tax.Text, regex).Value;

            Assert.AreEqual(total_of_cart_items, Decimal.Parse(total_products_price_string));

            Decimal calculated_total = Decimal.Parse(total_products_price_string) + Decimal.Parse(total_shipping_string) + Decimal.Parse(total_tax_string);

            Assert.AreEqual(total_inc_tax_string, calculated_total.ToString());

            return _product_list;
        }
    }
}
