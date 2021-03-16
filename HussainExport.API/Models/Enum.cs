using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HussainExport.API.Models
{
    public class Enum
    {
        enum TransactionTypeEnum
        {
            SalesContract = 1,
            Overhead = 2,
            Stock = 3
        }

        enum AccountTypeEnum
        {
                Asset=1,
Equity=2,
Liabilities=3,
Receivables=4,
Payable=5
        }
    }
}
