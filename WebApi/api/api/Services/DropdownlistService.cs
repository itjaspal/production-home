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

        public List<Dropdownlist> GetDdlWCPutaWay(string entity, string user)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "SELECT dept_code key, wc_tdesc value from auth_function a, wc_mast b where a.function_id = 'ICPWSCFM' and a.dept_code    = b.wc_code  and a.entity_code = :pentity_code and a.user_id = :puser_id ORDER BY 1";
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql, new OracleParameter("pentity_code", entity), new OracleParameter("puser_id", user))
                                            .Select(x => new Dropdownlist()
                                            {
                                                key = x.key,
                                                value = x.value,
                                            })
                                            .ToList();

                return ddl;
            }
        }

        public List<Dropdownlist> GetDdlPutAwayWHMast()
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select wh_code key,wh_tdesc value from wh_mast where iss_loc_def is not null order by 1";

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

        public List<Dropdownlist> GetDdlPtwSetNoList(string entity, string doc_code, string doc_no)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select job_no key, job_no value from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code group by job_no order by 1";
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql, new OracleParameter("pentity_code", entity), new OracleParameter("doc_no", doc_no), new OracleParameter("pdoc_code", doc_code))
                                            .Select(x => new Dropdownlist()
                                            {
                                                key = x.key,
                                                value = x.value,
                                            })
                                            .ToList();

                return ddl;
            }
        }

        public List<Dropdownlist> GetDdlPtwProdList(string entity, string doc_code, string doc_no)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select prod_code key, prod_code value from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code group by prod_code order by 1";
                List<Dropdownlist> ddl = ctx.Database.SqlQuery<Dropdownlist>(sql, new OracleParameter("pentity_code", entity), new OracleParameter("doc_no", doc_no), new OracleParameter("pdoc_code", doc_code))
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