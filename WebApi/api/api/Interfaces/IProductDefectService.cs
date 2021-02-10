using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IProductDefectService
    {
        ProductDefectView SearchDataProductDefect(ProductDefectSearchView model);
        int GetItemNo(string entity , string por_no);
        void DataQcCutting(DataQcCuttingView model);
        ItemNoModalView GetItemNoList(string entity, string por_no);
        

        int GetItemNoQcEntry(string entity, string por_no , string qc_process);
        DataQcEnrtyView GetQcGroup(string build_type);
        void DataQcEntrtyData(DataQcEnrtyView model);
        ItemNoWipModalView GetItemNoWipList(string entity, string por_no , string qc_process);
        OrderReqView getOrderReq(OrderReqSearchView model);



    }
}
