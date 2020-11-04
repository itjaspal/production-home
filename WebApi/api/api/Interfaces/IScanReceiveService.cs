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
        void SendMail(string entity , string doc_no);
        List<SendDataView> getProductDetail(string entity, string doc_no);
    }
}
