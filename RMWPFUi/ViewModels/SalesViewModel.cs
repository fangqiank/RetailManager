﻿using System;
using AutoMapper;
using Caliburn.Micro;
using RMWPFUi.Library.Api;
using RMWPFUi.Library.Helpers;
using RMWPFUi.Library.Models;
using RMWPFUi.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RMWPFUi.ViewModels
{
    public class SalesViewModel:Screen
    {
        private readonly IProductEndPoint _productEndPoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndPoint _saleEndPoint;
        private readonly IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private BindingList<ProductDisplayModel> _products;
        private int _itemQuantity = 1;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public SalesViewModel(IProductEndPoint productEndPoint,IConfigHelper configHelper,
            ISaleEndPoint saleEndPoint,IMapper mapper,StatusInfoViewModel status,IWindowManager window)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
            _saleEndPoint = saleEndPoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                //var info = IoC.Get<StatusInfoViewModel>();

                if (ex.Message=="Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access",
                             "You do not have permission to interact with the sales");
                    await _window.ShowDialogAsync(_status, null, settings);
                    
                }
                else
                {
                    _status.UpdateMessage("Fatal exception",
                        ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(()=>Products);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(()=>SelectedProduct);
                NotifyOfPropertyChange(()=>CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
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
                return CalculateSubTotal().ToString("c");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //}

            return taxAmount;
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("c");
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("c");
            }
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //way of resfreshing the cart display
                //Cart.Remove(existingItem);
                //Cart.Add(existingItem);
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
           
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(()=>Tax);
            NotifyOfPropertyChange(()=>Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = SelectedCartItem!=null && SelectedCartItem?.QuantityInCart>0;
                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(()=>CanCheckOut);
            NotifyOfPropertyChange(()=>CanAddToCart);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = Cart.Count > 0;

                //make sure something is selected
                return output;

            }
        }

        public async Task CheckOut()
        {
            //create a salemodel and post to the API
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndPoint.PostSale(sale);

            await ResetSalesViewModel();

        }

    }
}
