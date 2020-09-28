using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class JobOperationSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string user_id { get; set; }
        public string wc_code { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
    }

    public class JobOperationView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public string wc_code { get; set; }
        public string wc_name { get; set; }
        public string build_type { get; set; }
        public List<JobOperationDetailView> dataGroups { get; set; }
        public List<JobOperationTotalGroupView> dataTotals { get; set; }
    }

    public class JobOperationTotalGroupView
    {
        public DateTime req_date { get; set; }
        public string pdjit_grp { get; set; }
        public string pdgrp_tname { get; set; }
        //public int total_build_qty { get; set; }
        public int total_plan_qty { get; set; }
        public int total_cancel_qty { get; set; }
        public int total_act_qty { get; set; }
        public int total_defect_qty { get; set; }
        public int total_diff_qty { get; set; }
        
    }

    public class JobOperationDetailView
    {
        public string por_no { get; set; }
        public DateTime req_date { get; set; }
        public string display_group { get; set; }
        public string display_type { get; set; }
        public string pdjit_grp { get; set; }
        public string pddsgn_code { get; set; }
        public string design_name { get; set; }
        public int plan_qty { get; set; }
        public int cancel_qty { get; set; }
        public int act_qty { get; set; }
        public int defect_qty { get; set; }
        public int diff_qty { get; set; }
    }

    public class JitgroupView
    {
        public string pdjit_grp { get; set; }
        public string group_name { get; set; }
        
    }

    public class PorGroupView
    {
        public string pdjit_grp { get; set; }
        public string group_name { get; set; }
        public string por_no { get; set; }
    }


    public class PorDesignView
    {
        public string pddsgn_code { get; set; }
        public string design_name { get; set; }
        public string por_no { get; set; }
    }

    public class PorReqView
    {
        public DateTime req_date { get; set; }
    }

    public class OrderView
    {
        public string entity { get; set; }
        public string entity_name { get; set; }
        public string por_no { get; set; }
        public DateTime por_date { get; set; }
        public DateTime req_date { get; set; }
        public string ord_type { get; set; }
        public string ord_type_name { get; set; }
        public string por_type { get; set; }
        public string por_type_name { get; set; }
        public string cust_code { get; set; }
        public string cust_name { get; set; }
        public string dept { get; set; }
        public string dept_name { get; set; }
        public string country { get; set; }
        public string ref_no { get; set; }
        public string por_priority { get; set; }
        public string wh_code { get; set; }
        public string wh_name { get; set; }
        public string grp_code { get; set; }
        public string por_remark { get; set; }
        public List<OrderDetailView> orderDetail { get; set; }
        public List<OrderSpecialView> orderSpecial { get; set; }
        public List<OrderRemarkView> remark { get; set; }
    }

    public class OrderDetailView
    {   
        public string por_no { get; set; }
        public int line_no { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string packaging { get; set; }
        public string pdgrp_code { get; set; }
        public string pdcolor_code { get; set; }
        public string design { get; set; }
        public string pdsize_code { get; set; }
        public string uom { get; set; }
        public int qty_ord { get; set; }
        public string gplabel_no { get; set; }
        public string skb_flag { get; set; }
        public string dsgn_no { get; set; }
        public string sd_no { get; set; }
        public List<SubProductView> subProduct { get; set; }
    }

    public class SubProductView
    {
        public string por_no { get; set; }
        public int item { get; set; }
        public string bom_code { get; set; }
        public string description { get; set; }
        public string size { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public int height { get; set; }
        public string size_uom { get; set; }
        public int pack { get; set; }
        public int qty_ord { get; set; }
        public string uom_code { get; set; }
        
    }

    public class OrderSpecialView
    {
        public string por_no { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string spc_desc { get; set; }
    }

    public class SpecialDescView
    {
        public string spc_desc { get; set; }
    }

    public class OrderRemarkView
    {
        public int line_no { get; set; }
        public string trcmt_desc { get; set; }
    }

    
}