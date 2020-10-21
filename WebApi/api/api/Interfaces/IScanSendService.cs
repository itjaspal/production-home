using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IScanSendService
    {
        ScanSendView ScanSendAdd(ScanSendSearchView model);
        SetNoView getSetNo(SetNoSearchView model);
        void PringSticker(ScanSendView model);
    }
}
