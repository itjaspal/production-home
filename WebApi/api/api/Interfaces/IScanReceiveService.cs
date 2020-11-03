using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IScanReceiveService
    {
        ScanReceiveDataView SearchDataScanReceive(ScanReceiveDataSearchView model);
        ScanReceiveView ScanReceiveAdd(ScanReceiveSearchView model);
        void ScanReceiveCancel(ScanReceiveView scan);
        void ApproveReceive(ScanReceiveDataDetailView model);
    }
}
