using StoreApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreApp
{
    internal class Market : IMarket, IMarketReporter
    {
        private Product[] _products = new Product[0];
        public Product[] Products => _products;
        public int AlcoholPercentLimit;
        private double totalProfits;
        private double alcoholProfits;
        private double nonAlcoholProfits;

        public double AvgAlcoholPercent {
            get
            {
                var alcoholDrinks = GetAllAlcoholDrinks();

                double totalPercent = 0;
                for (int i = 0; i < alcoholDrinks.Length; i++)
                    totalPercent += alcoholDrinks[i].AlcoholPercent;

                return alcoholDrinks.Length==0?0:totalPercent / alcoholDrinks.Length;
            }
        }

        public void AddProduct(Product pr)
        {
            if(pr is DrinkProduct drink && drink.AlcoholPercent>this.AlcoholPercentLimit)
                return;

            Array.Resize(ref _products, _products.Length + 1);
            _products[_products.Length - 1] = pr;
        }

        public double GetAlcoholProfit()
        {
            return alcoholProfits;
        }

        public double GetAllProfit()
        {
            return totalProfits;
        }

        public double GetNonAlcoholProfit()
        {
            return nonAlcoholProfits;
        }

        public void RemoveProductByNo(int no)
        {
            var wantedProduct = FindByNo(no);
            if(wantedProduct==null) throw new ProductNotFoundException();

            if (wantedProduct.ExpireDate >= DateTime.Now.AddYears(1))
                throw new ProductExpireDateException();

            var wantedIndex = FindIndexByNo(no);
            for (int i = wantedIndex; i < _products.Length - 1; i++)
            {
                var temp = _products[i];
                _products[i] = _products[i + 1];
                _products[i + 1] = temp;
            }

            Array.Resize(ref _products, _products.Length - 1);

        }

        public Product FindByNo(int no)
        {
            for (int i = 0; i < _products.Length; i++)
            {
                if (_products[i].No == no)
                {
                    return _products[i];
                }
            }

            return null;
        }
        public int FindIndexByNo(int no)
        {
            for (int i = 0; i < _products.Length; i++)
            {
                if (_products[i].No == no)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Sell(int no, int count = 1)
        {
            Product prod = FindByNo(no);
            if (prod is DrinkProduct drink && drink.AlcoholPercent > 0)
            {
                alcoholProfits += prod.SalePrice * count;
            }
            else
            {
                nonAlcoholProfits += prod.SalePrice * count;
            }

            totalProfits += prod.SalePrice * count;
            Console.WriteLine("Mehsul satildi !");
        }

        public DrinkProduct[] GetAllAlcoholDrinks()
        {
            DrinkProduct[] drinks = new DrinkProduct[0];

            for (int i = 0; i < _products.Length; i++)
            {
                if (_products[i] is DrinkProduct drink && drink.AlcoholPercent>0)
                {
                    Array.Resize(ref drinks, drinks.Length + 1);
                    drinks[drinks.Length - 1] = drink;
                }
            }

            return drinks;
        }

        public DrinkProduct[] GetAllDrinks()
        {
            DrinkProduct[] drinks = new DrinkProduct[0];

            for (int i = 0; i < _products.Length; i++)
            {
                if (_products[i] is DrinkProduct drink)
                {
                    Array.Resize(ref drinks, drinks.Length + 1);
                    drinks[drinks.Length - 1] = drink;
                }
            }

            return drinks;
        }

        public DrinkProduct[] GetAllNonAlcoholDrinks()
        {
            DrinkProduct[] drinks = new DrinkProduct[0];

            for (int i = 0; i < _products.Length; i++)
            {
                if (_products[i] is DrinkProduct drink && drink.AlcoholPercent==0)
                {
                    Array.Resize(ref drinks, drinks.Length + 1);
                    drinks[drinks.Length - 1] = drink;
                }
            }

            return drinks;
        }

        public void UpdateProduct()
        {
            string noStr;
            int no;

            do
            {
                Console.Write("Yenilemek istediğiniz mehsulun nomresini daxil edin: ");
                noStr = Console.ReadLine();
            } while (!int.TryParse(noStr, out no));

            Product toUpdate = FindByNo(no);

            if (toUpdate == null)
            {
                Console.WriteLine("Mehsul tapılmadı. Zehmet olmasa duzgun nomre daxil edin.");
                return;
            }

            string name;
            double salePrice;
            string salePriceStr;

            do
            {
                Console.Write("Yeni ad: ");
                name = Console.ReadLine();
                
            } while (string.IsNullOrEmpty(name));

            do
            {
                Console.Write("Yeni satis qiymeti: ");
                salePriceStr = Console.ReadLine();

                
            } while (!double.TryParse(salePriceStr, out salePrice) || salePrice < 0);

            if (toUpdate != null)
            {
                toUpdate.Name = name;
                toUpdate.SalePrice = salePrice;

                Console.WriteLine("Mehsul yenilendi!");
            }

        }

    }
}
