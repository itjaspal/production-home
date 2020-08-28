using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class MasterMenuSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string menuFunctionId { get; set; }
        //public string menuFunctionGroupId { get; set; }
        public string menuFunctionName { get; set; }
        //public string menuURL { get; set; }
        //public string iconName { get; set; }
        //public int orderDisplay { get; set; }
    }

    public class MasterMenuView
    {
        public string menuFunctionId { get; set; }
        public string menuFunctionGroupId { get; set; }
        public string menuFunctionName { get; set; }
        public string menuURL { get; set; }
        public string iconName { get; set; }
        //public int orderDisplay { get; set; }

    }
}