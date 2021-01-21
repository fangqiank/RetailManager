using Caliburn.Micro;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Models;

namespace RMWPFUi.ViewModels
{
    public class SalesViewModel:Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private BindingList<ProductModel> _products;
        private int _itemQuantity = 1;
        private ProductModel _selectedProduct;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

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

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(()=>SelectedProduct);
                NotifyOfPropertyChange(()=>CanAddToCart);
            }
        }

        public BindingList<CartItemModel> Cart
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
                NotifyOfPropertyChange(()=>CanAddToCart);
            }

        }

        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }

                return subTotal.ToString("c");
            } 
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
                //make sure something is selected
                //Make sure there is an item quantity
                bool output = ItemQuantity>0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

                return output;

            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //way of resfreshing the cart display
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
           
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(()=>SubTotal);
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
            NotifyOfPropertyChange(() => SubTotal);
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
