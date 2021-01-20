using Caliburn.Micro;
using System.ComponentModel;

namespace RMWPFUi.ViewModels
{
    public class SalesViewModel:Screen
    {
        private BindingList<string> _products;
        private string _itemQuantity;
        private BindingList<string> _cart;

        public BindingList<string> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(()=>Products);
            }
        }

        public BindingList<string> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(()=>Cart);
            }
        }

        public string ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(()=>ItemQuantity);
            }

        }

        public string SubTotal
        {
            //ToDo
            get => "$0.00";
        }

        public string Tax
        {
            //ToDo
            get => "$0.00";
        }

        public string Total
        {
            //ToDo
            get => "$0.00";
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                //make sure something is selected
                //Make sure there is an item quantity
                return output;

            }
        }

        public void AddToCart()
        {

        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //make sure something is selected
                return output;

            }
        }

        public void RemoveFromCart()
        {

        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                //make sure something is selected
                return output;

            }
        }

        public void CheckeOut()
        {

        }

    }
}
