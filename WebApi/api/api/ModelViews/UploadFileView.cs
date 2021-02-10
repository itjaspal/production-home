using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class UploadFileView
    {
        public string pddsgn_code { get; set; }
        public string pddsgn_name { get; set; }
        public string dsgn_no { get; set; }
        public string dept_code { get; set; }
        public string type { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public string fullPath { get; set; }
        //public string fullPath
        //{
        //    get
        //    {
        //        string urlPrefix = ConfigurationManager.AppSettings["upload.urlPrefix"];
        //        return urlPrefix + this.file_path;
        //    }
        //}
    }

    public class UploadFileSearchView
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public string pddsgn_code { get; set; }
        public string type { get; set; }
    }
}