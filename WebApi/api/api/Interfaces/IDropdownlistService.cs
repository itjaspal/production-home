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


    }
}
