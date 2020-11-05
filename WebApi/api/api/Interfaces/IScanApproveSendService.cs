using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IScanApproveSendService
    {
        ScanApproveSendView SearchDataScanSend(ScanApproveSendSearchView model);
        ScanApproveFinView ScanApvSendNew(ScanApproveAddView model);
        ScanApproveFinView ScanApvSendAdd(ScanApproveAddView model);
        ScanApproveFinView ScanApvSendCancel(ScanApproveAddView model);
        ScanApproveProductView getProductDetail(string entity, string doc_no , string wc_code);
    }
}
