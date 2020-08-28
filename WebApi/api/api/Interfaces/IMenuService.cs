using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IMenuService
    {
        CommonSearchView<MasterMenuView> Search(MasterMenuSearchView model);
        MasterMenuView GetInfo(string code);
        void Update(MasterMenuView model);
    }
}
