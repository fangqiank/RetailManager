using System;

namespace RMDataManager.Library.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Decimal RetailPrice { get; set; }
        public int QuantityInStock { get; set; }
    }
}
