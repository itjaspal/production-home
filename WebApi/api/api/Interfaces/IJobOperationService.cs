using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IJobOperationService
    {
        JobOperationView SearchDataCurrent(JobOperationSearchView model);
        JobOperationView SearchDataPending(JobOperationSearchView model);
        JobOperationView SearchDataForward(JobOperationSearchView model);
        OrderView getOrderInfo(string entity ,string por_no);
        OrderSumView OrderSummary(OrderSumSearchView model);
    }
}
