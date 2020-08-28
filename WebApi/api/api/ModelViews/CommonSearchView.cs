using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class CommonSearchView<T>
    {
        public int pageIndex { get; set; }
        public int totalItem { get; set; }
        public int itemPerPage { get; set; }
        public List<T> datas { get; set; }
    }
}