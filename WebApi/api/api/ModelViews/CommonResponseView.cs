using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class CommonStatus
    {
        public static int SUCCESS = 0;
    }

    public class CommonResponseView<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }

    public class CommonResponseView
    {
        public long oid { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
}