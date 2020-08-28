using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Services
{
    public class DropdownlistService : IDropdownlistService
    {
         private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DropdownlistService));

        public DropdownlistService()
        {

        }

        public List<Dropdownlist<string>> GetDdlBranchStatus()
        {
                //log.Info("Test Info Log");
                List<Dropdownlist<string>> ddl = new List<ModelViews.Dropdownlist<string>>();
                ddl.Add(new ModelViews.Dropdownlist<string>()
                {
                    key = "A",
                    value = "ใช้งาน"
                });
                ddl.Add(new ModelViews.Dropdownlist<string>()
                {
                    key = "I",
                    value = "ไม่ใช้งาน"
                });
                return ddl;
        }

        

        public List<Dropdownlist> GetDdlMobilePrnt()
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select series_no key , series_no||' - '||prnt_point_name value from whmobileprnt_ctl where grp_type = 'SPRING'";
          
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql)
                                            .Select(x=> new Dropdownlist()
                                            {
                                                key = x.key,
                                                value = x.value,
                                            })
                                            .ToList();

                return ddl;
            }
        }


    }
}