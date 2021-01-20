using Caliburn.Micro;
using System.ComponentModel;
using System.Threading.Tasks;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;

namespace RMWPFUi.ViewModels
{
    public class SalesViewModel:Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private BindingList<ProductModel> _products;
        private int _itemQuantity;
        private BindingList<string> _cart;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;
          
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        public BindingList<ProductModel> Products
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

        public int ItemQuantity
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
