using System;
using System.Collections.Generic;
using System.Text;

namespace Multithreading.Patterns.Speculative_Execution
{
    class StockData
    {
        public string DataSource { get; private set; }
        public List<decimal> Prices { get; private set; }

        public StockData(string dataSource, List<decimal> prices)
        {
            this.DataSource = dataSource;
            this.Prices = prices;
        }
    }
}
