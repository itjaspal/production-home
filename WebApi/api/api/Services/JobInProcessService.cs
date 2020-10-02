using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;

namespace api.Services
{
    public class JobInProcessService : IJobInProcessService
    {
        public JobInProcessView EntryCancel(JobInProcessSearchView model)
        {
            throw new NotImplementedException();  
        }

        public JobInProcessView EntyAdd(JobInProcessSearchView model)
        {
            throw new NotImplementedException();
        }

        public JobInProcessView ScanAdd(JobInProcessSearchView model)
        {
            using (var ctx = new ConXContext())
            {


                String[] strlist = model.bar_code.Split('|');
                string vbar_code = strlist[0];

                string ventity = model.entity;
                string vpdjit_grp = model.pdjit_grp;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;

                string sqlp = "select bar_code , pcs_barcode, prod_tname prod_name ,prod_code";
                sqlp += " from mps_det_wc";
                sqlp += " where  entity = :p_entity";
                sqlp += " and req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                sqlp += " and pdjit_grp = :p_pdjit_grp";
                sqlp += " and wc_code =:p_wc_code";
                sqlp += " and bar_code =:p_bar_code";
                sqlp += " and mps_st =  'N'";
                sqlp += " and rownum = 1";

                JobInProcessView mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("Barcodeไม่ถูกต้อง");
                }


                //define model view
                JobInProcessView view = new ModelViews.JobInProcessView()
                {
                    bar_code = mps_in_process.bar_code,
                    prod_code = mps_in_process.prod_code,
                    prod_name = mps_in_process.prod_name,
                    qty = 1,

                };


                // Update Barcode

                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                            new OracleParameter("p_entity", ventity),
                            new OracleParameter("p_user_id", vuser_id),
                            new OracleParameter("p_fin_by", vuser_id),
                            new OracleParameter("p_pcs_barcode", mps_in_process.pcs_barcode),
                            new OracleParameter("p_wc_code", vwc_code)
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc set mps_st='Y' ,   fin_by =:p_fin_by , fin_date = SYSDATE ,  upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }




                //return data to contoller
                return view;
            }
        }

        public JobInProcessView ScanCancel(JobInProcessSearchView model)
        {
            using (var ctx = new ConXContext())
            {


                String[] strlist = model.bar_code.Split('|');
                string vbar_code = strlist[0];

                string ventity = model.entity;
                string vpdjit_grp = model.pdjit_grp;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;

                string sqlp = "select bar_code , pcs_barcode, prod_tname prod_name ,prod_code";
                sqlp += " from mps_det_wc";
                sqlp += " where  entity = :p_entity";
                sqlp += " and req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                sqlp += " and pdjit_grp = :p_pdjit_grp";
                sqlp += " and wc_code =:p_wc_code";
                sqlp += " and bar_code =:p_bar_code";
                sqlp += " and mps_st =  'Y'";
                sqlp += " and rownum = 1";

                JobInProcessView mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("Barcodeไม่ถูกต้อง");
                }


                //define model view
                JobInProcessView view = new ModelViews.JobInProcessView()
                {
                    bar_code = mps_in_process.bar_code,
                    prod_code = mps_in_process.prod_code,
                    prod_name = mps_in_process.prod_name,
                    qty = 1,

                };


                // Update Barcode

                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                            new OracleParameter("p_entity", ventity),
                            new OracleParameter("p_user_id", vuser_id),
                            new OracleParameter("p_fin_by", vuser_id),
                            new OracleParameter("p_pcs_barcode", mps_in_process.pcs_barcode),
                            new OracleParameter("p_wc_code", vwc_code)
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc set mps_st='N' ,  upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }




                //return data to contoller
                return view;
            }
        }
    }
}