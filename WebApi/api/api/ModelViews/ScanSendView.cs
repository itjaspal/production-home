using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ScanSendView
    {
        public string pcs_barcode { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string job_no { get; set; }
        public string set_no { get; set; }
        public int set_qty { get; set; }
        public string req_date { get; set; }
        public string user_id { get; set; }
        public string bar_code { get; set; }
        public string fin_date { get; set; }
    }

    public class ScanSendSearchView
    {
        public string entity { get; set; }
        public string req_date { get; set; }
        public string wc_code { get; set; }
        public string pcs_barcode { get; set; }
        public string user_id { get; set; }
    }

    public class SetNoSearchView
    {
        public string entity { get; set; }
        public string req_date { get; set; }    
    }

    public class SetNoView
    {
        public string set_no { get; set; }
        public string req_date { get; set; }

    }

}