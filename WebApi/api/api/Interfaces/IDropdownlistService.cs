using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IDropdownlistService
    {
       
        List<Dropdownlist> GetDdlMobilePrntJIT();
        List<Dropdownlist> GetDdlMobilePrntSTK();
        List<Dropdownlist> GetDdlWCInprocess(string user);
        List<Dropdownlist> GetDdlWCSend(string user);
        List<Dropdownlist> GetDdlWCPutaWay(string entity, string user);
        List<Dropdownlist> GetDdlPutAwayWHMast();
        List<Dropdownlist> GetDdlPtwSetNoList(string entity, string doc_code, string doc_no);
        List<Dropdownlist> GetDdlPtwProdList(string entity, string doc_code, string doc_no);
        List<Dropdownlist> GetDdlWCInprocessStock(string user);
        List<Dropdownlist> GetDeptMarketing();

    }
}
