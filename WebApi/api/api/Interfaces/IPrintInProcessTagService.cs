using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IPrintInProcessTagService
    {
        PrintInProcessTagView GetProductInfo(string code);
        void PringTag(PrintInProcessTagView model);
        TagProductModalView getProduct(TagProductSearchView model);
    }
}
