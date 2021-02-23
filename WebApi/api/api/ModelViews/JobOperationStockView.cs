using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class JobOperationStockSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string user_id { get; set; }
        public string wc_code { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
    }

    public class JobOperationStockView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public string wc_code { get; set; }
        public string wc_name { get; set; }
        public string build_type { get; set; }
        public List<PorStockGroupView> porGroups { get; set; }
        public List<DisplayGroupView> displayGroups { get; set; }
    }

    public class DisplayGroupView
    {
        public string disgroup_code { get; set; }
        public string disgroup_desc { get; set; }
    }


    //public class JobOperationStockTotalGroupView
    //{
    //    public DateTime req_date { get; set; }
    //    public string pdjit_grp { get; set; }
    //    public string pdgrp_tname { get; set; }
    //    public int total_build_qty { get; set; }
    //    public int total_plan_qty { get; set; }
    //    public int total_calcal_qty { get; set; }
    //    public int total_act_qty { get; set; }
    //    public int total_cancel_qty { get; set; }
    //    public int total_defect_qty { get; set; }
    //    public int total_diff_qty { get; set; }
    //    public List<JobOperationStockDetailView> dataGroups { get; set; }

    //}
    //public class JobOperationStockDetailView
    //{
    //    public string por_no { get; set; }
    //    public DateTime req_date { get; set; }
    //    public string display_group { get; set; }
    //    public string display_type { get; set; }
    //    public string display_qty { get; set; }

    //}

    public class GroupStockView
    {
        public string disgrp_line_code { get; set; }
        public string disgrp_line_desc { get; set; }

    }

    public class PorStockGroupView
    {
        public string entity { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string design_name { get; set; }
        public int qty { get; set; }
        public List<PorStockGroupDetailView> dataGroups { get; set; }
        
    }

    public class PorStockGroupDetailView
    {
        public string disgroup_code { get; set; }
        public string disgroup_desc { get; set; }
        public string qty { get; set; }
    }


    public class PorTypeDetailView
    {
        public string distype_code { get; set; }
        public string distype_desc { get; set; }
        public int qty { get; set; }
    }


    public class WcDataView
    {
        public string wc_code { get; set; }
        public string wc_name { get; set; }
    }

    public class DisTypeView
    {
        public string distype_code { get; set; }
        public string distype_desc { get; set; }
    }

    public class ProductGroupSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string user_id { get; set; }
        public string wc_code { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
    }

    public class ProductGroupView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<ProductGroupDetailView> datas { get; set; }
    }

    public class ProductGroupDetailView
    {
        public string disgrp_line_desc { get; set; }
        public string distype_desc { get; set; }
        public int qty_plan { get; set; }
        public int qty_fin { get; set; }
        public int qty_defect { get; set; }

    }

    public class ProductionTrackingStockView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public string entity { get; set; }
        public string req_date { get; set; }
        public string build_type { get; set; }
        public List<ProductDataGroupView> productGroups { get; set; }
        public List<DisplayWcGroupView> displayGroups { get; set; }
    }

    public class QtyGroupView
    {
        public int qty_fin { get; set; }
        public int qty_defect { get; set; }
    }

    public class OrderReqSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity { get; set; }
        public string por_no { get; set; }
        public string wc_code { get; set; }
        public string user_id { get; set; }
        public string build_type { get; set; }
    }

    public class OrderReqView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public List<OrderReqDetailView> datas { get; set; }
    }

    public class OrderReqDetailView
    {
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public DateTime req_date { get; set; }
    }

    public class ProductGroupDetailSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string user_id { get; set; }
        public string wc_code { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string group { get; set; }
    }
}