using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RMWPFUi.Models
{
    public class ProductDisplayModel:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        private int _QuantityInStock;

        public int QuantityInStock
        {
            get => _QuantityInStock;
            set
            {
                _QuantityInStock = value;
                NotifyPropertyChanged(nameof(QuantityInStock));
            }
        }

        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        //public void CallPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke((this, new PropertyChangedEventArgs(propertyName)));
        //}
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
