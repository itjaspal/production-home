using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class PrintInProcessTagView
    {
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public string prod_name { get; set; }
        public string size_name { get; set; }
        public string design_name { get; set; }
        public int qty { get; set; }
        public string description { get; set; }
        public string user_id { get; set; }
        public string req_date { get; set; }
    }


    public class PrintInProcessTagSearchView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public string pdjit_grp { get; set; }
        public string bar_code { get; set; }
        public string wc_code { get; set; }
        public string user_id { get; set; }

    }

    public class TagProductSearchView
    {
        public string entity { get; set; }
        public string req_date { get; set; }
        public string wc_code { get; set; }
        public string pdjit_grp { get; set; }
    }

    public class TagProductModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<TagProductView> datas { get; set; }
    }

    public class TagProductView
    {
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string bar_code { get; set; }
        public string size { get; set; }
        public string design { get; set; }

    }

    public class PrinterDataView
    {
        public string printer_name { get; set; }
        public string filepath_data { get; set; }
        public string filepath_txt { get; set; }
    }
}