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
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vreq_date = model.req_date;
                var vwc_code = model.wc_code;
                //var vmc_code = model.mc_code;
                var vuser_id = model.user_id;
                var vpdjit_grp = model.pdjit_grp;
                //var vsize_code = model.size_code;
                var vqty = model.qty;


                //ScanSendFinView view = new ModelViews.ScanSendFinView()
                //{
                //    pageIndex = 0,
                //    itemPerPage = 10,
                //    totalItem = 0,


                //    datas = new List<ModelViews.ScanSendDataView>()
                //};



                string sqlp = "select d.WC_PREV from PD_WCCTL_SEQ d where d.pd_entity = :p_entity and d.wc_code = :p_wc_code";

                string vprev_wc = ctx.Database.SqlQuery<string>(sqlp, new OracleParameter("p_entity", model.entity), new OracleParameter("p_wc_code", model.wc_code))
                            .FirstOrDefault();




                string sql = "select a.pcs_barcode , a.prod_code from MPS_DET a , PDMODEL_MAST b , MPS_DET_WC c";
                sql += " where a.req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                sql += " and a.entity  = :p_entity";
                sql += " and a.pdsize_code  = :p_size_code";
                sql += " and b.spring_type  = :p_spring_grp";
                sql += " and c.wc_code  = :p_wc_code";
                sql += " and a.pddsgn_code  = b.pdmodel_code";
                sql += " and a.entity  = c.entity";
                sql += " and a.req_date  = c.req_date";
                sql += " and a.pcs_no  = c.pcs_no";
                //sql += " and c.mps_st  <> 'OCL';
                sql += " and c.mps_st  ='N'";
                //sql += " and rownum = 1";
                sql += " and a.pcs_barcode in (select d.pcs_barcode from  MPS_DET d, PDMODEL_MAST e , MPS_DET_WC f";
                sql += " where d.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                sql += " and d.entity = :p_entity2";
                sql += " and d.pdsize_code = :p_size_code2";
                sql += " and e.spring_type  = :p_spring_grp2";
                sql += " and f.wc_code = :p_prev_wc";
                sql += " and d.pddsgn_code = e.pdmodel_code";
                sql += " and d.entity = f.entity";
                sql += " and d.req_date = f.req_date";
                sql += " and d.pcs_no = f.pcs_no";
                sql += " and f.mps_st = 'Y')";
                sql += " and rownum <= :p_qty";



                List<JobInProcessView> pcs = ctx.Database.SqlQuery<JobInProcessView>(sql, new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_entity", ventity), new OracleParameter("p_spring_grp", vpdjit_grp), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity) , new OracleParameter("p_prev_wc", vprev_wc), new OracleParameter("p_qty", vqty))
                            .ToList();




                using (TransactionScope scope = new TransactionScope())
                {

                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    foreach (var i in pcs)
                    {

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                            new OracleParameter("p_entity", ventity),
                            new OracleParameter("p_user_id", vuser_id),
                            new OracleParameter("p_pcs_barcode", i.pcs_barcode),
                            new OracleParameter("p_wc_code", vwc_code)
                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "update MPS_DET_WC set mps_st='Y' , fin_by =:p_user_id , fin_date = SYSDATE , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";

                        //oraCommand.ExecuteReader(CommandBehavior.SingleRow);
                        oraCommand.ExecuteNonQuery();
                    }

                    conn.Close();


                    scope.Complete();
                    JobInProcessView view = new ModelViews.JobInProcessView()
                    {
                        //bar_code = mps_in_process.bar_code,
                        //prod_code = mps_in_process.prod_code,
                        //prod_name = mps_in_process.prod_name,
                        qty = 1,

                    };

                    //foreach (var i in pcs)
                    //{

                    //    view.datas.Add(new ModelViews.ScanSendDataView()
                    //    {
                    //        pcs_barcode = i.pcs_barcode,
                    //        //pdmodel_code = i.pdmodel_code,
                    //        prod_code = i.prod_code
                    //        //prod_name = i.prod_name

                    //    });
                    //}

                    return view;
                }

                
            }
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

                string sqlp = "select bar_code , pcs_barcode, prod_name prod_name ,prod_code";
                sqlp += " from mps_det_wc";
                sqlp += " where  entity = :p_entity";
                sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                sqlp += " and pdjit_grp = :p_pdjit_grp";
                sqlp += " and wc_code =:p_wc_code";
                sqlp += " and bar_code =:p_bar_code";
                sqlp += " and mps_st =  'N'";
                sqlp += " and rownum = 1";

                JobInProcessView mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("Barcode ไม่ถูกต้อง");
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