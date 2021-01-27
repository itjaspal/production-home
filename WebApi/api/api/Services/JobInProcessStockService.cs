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
    public class JobInProcessStockService : IJobInProcessStockService
    {
        public void Cancel(JobInProcessStockView scan)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    // Delete PD_DET_WHCF
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_prod_code", scan.prod_code),
                        new OracleParameter("p_entity", scan.entity),
                        new OracleParameter("p_por_no", scan.por_no),
                        new OracleParameter("p_ref_no", scan.ref_no),
                        new OracleParameter("p_prod_code_sub", scan.sub_prod_code),
                        new OracleParameter("p_qty", scan.qty),
                        new OracleParameter("p_wc_code", scan.wc_code),
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_fin = qty_fin - :p_qty  where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_prod_code_sub";
                    oraCommand.ExecuteNonQuery();


                    conn.Close();

                    scope.Complete();
                }
            }
        }

        public JobInProcessStockView EntryAdd(JobInProcessStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {

                string vprod_code = model.prod_code;
                string vsub_prod_code = model.sub_prod_code;
                int vqty = model.qty;

                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;



                string sql2 = "select prod_name ,prod_code , prod_code_sub sub_prod_code, prod_name_sub sub_prod_name , por_no , ref_no , sum(qty_plan) qty_plan , sum(qty_fin) qty_fin";
                sql2 += " from mps_det_wc_stk";
                sql2 += " where  entity = :p_entity";
                sql2 += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                sql2 += " and wc_code =:p_wc_code";
                sql2 += " and por_no =:p_por_no";
                sql2 += " and ref_no =:p_ref_no";
                sql2 += " and prod_code = :p_prod_code";
                sql2 += " and prod_code_sub = :p_prod_code_sub";
                sql2 += " group by prod_name ,prod_code , prod_code_sub , prod_name_sub , por_no , ref_no";

                JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();
                //JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }

                if (vqty > (mps_in_process.qty_plan - mps_in_process.qty_fin))
                {
                    throw new Exception("บันทึกเกินจำนวน");
                }

                //define model view
                JobInProcessStockView view = new ModelViews.JobInProcessStockView()
                {
                    entity = ventity,
                    prod_code = mps_in_process.prod_code,
                    prod_name = mps_in_process.prod_name,
                    sub_prod_code = mps_in_process.sub_prod_code,
                    sub_prod_name = mps_in_process.sub_prod_name,
                    por_no = mps_in_process.por_no,
                    ref_no = mps_in_process.ref_no,
                    qty = vqty,
                    wc_code = vwc_code,
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
                            new OracleParameter("p_wc_code", vwc_code),
                            new OracleParameter("p_prod_code", vprod_code),
                            new OracleParameter("p_sub_prod_code", vsub_prod_code),
                            new OracleParameter("p_por_no", vpor_no),
                            new OracleParameter("p_ref_no", vref_no),
                            new OracleParameter("p_qty", vqty),
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_fin = qty_fin + :p_qty , fin_by =:p_fin_by , fin_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }

                //return data to contoller
                return view;


            }
        }

        public JobInProcessStockView EntryCancel(JobInProcessStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {

                string vprod_code = model.prod_code;
                string vsub_prod_code = model.sub_prod_code;
                int vqty = model.qty;

                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;



                string sql2 = "select prod_name ,prod_code , prod_code_sub sub_prod_code, prod_name_sub sub_prod_name , por_no , ref_no , sum(qty_plan) qty_plan , sum(qty_fin) qty_fin";
                sql2 += " from mps_det_wc_stk";
                sql2 += " where  entity = :p_entity";
                sql2 += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                sql2 += " and wc_code =:p_wc_code";
                sql2 += " and por_no =:p_por_no";
                sql2 += " and ref_no =:p_ref_no";
                sql2 += " and prod_code = :p_prod_code";
                sql2 += " and prod_code_sub = :p_prod_code_sub";
                sql2 += " group by prod_name ,prod_code , prod_code_sub , prod_name_sub , por_no , ref_no";

                JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();
                //JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }

                if (vqty > mps_in_process.qty_fin)
                {
                    throw new Exception("บันทึกยกเลิกเกินจำนวน");
                }

                //define model view
                JobInProcessStockView view = new ModelViews.JobInProcessStockView()
                {
                    entity = ventity,
                    prod_code = mps_in_process.prod_code,
                    prod_name = mps_in_process.prod_name,
                    sub_prod_code = mps_in_process.sub_prod_code,
                    sub_prod_name = mps_in_process.sub_prod_name,
                    por_no = mps_in_process.por_no,
                    ref_no = mps_in_process.ref_no,
                    qty = vqty,
                    wc_code = vwc_code,
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
                            new OracleParameter("p_wc_code", vwc_code),
                            new OracleParameter("p_prod_code", vprod_code),
                            new OracleParameter("p_sub_prod_code", vsub_prod_code),
                            new OracleParameter("p_por_no", vpor_no),
                            new OracleParameter("p_ref_no", vref_no),
                            new OracleParameter("p_qty", vqty),
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_fin = qty_fin - :p_qty , fin_by =:p_fin_by , fin_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }

                //return data to contoller
                return view;


            }
        }

        public ProductSubModalView getSubProduct(ProductSubSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                ProductSubModalView view = new ModelViews.ProductSubModalView()
                {

                    datas = new List<ModelViews.ProductSubView>()
                };

                string sql = "select distinct prod_code , prod_code_sub sub_prod_code , prod_name_sub sub_prod_name , qty_plan , qty_fin from mps_det_wc_stk where entity= :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code= :p_wc_code and por_no=:p_por_no and ref_no= :p_ref_no and qty_plan > qty_fin order by prod_code_sub , prod_code";
                List<ProductSubView> prod = ctx.Database.SqlQuery<ProductSubView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.ProductSubView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_code = i.sub_prod_code,
                        sub_prod_name = i.sub_prod_name,
                        qty_plan = i.qty_plan,
                        qty_fin = i.qty_fin,

                    });
                }


                //return data to contoller
                return view;


            }
        }

        public ProductSubModalView getSubProductCancel(ProductSubSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                ProductSubModalView view = new ModelViews.ProductSubModalView()
                {

                    datas = new List<ModelViews.ProductSubView>()
                };

                string sql = "select distinct prod_code , prod_code_sub sub_prod_code , prod_name_sub sub_prod_name , qty_plan , qty_fin from mps_det_wc_stk where entity= :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code= :p_wc_code and por_no=:p_por_no and ref_no= :p_ref_no and  qty_fin > 0 order by prod_code , prod_code_sub";
                List<ProductSubView> prod = ctx.Database.SqlQuery<ProductSubView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.ProductSubView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_code = i.sub_prod_code,
                        sub_prod_name = i.sub_prod_name,
                        qty_plan = i.qty_plan,
                        qty_fin = i.qty_fin,

                    });
                }


                //return data to contoller
                return view;


            }
        }

        public JobInProcessStockView ScanAdd(JobInProcessStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {


                String[] strlist = model.scan_data.Split('|');
                string vprod_code = strlist[0];
                string vsub_prod_code = strlist[1];
                //int vqty = Int32.Parse(strlist[4]);
                int vqty = model.qty;
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;



                string sql2 = "select prod_name ,prod_code , prod_code_sub sub_prod_code, prod_name_sub sub_prod_name , por_no , ref_no , sum(qty_plan) qty_plan , sum(qty_fin) qty_fin";
                sql2 += " from mps_det_wc_stk";
                sql2 += " where  entity = :p_entity";
                sql2 += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                sql2 += " and wc_code =:p_wc_code";
                sql2 += " and por_no =:p_por_no";
                sql2 += " and ref_no =:p_ref_no";
                sql2 += " and prod_code = :p_prod_code";
                sql2 += " and prod_code_sub = :p_prod_code_sub";
                sql2 += " group by prod_name ,prod_code , prod_code_sub , prod_name_sub , por_no , ref_no";

                JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();
                //JobInProcessStockView mps_in_process = ctx.Database.SqlQuery<JobInProcessStockView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }

                if (vqty > (mps_in_process.qty_plan - mps_in_process.qty_fin))
                {
                    throw new Exception("บันทึกเกินจำนวน");
                }

                //define model view
                JobInProcessStockView view = new ModelViews.JobInProcessStockView()
                {
                    entity = ventity,
                    prod_code = mps_in_process.prod_code,
                    prod_name = mps_in_process.prod_name,
                    sub_prod_code = mps_in_process.sub_prod_code,
                    sub_prod_name = mps_in_process.sub_prod_name,
                    por_no = mps_in_process.por_no,
                    ref_no = mps_in_process.ref_no,
                    qty = vqty,
                    wc_code = vwc_code,
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
                            new OracleParameter("p_wc_code", vwc_code),
                            new OracleParameter("p_prod_code", vprod_code),
                            new OracleParameter("p_sub_prod_code", vsub_prod_code),
                            new OracleParameter("p_por_no", vpor_no),
                            new OracleParameter("p_ref_no", vref_no),
                            new OracleParameter("p_qty", vqty),
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_fin = qty_fin + :p_qty , fin_by =:p_fin_by , fin_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }

                //return data to contoller
                return view;


            }
        }

        public JobOperationStockView SearchJobInPorcessDefect(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;
                string vwc_code = model.wc_code;

                //int total_plan_qty = 0;


                //DateTime req_tmp = DateTime.Now;

                //string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                //WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();
                string sql1 = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string vwc_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("param1", vwc_code)).SingleOrDefault();

                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = vwc_code,
                    wc_name = vwc_name,
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , nvl(sum(qty_defect),0) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select nvl(sum(a.qty_defect),0) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }


                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,

                        };

                        displayGroupViews.Add(dView);


                    }


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,


                    });


                }

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationStockView SearchJobInPorcessFin(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;
                string vwc_code = model.wc_code;

                //int total_plan_qty = 0;


                //DateTime req_tmp = DateTime.Now;

                //string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                //WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();
                string sql1 = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string vwc_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("param1", vwc_code)).SingleOrDefault();

                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = vwc_code,
                    wc_name = vwc_name,
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_fin) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select sum(a.qty_fin) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }


                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,

                        };

                        displayGroupViews.Add(dView);


                    }


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,


                    });


                }

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationStockView SearchJobInPorcessPlan(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;
                string vwc_code = model.wc_code;
                //int total_plan_qty = 0;


                //DateTime req_tmp = DateTime.Now;

                //string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                //WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();
                string sql1 = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string vwc_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("param1", vwc_code)).SingleOrDefault();

                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = vwc_code,
                    wc_name = vwc_name,
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_plan) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select sum(a.qty_plan) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }


                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,

                        };

                        displayGroupViews.Add(dView);


                    }


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,


                    });


                }

                view.displayGroups = displayGroupViews;


                return view;
            }
        }
    }
}