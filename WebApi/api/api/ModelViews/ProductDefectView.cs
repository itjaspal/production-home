using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ProductDefectView
    {
        public int pageIndex { get; set; }
        public int totalItem { get; set; }
        public int itemPerPage { get; set; }
        public string entity { get; set; }
        public DateTime req_date { get; set; }
        public string build_type { get; set; }
        public int total_qty_pdt { get; set; }
        public int total_qty_cutting { get; set; }
        public int total_qty_wip { get; set; }
        public int total_qty_fin { get; set; }
        public List<ProductDefectDetailView> datas { get; set; }
    }

    public class ProductDefectDetailView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string brand_name { get; set; }
        public string design_name { get; set; }
        public string size_name { get; set; }
        public int qty_pdt { get; set; }
        public int qty_cutting { get; set; }
        public int qty_wip { get; set; }
        public int qty_fin { get; set; }
    }

    public class ProductDefectSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity { get; set; }
        public string por_no { get; set; }
        public string req_date { get; set; }
        public string build_type { get; set; }
        public string user_id { get; set; }
    }
}