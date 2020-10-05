using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class JobInProcessView
    {
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public string prod_name { get; set; }
        public string pcs_barcode { get; set; }
        public int qty { get; set; }
    }

    public class JobInProcessSearchView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public string pdjit_grp { get; set; }
        public string bar_code { get; set; }
        public string wc_code { get; set; }
        public string user_id { get; set; }
        public int qty { get; set; }

    }
}