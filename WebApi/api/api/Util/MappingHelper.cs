﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace api.Util
{
    public class MappingHelper
    {
        public static List<string> GetDataNames(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType() == typeof(DataNamesAttribute)).FirstOrDefault();
            if (property != null)
            {
                return ((DataNamesAttribute)property).ColumnNames;
            }
            return new List<string>();
        }

        /// <summary>
        /// Currently handles string, int, long, DateTime, decimal, double, bool
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
            }
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, int.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, long.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<DateTime>))
            {
                DateTime date;
                bool isValid = DateTime.TryParse(value.ToString(), out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
                else
                {
                    //Making an assumption here about the format of dates in the source data.
                    isValid = DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.AssumeLocal, out date);
                    if (isValid)
                    {
                        prop.SetValue(entity, date, null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                prop.SetValue(entity, decimal.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                double number;
                bool isValid = double.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, double.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, bool.Parse(value.ToString()), null);
                }
            }
        }
    }
}