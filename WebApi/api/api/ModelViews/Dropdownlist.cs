using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.ModelViews
{
    public class Dropdownlist
    {
        public string key { get; set; }
        public string value { get; set; }
        public string parentKey { get; set; }

    }

    public class Dropdownlist<T>
    {
        public T key { get; set; }
        public string value { get; set; }
    }
}