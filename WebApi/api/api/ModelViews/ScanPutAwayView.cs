using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{

    public class BarcodeSetDetailView
    {
        public string ref_pd_docno { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public int pkg_barcode_set { get; set; }
        public int confirm_qty { get; set; }
    }

    public class ScanPutAwaySearchView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public string doc_no { get; set; }
        public string doc_code { get; set; }
        public string doc_date { get; set; }
        public string bar_code { get; set; }
        public string wc_code { get; set; }
        public string fr_wh_code { get; set; }
        public string wh_code { get; set; }
        public string loc_code { get; set; }
        public string user_id { get; set; }
        public int qty { get; set; }

    }

    public class ScanPutAwaysTotalView
    {
        public int total_qty { get; set; }
        public List<ScanPutAwayDetailView> ptwDetails { get; set; }
    }

    public class ScanPutAwayDetailView
    {
        public int item_no { get; set; }
        public int set_no { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public string prod_name { get; set; }
        public string wh_code { get; set; }
        public string loc_code { get; set; }
        public int qty { get; set; }
    }


    public class ScanPutAwayDetailSearchView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string doc_code { get; set; }
        public string set_no { get; set; }
        public string bar_code { get; set; }
        public string prod_code { get; set; }
    }

    public class ScanPutAwayCancelSearchView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string doc_code { get; set; }
        public string item { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
    }

    public class ScanPutAwayCancelView
    {
        public string ic_entity { get; set; }
        public string doc_no { get; set; }
        public string doc_code { get; set; }
        public int item { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public string wh_code { get; set; }
        public string wh_refer { get; set; }
        public string fr_wh_code { get; set; }
        public string loc_code { get; set; }
        public string loc_refer { get; set; }
        public int qty { get; set; }

    }

}