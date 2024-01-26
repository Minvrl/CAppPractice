using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp
{
    internal class Product
    {
        public Product()
        {
            _staticNo++;
            No = _staticNo;
        }

        public Product(string name, double salePrice, double costPrice, DateTime expireDate) : this()
        {
            Name = name;
            SalePrice = salePrice;
            CostPrice = costPrice;
            ExpireDate = expireDate;
        }

        static int _staticNo;
        public readonly int No;
        public string Name;
        public double SalePrice;
        public double CostPrice;
        public DateTime ExpireDate;

        public override string ToString()
        {
            return $"No: {No} - Name: {Name} - Price: {SalePrice} - ExpireDate: {ExpireDate.ToString("dd.MM.yyyy")}";
        }
    }
}
