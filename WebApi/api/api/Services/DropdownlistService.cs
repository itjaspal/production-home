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

        

        public List<Dropdownlist> GetDdlMobilePrntJIT()
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select series_no key , series_no||' - '||prnt_point_name value from whmobileprnt_ctl where grp_type = 'HMJIT'";
          
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

        public List<Dropdownlist> GetDdlMobilePrntSTK()
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select series_no key , series_no||' - '||prnt_point_name value from whmobileprnt_ctl where grp_type = 'HMSTK'";

                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql)
                                            .Select(x => new Dropdownlist()
                                            {
                                                key = x.key,
                                                value = x.value,
                                            })
                                            .ToList();

                return ddl;
            }
        }


        public List<Dropdownlist> GetDdlWCInprocess(string user)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select a.dept_code key , b.wc_tdesc value from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.doc_code <> 'Y' and a.user_id=:p_user_id";
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql ,new OracleParameter("p_user_id", user))
                                            .Select(x => new Dropdownlist()
                                            {
                                                key = x.key,
                                                value = x.value,
                                            })
                                            .ToList();

                return ddl;
            }
        }

        public List<Dropdownlist> GetDdlWCSend(string user)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select a.dept_code key , b.wc_tdesc value from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.doc_code = 'Y' and a.user_id=:p_user_id";
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql, new OracleParameter("p_user_id", user))
                                            .Select(x => new Dropdownlist()
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