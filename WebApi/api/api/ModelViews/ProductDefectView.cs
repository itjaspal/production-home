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
        public string bar_code { get; set; }
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

    public class ItemNoModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<ItemNoView> datas { get; set; }
    }


    public class ItemNoView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string build_type { get; set; }
        public string qc_date { get; set; }
        public string qc_time { get; set; }
        public string qc_process { get; set; }
        public int item_no { get; set; }
        public int qc_qty { get; set; }
        public int no_pass_qty { get; set; }
        public string width { get; set; }
        public string lenght { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string user_id { get; set; }
        public string size_name { get; set; }
    }

    public class DataQcView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string build_type { get; set; }
        public string qc_proceee { get; set; }

    }

    public class DataQcCuttingView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string build_type { get; set; }
        public string qc_date { get; set; }
        public string qc_process { get; set; }
        public int item_no { get; set; }
        public int qc_qty { get; set; }
        public int no_pass_qty { get; set; }
        public string width { get; set; }
        public string lenght { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string user_id { get; set; }
        public string size_name { get; set; }
    }

    public class DataQcEnrtyView
    {
        public string entity { get; set; }
        public int item_no { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string qc_date { get; set; }
        public string qc_time { get; set; }
        public string build_type { get; set; }
        public string qc_process { get; set; }
        public int qc_qty { get; set; }
        public int no_pass_qty { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string remark4 { get; set; }
        public string remark5 { get; set; }
        public string remark6 { get; set; }
        public string user_id { get; set; }
        public List<QcGroupCheckView> datas { get; set; }
    }

    public class QcGroupCheckView
    {
        public int seq_no { get; set; }
        public string item_desc { get; set; }
        public string item_value { get; set; }
        public string item_result { get; set; }
    }

    public class ItemNoWipModalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<ItemNoWipView> datas { get; set; }
    }


    public class ItemNoWipView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string prod_code { get; set; }
        public string build_type { get; set; }
        public string qc_date { get; set; }
        public string qc_time { get; set; }
        public string qc_process { get; set; }
        public int item_no { get; set; }
        public int qc_qty { get; set; }
        public int no_pass_qty { get; set; }
        public string width { get; set; }
        public string lenght { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string remark4 { get; set; }
        public string remark5 { get; set; }
        public string remark6 { get; set; }
        public string user_id { get; set; }
        public string size_name { get; set; }
        public List<QcGroupCheckView> groupdatas { get; set; }
    }
}