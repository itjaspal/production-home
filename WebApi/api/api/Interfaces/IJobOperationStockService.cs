using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IJobOperationStockService
    {
        JobOperationStockView SearchDataPlan(JobOperationStockSearchView model);
        JobOperationStockView SearchDataFin(JobOperationStockSearchView model);
        JobOperationStockView SearchDataDefect(JobOperationStockSearchView model);
        ProductGroupView SearchSummaryProdcutGroup(ProductGroupSearchView model);
    }
}
