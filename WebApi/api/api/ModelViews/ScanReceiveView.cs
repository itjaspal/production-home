﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ScanReceiveDataView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public int total_qty_pdt { get; set; }
        public int total_qty_rec { get; set; }
        public List<ScanReceiveDataDetailView> datas { get; set; }
    }

    public class ScanReceiveDataDetailView
    {
        public string entity { get; set; }
        public string req_date { get; set; }
        public string doc_no { get; set; }
        public string wc_code { get; set; }
        public string gen_by { get; set; }
        public string gen_date { get; set; }
        public int qty_pdt { get; set; }
        public int qty_rec { get; set; }
        public string doc_status { get; set; }
        public string user_id { get; set; }
    }

    public class ScanReceiveDataSearchView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string doc_date { get; set; }
        public string send_type { get; set; } 
        public string build_type { get; set; }
        public string user_id { get; set; }
    }

    public class ScanReceiveSearchView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string scan_type { get; set; }
        public string scan_data { get; set; }
        public int scan_qty { get; set; }
        public string user_id { get; set; }
    }

    public class ScanReceiveView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string set_no { get; set; }
        public int line_no { get; set; }
        public string prod_code { get; set; }
        public int qty { get; set; }
    }

    public class ScanDataView
    {
        public string set_no { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public int scan_qty { get; set; }
    }

    public class ScanPcsData
    {
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public int qty { get; set; }
    }

}