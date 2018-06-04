using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium.Support.PageObjects;
using bjss_tech_test.PageObjects;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using System.Drawing.Imaging;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace bjss_tech_test
{
    [Binding]
    public class item_purchase_step_defs
    {
        private EventFiringWebDriver driver;
        private IWebElement product_name;
        public List<IWebElement> product_name_list;
        String input_text_for_message = "";

        Dictionary<String, String>[] dicts = new Dictionary<string, string>[2];
        public int dict_counter = 0;

        [BeforeScenario("purchase")]
        public void InitializeChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("disable-infobars");
            //options.AddArgument("--start-fullscreen");
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1280x1024");
            //options.AddArgument("--disable-gpu");

            driver = new EventFiringWebDriver(new ChromeDriver(options));
            driver.ExceptionThrown += firingDriver_TakeScreenshotOnException;
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
        }

        private void firingDriver_TakeScreenshotOnException(object sender, WebDriverExceptionEventArgs e)
        {
            string message = "Exception:- " + DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
            var screenshot = driver.TakeScreenshot();
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile("message"+".png", ScreenshotImageFormat.Png);
        }

        [AfterScenario("purchase")]
        public void logout()
        {
            Thread.Sleep(1000);
            IWebElement sign_out_button = driver.FindElement(By.ClassName("logout"));
            sign_out_button.Click();
            IWebElement page_heading = driver.FindElement(By.CssSelector("#center_column > h1"));
            Assert.AreEqual("AUTHENTICATION", page_heading.Text);

            driver.Quit();
        }

        [Given(@"I have logged in to the site with email ""(.*)"" and password ""(.*)""")]
        public void GivenIHaveLoggedInToTheSiteWithEmailAndPassword( string eml, string pswd)
        {
            //WebDriverWait webdriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Url = "http://automationpractice.com/index.php?controller=authentication&back=my-account";
            authentication_page auth_page = new authentication_page();
            PageFactory.InitElements(driver, auth_page);
            var heading_text = auth_page.page_heading;
            var email_input = auth_page.email;
            var password_input = auth_page.password;
            var login_button = auth_page.login_button;
            Assert.IsTrue(heading_text.Text.Contains("AUTHENTICATION"), heading_text + " doesn't contain AUTHENTICATION as expected");
            email_input.SendKeys(eml);
            password_input.SendKeys(pswd);
            login_button.Click();
            Assert.IsTrue(heading_text.Text.Contains("MY ACCOUNT"), heading_text + " doesn't contain MY ACCOUNT as expected");
        }
        
        [Given(@"I navigate to the ""(.*)"" navbar item")]
        public void GivenINavigateToThePage(string chosen_navbar_item)
        {
            site_wide_objects site_wide = new site_wide_objects();
            PageFactory.InitElements(driver, site_wide);
            var women_link = site_wide.women;
            var dresses_link = site_wide.dresses;
            var tshirts_link = site_wide.tshirts;

            switch(chosen_navbar_item.ToLower())
            {
                case "women":
                    women_link.Click();
                    product_name_list = site_wide.get_product_name(driver);
                    break;
                case "dresses":
                    dresses_link.Click();
                    product_name_list = site_wide.get_product_name(driver);
                    break;
                case "tshirts":
                case "t-shirts":
                    tshirts_link.Click();
                    product_name_list = site_wide.get_product_name(driver);
                    break;
            }
        }
        
        [Given(@"I quick view product ""(.*)"""),Then(@"I quick view product ""(.*)""")]
        public void GivenIQuickViewProduct(int product_number)
        {
            site_wide_objects site_wide = new site_wide_objects();
            //chosen_item = site_wide.quick_view_item(product_number, driver);
            var chosen_item = site_wide.get_image(product_number, driver);
            IWebElement quick_view = chosen_item.FindElement(By.ClassName("quick-view"));

            Actions action = new Actions(driver);
            action.MoveToElement(chosen_item).Click(quick_view).Build().Perform();

            product_name = driver.FindElement(By.XPath("//*[@itemprop='name']"));
        }


        [Then(@"I should see the product details for product ""(.*)""")]
        public void ThenIShouldSeeTheProductDetailsForProduct(int product_number)
        {
            var product_name = driver.FindElement(By.XPath("//*[@itemprop='name']"));

            IWebElement list_item = product_name_list[product_number].FindElement(By.XPath("//*[contains(@itemprop,'name')]"));
            if (list_item.Text != product_name.Text)
            {
                Console.WriteLine("It is likely an incorrect product was opened");
                driver.Quit();
            }
        }

        [When(@"I add that product to my cart with the following details:")]
        public void WhenIAddThatProductToMyCartWithTheFollowingDetails(Table table)
        {
            site_wide_objects site_wide = new site_wide_objects();
            quick_view_page quick_view = new quick_view_page();
            var dictionary = site_wide_objects.ToDictionary(table);
            String quantity = dictionary["quantity"];
            String size = dictionary["size"];
            IWebElement iframeSwitch = driver.FindElement(By.XPath("//iframe[@class='fancybox-iframe']"));
            driver.SwitchTo().Frame(iframeSwitch.GetAttribute("name"));

            PageFactory.InitElements(driver, quick_view);

            Thread.Sleep(1000);
            IWebElement quantity_option = quick_view.quantity;
            quantity_option.Clear();
            quantity_option.SendKeys(quantity);

            Thread.Sleep(1000);
            IWebElement size_dropdown = quick_view.size;
            var select_element = new SelectElement(size_dropdown);
            select_element.SelectByText(size);

            IWebElement add_to_cart_button = quick_view.add_to_cart_button;
            add_to_cart_button.Click();

        }

        [Then(@"I should see a confirmation popup")]
        public void ThenIShouldSeeAConfirmationPopup()
        {
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);
            confirmation_popup confirmed = new confirmation_popup();
            PageFactory.InitElements(driver, confirmed);
            IWebElement confirmation_message = confirmed.product_added;
            IWebElement product_title = confirmed.product_title;
            IWebElement garment_attributes = confirmed.colour_size;
            IWebElement quantity_value = confirmed.quantity;
            IWebElement price_value = confirmed.value;
            String product_name = product_title.Text;
            String colour_size_attribute = garment_attributes.Text;
            String colour = colour_size_attribute.Split(',')[0];
            String size = colour_size_attribute.Substring(colour_size_attribute.Length - 1, 1);
            String quantity = quantity_value.Text;
            String price = price_value.Text.Substring(1, price_value.Text.Length - 1);
            item_in_cart_dict item_in_cart = new item_in_cart_dict();
            Dictionary<string, string> _dict = item_in_cart.item_to_dictionary(product_name, colour, size, quantity, price);
            dicts[dict_counter] = _dict;
            dict_counter++;

            if (confirmation_message.Text != "Product successfully added to your shopping cart") 
            {
                Console.WriteLine("Product was not added to cart correctly");
                driver.Quit();
            }

        }

        [Then(@"I should see the size as ""(.*)"" on the confirmation popup")]
        public void ThenIShouldSeeTheSizeAsOnTheConfirmationPopup(string chosen_size)
        {
            confirmation_popup confirmed = new confirmation_popup();
            PageFactory.InitElements(driver, confirmed);
            IWebElement garment_attributes = confirmed.colour_size;
            String colour_size_attribute = garment_attributes.Text;
            String size = colour_size_attribute.Substring(colour_size_attribute.Length - 1, 1);
            if (size.ToLower() != chosen_size.ToLower())
            {
                Console.WriteLine("Size added was incorrect");
                driver.Quit();
            }
        }

        [Then(@"I continue shopping")]
        public void ThenIContinueShopping()
        {
            Thread.Sleep(1000);
            IWebElement continue_shopping_button = driver.FindElement(By.XPath("//*[@title='Continue shopping']"));
            continue_shopping_button.Click();
        }

        [Then(@"I proceed to checkout")]
        public void ThenIProceedToCheckout()
        {
            Thread.Sleep(1000);
            IWebElement checkout_button = driver.FindElement(By.XPath("//*[@title='Proceed to checkout']"));
            checkout_button.Click();
        }

        [Then(@"the details for all orders should be correct in the cart")]
        public void ThenTheDetailsForAllOrdersShouldBeCorrectInTheCart()
        {
            basket_page basket = new basket_page();
            PageFactory.InitElements(driver, basket);
            Dictionary<String, String>[] basket_items = basket.Get_cart_items(driver);
            Assert.AreEqual(dicts, basket_items, "The cart did not match the original product added");

        }

        [Then(@"I proceed to payment and complete my order")]
        public void ThenIProceedToPaymentAndCompleteMyOrder()
        {
            //proceed to the address page
            IWebElement checkout_button = driver.FindElement(By.XPath("//*[@id='center_column']/p[2]/a[1]"));
            checkout_button.Click();
            //proceed to the shipping page 
            IWebElement proceed_to_shipping = driver.FindElement(By.Name("processAddress"));
            proceed_to_shipping.Click();
            //agree to terms and proceed to payment
            IWebElement agree_terms = driver.FindElement(By.Id("cgv"));
            agree_terms.Click();
            IWebElement proceed_to_payment = driver.FindElement(By.Name("processCarrier"));
            proceed_to_payment.Click();

            IWebElement select_by_wire = driver.FindElement(By.ClassName("bankwire"));
            select_by_wire.Click();
            IWebElement confirm_order = driver.FindElement(By.XPath("//*[@id='cart_navigation']/button"));
            confirm_order.Click();
            IWebElement page_heading = driver.FindElement(By.ClassName("page-heading"));
            Assert.AreEqual(page_heading.Text, "ORDER CONFIRMATION");
        }

        [Given(@"I go to my order history")]
        public void GivenIGoToMyOrderHistory()
        {
            driver.Url = "http://automationpractice.com/index.php?controller=history";
            IWebElement page_heading = driver.FindElement(By.ClassName("page-heading"));
            Assert.AreEqual(page_heading.Text, "ORDER HISTORY");
        }

        [Given(@"I select my most recent order and view its details")]
        public void GivenISelectMyMostRecentOrderAndViewItsDetails()
        {
            IWebElement date_of_latest_item = driver.FindElement(By.CssSelector("#order-list > tbody > tr.first_item > td.history_date.bold"));
            Assert.AreEqual(date_of_latest_item.Text, DateTime.Now.ToString("MM/dd/yyyy"), "There are no previous orders");
            IWebElement most_recent_order = driver.FindElement(By.CssSelector("#order-list > tbody > tr.first_item > td.history_link.bold.footable-first-column"));
            most_recent_order.Click();
            IWebElement order_details = driver.FindElement(By.Id("block-order-detail"));
        }

        [When(@"I add a message to item ""(.*)"" in the order")]
        public void WhenIAddAMessageToItemInTheOrder(int item_no)
        {
            IWebElement order_details = driver.FindElement(By.Id("block-order-detail"));
            IWebElement product_selector = driver.FindElement(By.Name("id_product"));
            List<IWebElement> products_list = new List<IWebElement>();
            products_list = driver.FindElements(By.TagName("option")).ToList();
            try
            {
                Assert.IsTrue(item_no <= products_list.Count, "There arent that many products in the order");
                Console.Write("A screenshot has been triggered upon assertion failure.");
            }
            catch (Exception e)
            {
                string message = "Exception- " + DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
                var screenshot = driver.TakeScreenshot();
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile("message" + ".png", ScreenshotImageFormat.Png);
            }

            product_selector.SendKeys(products_list[item_no].Text);
            IWebElement text_input = driver.FindElement(By.Name("msgText"));
            input_text_for_message = "This message should be added to product: " + products_list[item_no].Text;
            text_input.SendKeys(input_text_for_message);
            IWebElement send_button = driver.FindElement(By.XPath("//button[@name='submitMessage']"));
            send_button.Click();
        }

        [Then(@"I should see that message has been added")]
        public void ThenIShouldSeeThatMessageHasBeenAdded()
        {
            IWebElement message_text = driver.FindElement(By.XPath("//*[@id='block-order-detail']/div[5]/table/tbody/tr/td[2]"));
            Assert.AreEqual(message_text.Text, input_text_for_message);
        }

        JObject response_json = new JObject();
        JObject deleted_user = new JObject();
        
 
        [Given(@"I want to retrieve user ""(.*)""")]
        public void GivenIWantToRetrieveUser(string user_id)
        {
            string url = "https://reqres.in/api/users/"+user_id;

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.DownloadString(url);
                Console.Write(data.ToString());

                response_json = JObject.Parse(data);
            }
        }

        [Then(@"the first name should be ""(.*)""")]
        public void ThenTheFirstNameShouldBe(string first_name)
        {
            Assert.AreEqual(response_json["data"]["first_name"].ToString(), first_name);
        }

        [Then(@"the last name should be ""(.*)""")]
        public void ThenTheLastNameShouldBe(string last_name)
        {
            Assert.AreEqual(response_json["data"]["last_name"].ToString(), last_name);
        }

        [Then(@"the id should be ""(.*)""")]
        public void ThenTheIdShouldBe(string id)
        {
            Assert.AreEqual(response_json["data"]["id"].ToString(), id);
        }

        Dictionary<String, String> POST_user_details = new Dictionary<string, string>();

        [Given(@"I want to create a user with the following details:")]
        public void GivenIWantToCreateAUserWithTheFollowingDetails(Table table)
        {
            string url = "https://reqres.in/api/users/";

            site_wide_objects site_wide = new site_wide_objects();
            POST_user_details = site_wide_objects.ToDictionary(table);
            var datastring = JsonConvert.SerializeObject(POST_user_details);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.UploadString(url, datastring);
                Console.Write(data.ToString());

                response_json = JObject.Parse(data);
            }
        }

        [Then(@"the response details should match")]
        public void ThenTheResponseDetailsShouldMatch()
        {
            Assert.AreEqual(response_json["name"].ToString(), POST_user_details["name"]);
            Assert.AreEqual(response_json["job"].ToString(), POST_user_details["job"]);
        }

        [Given(@"I want to update user ""(.*)"" with the following details:")]
        public void GivenIWantToUpdateUserWithTheFollowingDetails(string user_id, Table table)
        {
            string url = "https://reqres.in/api/users/" + user_id;

            site_wide_objects site_wide = new site_wide_objects();
            POST_user_details = site_wide_objects.ToDictionary(table);
            var datastring = JsonConvert.SerializeObject(POST_user_details);

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.UploadString(url, datastring);
                Console.Write(data.ToString());

                response_json = JObject.Parse(data);
            }
        }

        [Given(@"I want to remove user ""(.*)"" from the system")]
        public void GivenIWantToRemoveUserFromTheSystem(string user_id)
        {
            string url = "https://reqres.in/api/users/" + user_id;

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.DownloadString(url);
                Console.Write(data.ToString());

                deleted_user = JObject.Parse(data);
            }

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.UploadString(url, "PUT", "");
                Console.Write(data.ToString());
            }
        }

        [Then(@"the call to get user ""(.*)"" details should fail")]
        public void ThenTheCallToGetThatUsersDetailsShouldFail(string user_id)
        {
            string url = "https://reqres.in/api/users/" + user_id;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var data = client.DownloadString(url);
                Console.Write(data.ToString());

                response_json = JObject.Parse(data);
            }
            //Assert.AreNotEqual(deleted_user, response_json);
        }

    }
}