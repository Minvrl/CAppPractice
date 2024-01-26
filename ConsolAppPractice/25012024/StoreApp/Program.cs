
using StoreApp;
using StoreApp.Exceptions;

Market market = new Market();
market.AlcoholPercentLimit = 45;
string opt;
do
{
    opt = ChoseOperation();

    switch (opt)
    {
        case "1":
            AddProduct();
            break;
        case "2":
            RemoveProduct();    
            break;
        case "3":
            ShowProducts();
            break;
        case "4":
            market.UpdateProduct();
            break;
        case "5":
            string noStr;
            int no;
            string countStr;
            int count;  
            do
            {
                Console.WriteLine("Satmaq istediyniz mehsulun nomresini daxil edin - ");
                noStr = Console.ReadLine(); 
            } while (!int.TryParse(noStr, out no));
            do
            {
                Console.WriteLine(" Sayini daxil edin - ");
                countStr = Console.ReadLine();
            } while (!int.TryParse(countStr, out count));
            market.Sell(no, count);

            break;
        case "6":
            Console.WriteLine(market.AvgAlcoholPercent);
            break;
        case "7":
            ShowProfit();
            break;
        default:
            break;
    }

} while (opt!="0");




string ChoseOperation()
{
    ShowMenu();
    Console.WriteLine("Emeliyyat secin: ");
    return Console.ReadLine();
}
void ShowMenu()
{
    Console.WriteLine("\n ====== MENU =======");
    Console.WriteLine("1. Mehsul elave et");
    Console.WriteLine("2. Mehsul sil");
    Console.WriteLine("3. Mehsullara bax");
    Console.WriteLine("4. Mehsulu yenile");
    Console.WriteLine("5. Mehsulu sat");
    Console.WriteLine("6. Ortalama alkoqol faizi");
    Console.WriteLine("7. Gelirlere bax");
    Console.WriteLine("0. Cix");
}

void AddProduct()
{
    string name;
    double costPrice, salePrice;
    string costPriceStr, salePriceStr, expireDateStr;
    DateTime expireDate;

    do
    {
        Console.WriteLine("Ad: ");
        name = Console.ReadLine();
    } while (string.IsNullOrEmpty(name));

    do
    {
        Console.Write(" Maya qiymeti: ");
        costPriceStr = Console.ReadLine();
       
    } while (!double.TryParse(costPriceStr, out costPrice) || costPrice<0);

    do
    {
        Console.Write("  Satis qiymeti: ");
        salePriceStr = Console.ReadLine();
        
    } while (!double.TryParse(salePriceStr, out salePrice) || salePrice<0 || salePrice<costPrice);

    do
    {
        Console.Write("   Yararliliq bitme muddeti (YYYY-MM-DD): ");
        expireDateStr = Console.ReadLine();
       
    } while (!DateTime.TryParse(expireDateStr, out expireDate));


checkIsDrink:
    Console.WriteLine("Icki mehsuludurmu? y/n");
    string isDrinkStr = Console.ReadLine();

    Product product;
    string alchoPercentStr = null;
    if (isDrinkStr == "y")
    {
        Console.WriteLine("Alkoqol faizi: ");
        alchoPercentStr = Console.ReadLine();
        double alchoPercent = Convert.ToDouble(alchoPercentStr);
        product = new DrinkProduct(name, salePrice, costPrice, expireDate, alchoPercent);
    }
    else if (isDrinkStr == "n")
    {
        product = new Product(name, salePrice, costPrice, expireDate);
    }
    else
        goto checkIsDrink;

    market.AddProduct(product);
}

void ShowProducts()
{
    Console.WriteLine("\na. Butun mehsullar");
    Console.WriteLine("b. Alkoqullu ickiler");
    Console.WriteLine("c  Alkoqolsuz ickiler");
    Console.WriteLine("Secim edin:");
    string showOpt = Console.ReadLine();

    switch (showOpt)
    {
        case "a":
            for (int i = 0; i < market.Products.Length; i++)
                Console.WriteLine(market.Products[i]);
            break;
        case "b":
            var alcoholProducts = market.GetAllAlcoholDrinks();
            for (int i = 0; i < alcoholProducts.Length; i++)
                Console.WriteLine(alcoholProducts[i]);
            break;
        case "c":
            var nonAlcoholProducts = market.GetAllNonAlcoholDrinks();
            for (int i = 0; i < nonAlcoholProducts.Length; i++)
                Console.WriteLine(nonAlcoholProducts[i]);
            break;
        default:
            Console.WriteLine("Seciminiz yanlisdir");
            break;
    }
}


void RemoveProduct()
{
    Console.WriteLine("\n======== Mehsul silmek ==========");
    for (int i = 0; i < market.Products.Length; i++)
        Console.WriteLine(market.Products[i]);
    Console.WriteLine("Melsul no:");
    string noStr = Console.ReadLine();
    int no = Convert.ToInt32(noStr);

    try
    {
        market.RemoveProductByNo(no);
    }
    catch (ProductNotFoundException)
    {
        Console.WriteLine($"{no} nomreli mehsul yoxdur");
    }
    catch (ProductExpireDateException)
    {
        Console.WriteLine($"Mehsulun yararliqli muddetinin bitmesine 1 ilden cox var");
    }
    catch
    {
        Console.WriteLine("Bilinmedik bir xeta bas verdi");
    }

}

void ShowProfit()
{
    Console.WriteLine("a. Umumi gelir");
    Console.WriteLine("b. Alkoqollu ickilerden gelir");
    Console.WriteLine("c. Alkoqolsuz ickilerden gelir");

    string optP = Console.ReadLine();
    switch (optP)
    {
        case "a":
            Console.WriteLine(market.GetAllProfit());
            break;
        case "b":
            Console.WriteLine(market.GetAlcoholProfit()); 
            break;
        case "c":
            Console.WriteLine(market.GetNonAlcoholProfit()  );
            break;
        default:
            Console.WriteLine("Duzgun operator daxil edin");
            break;
    }

}
