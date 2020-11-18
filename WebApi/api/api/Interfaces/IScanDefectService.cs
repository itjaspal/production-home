using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IScanDefectService
    {
        ScanDefectView ScanAdd(ScanDefectSearchView model);
        ScanDefectView EntryAdd(ScanDefectSearchView model);
        void Cancel(ScanDefectView scan);
        DefectProductSubModalView getSubProduct(DefectProductSubSearchView model);
        JobInProcessStockView EntryCancel(ScanDefectSearchView model);
        DefectProductSubModalView getSubProductCancel(DefectProductSubSearchView model);

    }
}
