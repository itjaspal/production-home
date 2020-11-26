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
    public class ScanDefectService : IScanDefectService
    {
        public void Cancel(ScanDefectView scan)
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
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_defect = nvl(qty_defect,0) - :p_qty  where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_prod_code_sub";
                    oraCommand.ExecuteNonQuery();


                    // Delete PD_QC_MAST
                    OracleCommand oraCommanddel1 = conn.CreateCommand();
                    OracleParameter[] paramdel1 = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", scan.entity),
                        new OracleParameter("p_doc_no", scan.por_no),
                        new OracleParameter("p_item_no", scan.item_no),

                    };
                    oraCommanddel1.BindByName = true;
                    oraCommanddel1.Parameters.AddRange(paramdel1);
                    oraCommanddel1.CommandText = "delete pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and item_no = :p_item_no and qc_process='FG'";

                    oraCommanddel1.ExecuteNonQuery();



                    conn.Close();

                    scope.Complete();
                }
            }
        }

        public ScanDefectView EntryAdd(ScanDefectSearchView model)
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

                ScanDefectView mps_in_process = ctx.Database.SqlQuery<ScanDefectView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }

                //if (vqty > mps_in_process.qty_fin)
                //{
                //    throw new Exception("บันทึกเกินจำนวนผลิตเสร็จ");
                //}

                string sql3 = "select nvl(max(item_no),0)+1 from pd_qc_mast where pd_entity= :p_entity and doc_no=:p_por_no and qc_process='FG'";
                int seq = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vpor_no)).FirstOrDefault();


                //define model view
                ScanDefectView view = new ModelViews.ScanDefectView()
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
                    item_no = seq
                
                };


                

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
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_defect = nvl(qty_defect,0) + :p_qty , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";

                    oraCommand.ExecuteNonQuery();


                    //Insert into PD_QC_MAST
                    OracleCommand oraCommandqc = conn.CreateCommand();
                    OracleParameter[] paramqc = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", seq),
                        new OracleParameter("p_build_type", model.build_type),
                        new OracleParameter("p_prod_code", model.sub_prod_code),
                        new OracleParameter("p_ref_prod_code", model.prod_code),
                        new OracleParameter("p_ref_no", model.ref_no),
                        new OracleParameter("p_no_pass_qty", model.qty),
                        new OracleParameter("p_upd_by", model.user_id),
                        new OracleParameter("p_cre_by", model.user_id)
                    };
                    oraCommandqc.BindByName = true;
                    oraCommandqc.Parameters.AddRange(paramqc);
                    oraCommandqc.CommandText = "insert into pd_qc_mast (pd_entity , doc_no , item_no , prod_code, build_type , qc_process , ref_por_no , qc_date , no_pass_qty , upd_by , upd_date , cre_by , cre_date , ref_prod_code) values (:p_entity , :p_doc_no , :p_item_no , :p_prod_code, :p_build_type , 'FG' , :p_ref_no , sysdate , :p_no_pass_qty , :p_upd_by , sysdate , :p_cre_by , sysdate , :p_ref_prod_code)";

                    oraCommandqc.ExecuteNonQuery();

                    conn.Close();


                    scope.Complete();
                }

                //return data to contoller
                return view;


            }
        }

        public ScanDefectView EntryCancel(ScanDefectSearchView model)
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
                int vitem_no = model.item_no; 



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

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }



                //define model view
                ScanDefectView view = new ModelViews.ScanDefectView()
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
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_defect = qty_defect - :p_qty , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";

                    oraCommand.ExecuteNonQuery();


                    // Delete PD_QC_MAST
                    OracleCommand oraCommanddel1 = conn.CreateCommand();
                    OracleParameter[] paramdel1 = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", ventity),
                        new OracleParameter("p_doc_no", vpor_no),
                        new OracleParameter("p_item_no", vitem_no),

                    };
                    oraCommanddel1.BindByName = true;
                    oraCommanddel1.Parameters.AddRange(paramdel1);
                    oraCommanddel1.CommandText = "delete pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and item_no = :p_item_no and qc_process='FG'";

                    oraCommanddel1.ExecuteNonQuery();



                    conn.Close();


                    scope.Complete();
                }

                

                //return data to contoller
                return view;

            }
        }

        public void EntryRemark(DataQcCuttingView model)
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
                        
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_por_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),
                        new OracleParameter("p_remark1", model.remark1),
                        new OracleParameter("p_remark2", model.remark2),
                        new OracleParameter("p_remark3", model.remark3),
                        new OracleParameter("p_user_id", model.user_id),
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update pd_qc_mast set remark4 = :p_remark1 , remark5 = :p_remark2 ,remark6 = :p_remark3 , upd_by = :p_user_id , upd_date = sysdate   where pd_entity = :p_entity  and doc_no = :p_por_no and item_no = :p_item_no and qc_process='FG'";
                    oraCommand.ExecuteNonQuery();


                   
                    conn.Close();

                    scope.Complete();
                }
            }
        }

        public DefectProductSubModalView getSubProduct(DefectProductSubSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                DefectProductSubModalView view = new ModelViews.DefectProductSubModalView()
                {

                    datas = new List<ModelViews.DefectProductSubView>()
                };

                string sql = "select distinct prod_code , prod_code_sub sub_prod_code , prod_name_sub sub_prod_name , qty_plan , qty_fin from mps_det_wc_stk where entity= :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and wc_code= :p_wc_code and por_no=:p_por_no and ref_no= :p_ref_no  order by prod_code_sub , prod_code";
                List<ProductSubView> prod = ctx.Database.SqlQuery<ProductSubView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.DefectProductSubView()
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

        public DefectProductSubModalView getSubProductCancel(DefectProductSubSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                DefectProductSubModalView view = new ModelViews.DefectProductSubModalView()
                {

                    datas = new List<ModelViews.DefectProductSubView>()
                };

                string sql = "select a.item_no, a.ref_prod_code prod_code, a.prod_code sub_prod_code , a.no_pass_qty qty_defect , b.bom_name sub_prod_name from pd_qc_mast a , bm_sub_bom_code b  where  a.prod_Code = b.bom_code and a.pd_entity = :p_entity and a.doc_no = :p_por_no and a.ref_por_no = :p_ref_no and qc_process = 'FG'";
                List<DefectProductSubView> prod = ctx.Database.SqlQuery<DefectProductSubView>(sql, new OracleParameter("p_entity", ventity) , new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.DefectProductSubView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_code = i.sub_prod_code,
                        sub_prod_name = i.sub_prod_name,
                        qty_defect = i.qty_defect,
                        item_no = i.item_no,

                    });
                }


                //return data to contoller
                return view;


            }

            
        }

        public DefectProductSubModalView getSummaryDefect(DefectProductSubSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                DefectProductSubModalView view = new ModelViews.DefectProductSubModalView()
                {

                    datas = new List<ModelViews.DefectProductSubView>()
                };

                string sql = "select a.ref_prod_code prod_code, a.prod_code sub_prod_code , sum(a.no_pass_qty) qty_defect , b.bom_name sub_prod_name from pd_qc_mast a , bm_sub_bom_code b  where  a.prod_Code = b.bom_code and a.pd_entity = :p_entity and a.doc_no = :p_por_no and a.ref_por_no = :p_ref_no and qc_process = 'FG' group by a.ref_prod_code ,a.prod_code ,b.bom_name";
                List<DefectProductSubView> prod = ctx.Database.SqlQuery<DefectProductSubView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.DefectProductSubView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_code = i.sub_prod_code,
                        sub_prod_name = i.sub_prod_name,
                        qty_defect = i.qty_defect,
                       

                    });
                }


                //return data to contoller
                return view;


            }
        }

        public ScanDefectView ScanAdd(ScanDefectSearchView model)
        {
            using (var ctx = new ConXContext())
            {


                String[] strlist = model.scan_data.Split('|');
                string vprod_code = strlist[0];
                string vsub_prod_code = strlist[1];
                int vqty_qr = Int32.Parse(strlist[4]);

                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;
                int vqty = model.qty;
                string vbuild_type = model.build_type;

                if(vqty == 0)
                {
                    vqty = vqty_qr;
                }



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

                ScanDefectView mps_in_process = ctx.Database.SqlQuery<ScanDefectView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", model.wc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_prod_code_sub", vsub_prod_code)).FirstOrDefault();

                if (mps_in_process == null)
                {
                    throw new Exception("ไม่พบข้อมูลรายการสินค้านี้ กรุณาตรวจสอบ");
                }

                //if (vqty > mps_in_process.qty_fin)
                //{
                //    throw new Exception("บันทึกเกินจำนวนที่ผลิตเสร็จ");
                //}

                string sql3 = "select nvl(max(item_no),0)+1 from pd_qc_mast where pd_entity= :p_entity and doc_no=:p_por_no and qc_process='FG'";
                int seq = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vpor_no)).FirstOrDefault();

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
                    oraCommand.CommandText = "update mps_det_wc_stk set qty_defect = nvl(qty_defect,0) + :p_qty , upd_by =:p_user_id , upd_date = SYSDATE where entity = :p_entity  and wc_code =:p_wc_code and por_no = :p_por_no and ref_no = :p_ref_no and prod_code = :p_prod_code and prod_code_sub = :p_sub_prod_code";

                    oraCommand.ExecuteNonQuery();

                    //Insert into PD_QC_MAST
                    OracleCommand oraCommandqc = conn.CreateCommand();
                    OracleParameter[] paramqc = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", ventity),
                        new OracleParameter("p_doc_no", vpor_no),
                        new OracleParameter("p_item_no", seq),
                        new OracleParameter("p_build_type", vbuild_type),
                        new OracleParameter("p_prod_code", vsub_prod_code),
                        new OracleParameter("p_ref_prod_code", vprod_code),
                        new OracleParameter("p_ref_no", vref_no),
                        new OracleParameter("p_no_pass_qty", vqty),                       
                        new OracleParameter("p_upd_by", vuser_id),
                        new OracleParameter("p_cre_by",vuser_id)
                    };
                    oraCommandqc.BindByName = true;
                    oraCommandqc.Parameters.AddRange(paramqc);
                    oraCommandqc.CommandText = "insert into pd_qc_mast (pd_entity , doc_no , item_no , prod_code, build_type , qc_process , ref_por_no , qc_date , no_pass_qty , upd_by , upd_date , cre_by , cre_date , ref_prod_code) values (:p_entity , :p_doc_no , :p_item_no , :p_prod_code, :p_build_type , 'FG' , :p_ref_no , sysdate , :p_no_pass_qty , :p_upd_by , sysdate , :p_cre_by , sysdate ,:p_ref_prod_code)";

                    oraCommandqc.ExecuteNonQuery();


                    conn.Close();


                    scope.Complete();
                }

                //define model view
                ScanDefectView view = new ModelViews.ScanDefectView()
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
                    item_no = seq
                };

                //return data to contoller
                return view;


            }
        }
    }
}