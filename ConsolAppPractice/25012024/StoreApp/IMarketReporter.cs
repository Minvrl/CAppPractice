using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp
{
    internal interface IMarketReporter
    {
        double GetAllProfit();
        double GetAlcoholProfit();
        double GetNonAlcoholProfit();
    }
}
