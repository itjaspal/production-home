﻿using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IProductionRecService
    {
        ProductionRecTotalView SearchProductionRec(ProductionRecSearchView model);
        ProductionRecDetailTotalView SearchProductionRecDetail(ProductionRecDetailSearchView model);
        ProductionRecTotalView SearchPutAwayWaiting(ProductionRecSearchView model);
        int getTimeDelay(string entity, string build_type);
    }
}
