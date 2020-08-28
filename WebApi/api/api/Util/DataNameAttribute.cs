using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Util
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataNamesAttribute : Attribute
    {
        protected List<string> _columnNames { get; set; }

        public List<string> ColumnNames
        {
            get
            {
                return _columnNames;
            }
            set
            {
                _columnNames = value;
            }
        }

        public DataNamesAttribute()
        {
            _columnNames = new List<string>();
        }

        public DataNamesAttribute(params string[] columnNames)
        {
            _columnNames = columnNames.ToList();
        }
    }
}