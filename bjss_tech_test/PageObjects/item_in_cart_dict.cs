using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bjss_tech_test.PageObjects
{
    class item_in_cart_dict
    {
        public Dictionary<string, string> item_to_dictionary(string _product_name, string _colour, string _size, string _quantity, string _price)
        {
            Dictionary<string, string> myDict = new Dictionary<string, string>
            {
                { "product_name", _product_name },
                { "colour", _colour },
                { "size", _size },
                { "quantity", _quantity },
                { "price", _price }
            };
            return myDict;
        }
    }
}
