using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IPrintInProcessTagStockService
    {
        TagStockProductModalView getProduct(TagStockProductSearchView model);
        TagStockGroupModalView getGroup(TagStockProductSearchView model);
        //void PringTag(PrintInProcessTagView model);
    }
}
