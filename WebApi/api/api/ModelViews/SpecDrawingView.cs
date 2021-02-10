using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class SpecDrawingView
    {
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string bar_code { get; set; }
        public string sd_no { get; set; }
        public string design_no { get; set; }
        public string type { get; set; }
        public string file_path { get; set; }
        public string design_code { get; set; }
        public string file_name { get; set; }
        public string dsgn_no { get; set; }
        public string dept_code { get; set; }
    }

    public class SpecDrawingAllView
    {
        public string prod_code { get; set; }
        public string prod_name { get; set; }
        public string bar_code { get; set; }
        public string sd_no { get; set; }
        public string design_no { get; set; }
        public string type { get; set; }
        public List<SpecDrawinDetailView> datas { get; set; }
    }

    public class SpecDrawinDetailView
    {
        public string design_code { get; set; }
        public string design_name { get; set; }
        public string file_name { get; set; }
        public string dsgn_no { get; set; }
        public string dept_code { get; set; }
    }
}