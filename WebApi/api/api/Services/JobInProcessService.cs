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
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vreq_date = model.req_date;
                var vwc_code = model.wc_code;
                var vuser_id = model.user_id;
                var vpdjit_grp = model.pdjit_grp;
                var vqty = model.qty;
                var vbar_code = model.bar_code;
                var vprod_code = "";
                var vprod_name = "";

                string sqlw = "select d.wc_next from PD_WCCTL_SEQ d where d.pd_entity = :p_entity and d.wc_code = :p_wc_code";

                string vnext_wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code))
                            .FirstOrDefault();

                if (vnext_wc == null)
                {
                    string sqlc = "select count(*)";
                    sqlc += " from mps_det_wc";
                    sqlc += " where  entity = :p_entity";
                    sqlc += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlc += " and pdjit_grp = :p_pdjit_grp";
                    sqlc += " and wc_code =:p_wc_code";
                    sqlc += " and mps_st =  'Y'";


                    int cnt = ctx.Database.SqlQuery<int>(sqlc, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault(); ;


                    if (vqty > cnt)
                    {
                        throw new Exception("ยกเลิกจำนวนเกินผลผลิต");
                    }

                    string sqlp = "select bar_code , pcs_barcode, prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'Y'";
                    sqlp += " and rownum <= :p_qty";

                    List<JobInProcessView> mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_qty", vqty)).ToList();


                    // Update Barcode

                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();
                        foreach (var i in mps_in_process)
                        {
                            vprod_code = i.prod_code;
                            vprod_name = i.prod_name;

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_user_id", vuser_id),
                                new OracleParameter("p_fin_by", vuser_id),
                                new OracleParameter("p_pcs_barcode", i.pcs_barcode),
                                new OracleParameter("p_wc_code", vwc_code)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "update mps_det_wc set mps_st='N' , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                            oraCommand.ExecuteNonQuery();
                        }
                        conn.Close();


                        scope.Complete();


                    }

                    //define model view
                    JobInProcessView view = new ModelViews.JobInProcessView()
                    {
                        bar_code = vbar_code,
                        prod_code = vprod_code,
                        prod_name = vprod_name,
                        qty = vqty,
                    };

                    //return data to contoller
                    return view;
                }
                else
                {

                    string sqlc = "select count(*)";
                    sqlc += " from mps_det_wc";
                    sqlc += " where  entity = :p_entity";
                    sqlc += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlc += " and pdjit_grp = :p_pdjit_grp";
                    sqlc += " and wc_code =:p_wc_code";
                    sqlc += " and bar_code =:p_bar_code";
                    sqlc += " and mps_st =  'Y'";
                    sqlc += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlc += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlc += " and b.entity = :p_entity2";
                    sqlc += " and b.wc_code = :p_next_wc";
                    sqlc += " and b.pdjit_grp = :p_pdjit_grp2";
                    sqlc += " and b.mps_st = 'N')";


                    int cnt = ctx.Database.SqlQuery<int>(sqlc, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_next_wc", vnext_wc), new OracleParameter("p_pdjit_grp2", vpdjit_grp)).FirstOrDefault();


                    if (vqty > cnt)
                    {
                        throw new Exception("ยกเลิกจำนวนเกินผลผลิต");
                    }

                    string sqlp = "select bar_code , pcs_barcode, prod_name prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'Y'";
                    sqlp += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlp += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlp += " and b.entity = :p_entity2";
                    sqlp += " and b.wc_code = :p_next_wc";
                    sqlp += " and b.pdjit_grp = :p_pdjit_grp2";
                    sqlp += " and b.mps_st = 'N')";
                    sqlp += " and rownum <= :p_qty";

                    List<JobInProcessView> mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_next_wc", vnext_wc), new OracleParameter("p_pdjit_grp2", vpdjit_grp), new OracleParameter("p_qty", vqty)).ToList();




                    // Update Barcode

                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();
                        foreach (var i in mps_in_process)
                        {
                            vprod_code = i.prod_code;
                            vprod_name = i.prod_name;

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_user_id", vuser_id),
                                new OracleParameter("p_fin_by", vuser_id),
                                new OracleParameter("p_pcs_barcode", i.pcs_barcode),
                                new OracleParameter("p_wc_code", vwc_code)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "update mps_det_wc set mps_st='N' , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                            oraCommand.ExecuteNonQuery();
                        }
                        conn.Close();


                        scope.Complete();
                    }

                    JobInProcessView view = new ModelViews.JobInProcessView()
                    {
                        bar_code = vbar_code,
                        prod_code = vprod_code,
                        prod_name = vprod_name,
                        qty = vqty,

                    };

                    //return data to contoller
                    return view;

                }
            }
        }

        public JobInProcessView EntryAdd(JobInProcessSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vreq_date = model.req_date;
                var vwc_code = model.wc_code;
                var vuser_id = model.user_id;
                var vpdjit_grp = model.pdjit_grp;
                var vqty = model.qty;
                var vbar_code = model.bar_code;
                var vprod_code = "";
                var vprod_name = "";

                string sqlw = "select d.WC_PREV from PD_WCCTL_SEQ d where d.pd_entity = :p_entity and d.wc_code = :p_wc_code";

                string vprev_wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code))
                            .FirstOrDefault();

                if (vprev_wc == null)
                {
                    string sqlc = "select count(*)";
                    sqlc += " from mps_det_wc";
                    sqlc += " where  entity = :p_entity";
                    sqlc += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlc += " and pdjit_grp = :p_pdjit_grp";
                    sqlc += " and wc_code =:p_wc_code";
                    sqlc += " and mps_st =  'N'";
                   

                    int cnt = ctx.Database.SqlQuery<int>(sqlc, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault(); ;


                    if (vqty > cnt)
                    {
                        throw new Exception("บันทึกจำนวนเกินผลผลิต");
                    }

                    string sqlp = "select bar_code , pcs_barcode, prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'N'";
                    sqlp += " and rownum <= :p_qty";

                    List<JobInProcessView> mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_qty", vqty)).ToList();


                    // Update Barcode

                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();
                        foreach (var i in mps_in_process)
                        {
                            vprod_code = i.prod_code;
                            vprod_name = i.prod_name;

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_user_id", vuser_id),
                                new OracleParameter("p_fin_by", vuser_id),
                                new OracleParameter("p_pcs_barcode", i.pcs_barcode),
                                new OracleParameter("p_wc_code", vwc_code)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "update mps_det_wc set mps_st='Y' ,   fin_by =:p_fin_by , fin_date = SYSDATE ,  upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                            oraCommand.ExecuteNonQuery();
                        }
                        conn.Close();


                        scope.Complete();

                       
                    }

                    //define model view
                    JobInProcessView view = new ModelViews.JobInProcessView()
                    {
                        bar_code = vbar_code,
                        prod_code = vprod_code,
                        prod_name = vprod_name,
                        qty = vqty,
                    };

                    //return data to contoller
                    return view;
                }
                else
                {

                    string sqlc = "select count(*)";
                    sqlc += " from mps_det_wc";
                    sqlc += " where  entity = :p_entity";
                    sqlc += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlc += " and pdjit_grp = :p_pdjit_grp";
                    sqlc += " and wc_code =:p_wc_code";
                    sqlc += " and bar_code =:p_bar_code";
                    sqlc += " and mps_st =  'N'";
                    sqlc += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlc += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlc += " and b.entity = :p_entity2";
                    sqlc += " and b.wc_code = :p_prev_wc";
                    sqlc += " and b.pdjit_grp = :p_pdjit_grp2";
                    sqlc += " and b.mps_st = 'Y')";
                    

                    int cnt = ctx.Database.SqlQuery<int>(sqlc, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_prev_wc", vprev_wc), new OracleParameter("p_pdjit_grp2", vpdjit_grp)).FirstOrDefault(); ;


                    if (vqty > cnt)
                    {
                        throw new Exception("บันทึกจำนวนเกินผลผลิต");
                    }

                    string sqlp = "select bar_code , pcs_barcode, prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'N'";
                    sqlp += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlp += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlp += " and b.entity = :p_entity2";
                    sqlp += " and b.wc_code = :p_prev_wc";
                    sqlp += " and b.pdjit_grp = :p_pdjit_grp2";
                    sqlp += " and b.mps_st = 'Y')";
                    sqlp += " and rownum <= :p_qty";

                    List<JobInProcessView> mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_prev_wc", vprev_wc), new OracleParameter("p_pdjit_grp2", vpdjit_grp), new OracleParameter("p_qty", vqty)).ToList();

                                


                    // Update Barcode

                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();
                        foreach (var i in mps_in_process)
                        {
                            vprod_code = i.prod_code;
                            vprod_name = i.prod_name;

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_user_id", vuser_id),
                                new OracleParameter("p_fin_by", vuser_id),
                                new OracleParameter("p_pcs_barcode", i.pcs_barcode),
                                new OracleParameter("p_wc_code", vwc_code)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "update mps_det_wc set mps_st='Y' ,   fin_by =:p_fin_by , fin_date = SYSDATE ,  upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                            oraCommand.ExecuteNonQuery();
                        }
                        conn.Close();


                        scope.Complete();
                    }

                    JobInProcessView view = new ModelViews.JobInProcessView()
                    {
                        bar_code = vbar_code,
                        prod_code = vprod_code,
                        prod_name = vprod_name,
                        qty = vqty,

                    };

                    //return data to contoller
                    return view;

                }
            }
        }

        public ProductModalView getProduct(ProductSearchView model)
        {
            
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpdjit_grp = model.pdjit_grp;

                ProductModalView view = new ModelViews.ProductModalView()
                {

                    datas = new List<ModelViews.ProductView>()
                };

                string sql = "select distinct prod_code , prod_name , bar_code ,sum(qty_pdt) qty_plan from mps_det_wc where entity = :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code = :p_wc_code and pdjit_grp = :p_pdjit_grp and mps_st='N' group by prod_code , prod_name , bar_code";
                List<ProductView> prod = ctx.Database.SqlQuery<ProductView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                foreach (var i in prod)
                {
                    string sql2 = "select nvl(sum(qty_pdt),0) qty_plan from mps_det_wc where entity = :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code = :p_wc_code and pdjit_grp = :p_pdjit_grp and mps_st='Y'";
                    int qty_act = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).SingleOrDefault();


                    view.datas.Add(new ModelViews.ProductView()
                    {
                      
                        prod_code = i.prod_code,
                        prod_name = i.prod_name,
                        bar_code = i.bar_code,
                        qty_plan = i.qty_plan,
                        qty_act = qty_act,


                    });
                }


                return view;

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

                string sqlw = "select d.WC_PREV from PD_WCCTL_SEQ d where d.pd_entity = :p_entity and d.wc_code = :p_wc_code";

                string vprev_wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code))
                            .FirstOrDefault();

                if(vprev_wc == null)
                {
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
                        throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
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
                else
                {
                    string sqlp = "select bar_code , pcs_barcode, prod_name prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'N'";
                    sqlp += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlp += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlp += " and b.entity = :p_entity2";
                    sqlp += " and b.wc_code = :p_prev_wc";
                    sqlp += " and b.mps_st = 'Y')";
                    sqlp += " and rownum = 1";

                    JobInProcessView mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_prev_wc", vprev_wc)).FirstOrDefault();

                    if (mps_in_process == null)
                    {
                        throw new Exception("Barcode ไม่ถูกต้อง / หน่วยผลิตก่อนหน้ายังไม่บันทึกผลผลิต");
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

                string sqlw = "select d.wc_next from PD_WCCTL_SEQ d where d.pd_entity = :p_entity and d.wc_code = :p_wc_code";

                string vnext_wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code))
                            .FirstOrDefault();

                if(vnext_wc == null)
                {
                    string sqlp = "select bar_code , pcs_barcode, prod_name ,prod_code";
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
                        throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
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
                else
                {
                    string sqlp = "select bar_code , pcs_barcode, prod_name ,prod_code";
                    sqlp += " from mps_det_wc";
                    sqlp += " where  entity = :p_entity";
                    sqlp += " and req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlp += " and pdjit_grp = :p_pdjit_grp";
                    sqlp += " and wc_code =:p_wc_code";
                    sqlp += " and bar_code =:p_bar_code";
                    sqlp += " and mps_st =  'Y'";
                    sqlp += " and pcs_barcode in (select distinct b.pcs_barcode from  mps_det_wc b";
                    sqlp += " where b.req_date = to_date(:p_req_date2,'dd/mm/yyyy')";
                    sqlp += " and b.entity = :p_entity2";
                    sqlp += " and b.wc_code = :p_next_wc";
                    sqlp += " and b.mps_st = 'N')";
                    sqlp += " and rownum = 1";

                    JobInProcessView mps_in_process = ctx.Database.SqlQuery<JobInProcessView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", model.req_date), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_bar_code", vbar_code), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_entity2", ventity), new OracleParameter("p_next_wc", vnext_wc)).FirstOrDefault();

                    if (mps_in_process == null)
                    {
                        throw new Exception("Barcode ไม่ถูกต้อง / หน่วยผลิตถัดไปยังไม่บันทึกยกเลิกผลผลิต");
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

        public CommonSearchView<ProductView> getProductCancel(ProductSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpdjit_grp = model.pdjit_grp;

                CommonSearchView<ProductView> view = new ModelViews.CommonSearchView<ModelViews.ProductView>()
                {

                    datas = new List<ModelViews.ProductView>()
                };

                string sql = "select distinct prod_code , prod_name , bar_code  from mps_det_wc where entity = :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code = :p_wc_code and pdjit_grp = :p_pdjit_grp and mps_st='Y'";
                List<ProductView> prod = ctx.Database.SqlQuery<ProductView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();



                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.ProductView()
                    {
                        prod_code = i.prod_code,
                        prod_name = i.prod_name,
                        bar_code = i.bar_code

                    });
                }


                //return data to contoller
                return view;


            }
        }

       
    }
}