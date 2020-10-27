using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class ScanApproveSendView
    {
        public string entity { get; set; }
        public string build_type { get; set; }
        public string user_id { get; set; }
        public int total_set { get; set; }
        public int total_qty { get; set; }
        public List<ScanApproveSendDataView> datas { get; set; }
    }

    public class ScanApproveSendDataView
    {
        public DateTime req_date { get; set; }
        public string pdjit_grp { get; set; }
        public string wc_code { get; set; }
        public string doc_no { get; set; }
        public int set_qty { get; set; }
        public int tot_qty { get; set; }
        public string status { get; set; }
        public string doc_status { get; set; }
    }

    public class ScanApproveSendSearchView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public string fin_date { get; set; }
        public string send_type { get; set; }
        public string build_type { get; set; }
        public string user_id { get; set; }
    }

    public class ScanApproveAddView
    {
        public string entity { get; set; }
        public string doc_no { get; set; }
        public int set_no { get; set; }
        public string build_type {get; set;}
        public string fin_date { get; set; }
        public string user_id { get; set; }
    }

    public class SetDataView
    {
        public string entity { get; set; }
        public string tran_no { get; set; }
        public DateTime tran_date { get; set; }
        public string wc_code { get; set; }
        public string prod_code { get; set; }
        public string bar_code { get; set; }
        public string fin_date { get; set; }
    }

    public class MpsWcDataView
    {
        public string entity { get; set; }
        public DateTime req_date { get; set; } 
        public string wc_code { get; set; }
        public string pcs_barcode { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string bar_code { get; set; }
        public string por_no { get; set; }
        public string ref_no { get; set; }
        public string pdjit_grp { get; set; }
        public string wh_code { get; set; }
        public string doc_code { get; set; }
        public string ord_type { get; set; }
        public string po_entity { get; set; }
    }

    public class ScanApproveFinView
    {
        public string doc_no { get; set; }
        public int set_no { get; set; }
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public int qty { get; set; }
        public string fin_date { get; set; }
    }


}