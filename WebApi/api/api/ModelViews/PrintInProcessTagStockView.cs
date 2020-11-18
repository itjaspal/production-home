using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class PrintInProcessTagStockView
    {
        public string prod_code { get; set; }
        public string sub_prod_code { get; set; }
        public string sub_prod_name { get; set; }
        public string size_name { get; set; }
        public string design_name { get; set; }
        public int qty { get; set; }
        public string description { get; set; }
        public string user_id { get; set; }
        public string req_date { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public int build_no { get; set; }
    }

    public class TagStockProductSearchView
    {
        public string entity { get; set; }
        public string req_date { get; set; }
        public string wc_code { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string group_line { get; set; }
    }

    public class TagStockProductModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<TagStockProductView> datas { get; set; }
    }

    public class TagStockProductView
    {
        public string prod_code { get; set; }
        public string sub_prod_code { get; set; }
        public string sub_prod_name { get; set; }
        public string por_no { get; set; }
        public string size_name { get; set; }
        public string design_name { get; set; }
        public string ref_no { get; set; }
        public string req_date { get; set; }
        public int build_no { get; set; }
    }

    public class TagStockGroupModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<TagStockGroupView> datas { get; set; }
    }

    public class TagStockGroupView
    {
        public string group_line { get; set; }
        public string group_line_name { get; set; }

    }

    public class ProductDataView
    {
        public string size_name { get; set; }
        public string design_name { get; set; }
    }
}