using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace api.Util
{
    public class DataTableMapper<TEntity> where TEntity : class, new()
    {
        public DataTable Map(List<TEntity> entities)
        {
            DataTable oReturn = new DataTable(typeof(TEntity).Name);
            object[] a_oValues;
            int i;

            //#### Collect properties for column name mapping
            PropertyInfo[] a_oProperties = (typeof(TEntity)).GetProperties()
                                              .Where(f => !f.GetGetMethod().IsVirtual)
                                              .Where(x => !x.IsDefined(typeof(DataIgnoreAttribute), true))
                                              .ToArray();

            Type type = typeof(TEntity);
            foreach (PropertyInfo oProperty in a_oProperties)
            {
                List<string> columnNames = MappingHelper.GetDataNames(type, oProperty.Name);
                string columnDataName = oProperty.Name;

                if (columnNames != null && columnNames.Count() > 0)
                {
                    columnDataName = columnNames.First();
                }

                if (oProperty.PropertyType == typeof(DateTime) ||
                    oProperty.PropertyType == typeof(Nullable<DateTime>))
                {
                    oReturn.Columns.Add(columnDataName, typeof(string));
                }
                else
                {
                    oReturn.Columns.Add(columnDataName, BaseType(oProperty.PropertyType));
                }
            }

            //#### Traverse the l_oItems
            foreach (TEntity oItem in entities)
            {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[a_oProperties.Length];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < a_oProperties.Length; i++)
                {
                    if (a_oProperties[i].PropertyType == typeof(DateTime) ||
                        a_oProperties[i].PropertyType == typeof(Nullable<DateTime>))
                    {
                        var oValue = a_oProperties[i].GetValue(oItem, null);
                        string dtFormat = string.Format("'{0:yyyy-MM-dd hh:mm:ss}", oValue);
                        a_oValues[i] = dtFormat;
                    }
                    else if (a_oProperties[i].PropertyType == typeof(string))
                    {
                        int n;
                        var oValue = a_oProperties[i].GetValue(oItem, null);

                        if (oValue != null && int.TryParse(oValue.ToString(), out n))
                        {
                            string dtFormat = string.Format("'{0}", oValue.ToString());
                            a_oValues[i] = dtFormat;
                        }
                        else
                        {
                            a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                        }
                    }
                    else
                    {
                        a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                    }
                }

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }

        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }

        public List<TEntity> Map(DataTable table)
        {
            List<TEntity> entities = new List<TEntity>();
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TEntity)).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList(); //Only get properties that have the SourceNamesAttribute; ignore others
            foreach (DataRow row in table.Rows)
            {
                TEntity entity = new TEntity();
                foreach (var prop in properties)
                {
                    Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        private void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = MappingHelper.GetDataNames(type, prop.Name);
            //Handle .NET Primitives and Structs (e.g. DateTime) here.
            foreach (var columnName in columnNames)
            {
                if (!String.IsNullOrWhiteSpace(columnName) && row.Table.Columns.Contains(columnName))
                {
                    var propertyValue = row[columnName];
                    if (propertyValue != DBNull.Value)
                    {
                        MappingHelper.ParsePrimitive(prop, entity, row[columnName]);
                        break; //Assumes that the first matching column contains the source data
                    }
                }
            }
        }
    }
}