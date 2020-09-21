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
        public string user_id { get; set; }
        public string wc_code { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
    }

    public class JobOperationView
    {
        public string wc_code { get; set; }
        public string wc_name { get; set; }
        public string build_type { get; set; }
        public string req_date { get; set; }
        public int total_build_qty { get; set; }
        public int total_plan_qty { get; set; }
        public int total_calcal_qty { get; set; }
        public int total_act_qty { get; set;}
        public int total_defect_qty { get; set; }
        public int total_diff_qty { get; set; }
        public List<JobOperationDetailView> dataGroups { get; set; }
    }

    public class JobOperationDetailView
    {
        public string por_no { get; set; }
        public string display_group { get; set; }
        public string display_type { get; set; }
        public int build_qty { get; set; }
        public int plan_qty { get; set; }
        public int calcal_qty { get; set; }
        public int act_qty { get; set; }
        public int defect_qty { get; set; }
        public int diff_qty { get; set; }
    }
}