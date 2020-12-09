using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ProductionRecSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string doc_date { get; set; }
        public string build_type { get; set; }
        public string doc_no { get; set; }
        public string doc_status { get; set; }
    }

    public class ProductionRecTotalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public int total_rec_qty { get; set; }
        public int total_ptw_qty { get; set; }
        public int total_net_qty { get; set; }
        public List<ProductionRecView> recDetails { get; set; }
    }

    public class ProductionRecView
    {
        public DateTime jit_date { get; set; }
        public string doc_no { get; set; }
        public string doc_code { get; set; }
        public string wh_code { get; set; }
        public string wc_code { get; set; }
        public string wc_name { get; set; }
        public DateTime gen_date { get; set; }
        public string gen_by { get; set; }
        public int conf_qty { get; set; }
        public int ptw_qty { get; set; }
        public int net_qty { get; set; }
    }


    public class ProductionRecDetailSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string entity_code { get; set; }
        public string doc_date { get; set; }
        public string doc_no { get; set; }
        public string build_type { get; set; }
    }

    public class ProductionRecDetailTotalView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int totalItem { get; set; }
        public int total_rec_qty { get; set; }
        public int total_prod_item { get; set; }
        public List<ProductionRecDetailView> recDetails { get; set; }
    }

    public class ProductionRecDetailView
    {
        public string doc_no { get; set; }
        public string wc_code { get; set; }
        public string prod_code { get; set; }
        public string prod_tname { get; set; }
        public int qty_pdt { get; set; }
        public string por_no { get; set; }
        public string uom_code { get; set; }
        public DateTime mps_date { get; set; }
        public List<ProductionRecSetDetailView> setDetails { get; set; }

    }

    public class ProductionRecSetDetailView
    {
        public int pkg_barcode_set { get; set; }
        public int confirm_qty { get; set; }
    }



}