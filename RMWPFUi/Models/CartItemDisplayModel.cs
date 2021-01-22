using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RMWPFUi.Models
{
    public class CartItemDisplayModel: INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }
        private int _QuantityInCart;

        public int QuantityInCart
        {
            get => _QuantityInCart;
            set
            {
                _QuantityInCart = value;
                NotifyPropertyChanged(nameof(QuantityInCart));
                NotifyPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get => $"{Product.ProductName}({QuantityInCart})";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //public void CallPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke((this, new PropertyChangedEventArgs(propertyName)));
        //}

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

