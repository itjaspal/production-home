﻿using api.ModelViews;
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
        ProductionTrackingStockView ProductionTrackingStock(ProductGroupSearchView model);
        ProductionTrackingStockView ProductionTrackingDetailStock(ProductGroupSearchView model);
        OrderReqView getOrderReqAll(OrderReqSearchView model);
        ProductionTrackingStockView ProductionTrackingGroupDetailStock(ProductGroupDetailSearchView model);
        ProductionTrackingStockView ProductionTrackingGroupOthDetailStock(ProductGroupDetailSearchView model);
    }
}
