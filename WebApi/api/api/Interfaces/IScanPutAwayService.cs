using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IScanPutAwayService
    {
        ScanPutAwaysTotalView ScanPutAwayAdd(ScanPutAwaySearchView model);
        ScanPutAwaysTotalView ScanPutAwayManualAdd(ScanPutAwaySearchView model);
        ScanPutAwaysTotalView PostSearhPtwDetail(ScanPutAwayDetailSearchView model);
        ScanPutAwaysTotalView ScanPutAwayCancel(ScanPutAwayCancelSearchView model);
    }
}
