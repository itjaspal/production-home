using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IJobInProcessStockService
    {
        JobOperationStockView SearchJobInPorcessPlan(JobOperationStockSearchView model);
        JobOperationStockView SearchJobInPorcessFin(JobOperationStockSearchView model);
        JobInProcessStockView ScanAdd(JobInProcessStockSearchView model);
        JobInProcessStockView EntryAdd(JobInProcessStockSearchView model);
        void Cancel(JobInProcessStockView scan);
        ProductSubModalView getSubProduct(ProductSubSearchView model);
        ProductSubModalView getSubProductCancel(ProductSubSearchView model);
        JobInProcessStockView EntryCancel(JobInProcessStockSearchView model);
    }
}
