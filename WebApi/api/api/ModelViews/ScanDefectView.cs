using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ScanDefectView
    {
        public string entity { get; set; }
        public string sub_prod_code { get; set; }
        public string sub_prod_name { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public int qty_plan { get; set; }
        public int qty_fin { get; set; }
        public int qty { get; set; }
        public string wc_code { get; set; }
        public int item_no { get; set; }
    }

    
    public class ScanDefectSearchView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public string scan_data { get; set; }
        public string prod_code { get; set; }
        public string sub_prod_code { get; set; }
        public string wc_code { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string user_id { get; set; }
        public int qty { get; set; }
        public int item_no { get; set; }

    }

    public class DefectProductSubSearchView
    {
        public string entity { get; set; }
        public string req_date { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string wc_code { get; set; }
    }

    public class DefectProductSubModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<DefectProductSubView> datas { get; set; }
    }

    public class DefectProductSubView
    {
        public string prod_code { get; set; }
        public string sub_prod_code { get; set; }
        public string sub_prod_name { get; set; }
        public int qty_plan { get; set; }
        public int qty_fin { get; set; }
        public int qty_defect {get; set; }
        public int item_no { get; set; }
    }
}