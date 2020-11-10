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

        DataQcEnrtyView GetQCEntryData(string build_type);
        //ItemNoWipModalView GetItemNoWipList(string entity, string por_no);
        //ItemNoWipModalView GetItemNoFinList(string entity, string por_no);
        //void DataQcEntry(DataQcEnrtyView model);


    }
}
