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
    public class ScanApproveSendService : IScanApproveSendService
    {
        public ScanApproveFinView ScanApvSendAdd(ScanApproveAddView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vdoc_no = model.doc_no;
                var vset_no = model.set_no;
                var vbuild_type = model.build_type;
                var vfin_date = model.fin_date;
                var vuser_id = model.user_id;
                

                if (vbuild_type == "HMJIT")
                {
                    string sql1 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no and ref_pd_docno is null";
                    SetDataView set_no = ctx.Database.SqlQuery<SetDataView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                    if (set_no == null)
                    {
                        throw new Exception("Set No. ไม่ถูกต้อง / Set No. มีการส่งมอบแล้ว");
                    }

                    string sql2 = "select entity , req_date , wc_code , pcs_barcode , prod_code , prod_name , por_no , pdjit_grp , wh_code , po_entity , ref_no , doc_code , ord_type , bar_code from mps_det_wc where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code = :p_wc_code";
                    MpsWcDataView mps = ctx.Database.SqlQuery<MpsWcDataView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_pcs_barcode", set_no.tran_no), new OracleParameter("p_wc_code", set_no.wc_code)).FirstOrDefault();


                    string sql4 = "select count(*) from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                    int vqty = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                   
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 15, 0)))
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();


                        string sql6 = "select prod_code from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code and por_no = :p_por_no and plan_no = :p_plan_no";
                        string chkdup = ctx.Database.SqlQuery<string>(sql6, new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", mps.prod_code), new OracleParameter("p_por_no", mps.por_no), new OracleParameter("p_plan_no", mps.ref_no)).FirstOrDefault();


                        if (chkdup == null)
                        {
                            string sql5 = "select count(*)+1 from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no";
                            int vline_no = ctx.Database.SqlQuery<int>(sql5, new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();



                            //Insert into PD_DET
                            OracleCommand oraCommandpd = conn.CreateCommand();
                            OracleParameter[] parampd = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_line_no", vline_no),
                                new OracleParameter("p_bar_code", mps.bar_code),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_wh_code", mps.wh_code),
                                new OracleParameter("p_ref_no", mps.ref_no)
                            };

                            oraCommandpd.BindByName = true;
                            oraCommandpd.Parameters.AddRange(parampd);
                            oraCommandpd.CommandText = "insert into pd_det (pd_entity , doc_no , line_no , bar_code , prod_code , qty_pdt , pd_time , pd_manpower , def_status , por_no , wh_code , plan_no) values (:p_entity , :p_doc_no , :p_line_no , :p_bar_code , :p_prod_code , :p_qty , 0 , 0 , 'N' , :p_por_no , :p_wh_code , :p_ref_no)";

                            oraCommandpd.ExecuteNonQuery();
                        }
                        else
                        {
                            //Insert into PD_DET
                            OracleCommand oraCommandpd = conn.CreateCommand();
                            OracleParameter[] parampd = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_plan_no", mps.ref_no),
                            };

                            oraCommandpd.BindByName = true;
                            oraCommandpd.Parameters.AddRange(parampd);
                            oraCommandpd.CommandText = "update pd_det set qty_pdt = qty_pdt + :p_qty where pd_entity = :p_entity and doc_no = :p_doc_no and por_no = :p_por_no and plan_no = :p_plan_no";

                            oraCommandpd.ExecuteNonQuery();
                        }


                        //Update POR_DET - PO
                        OracleCommand oraCommandpo = conn.CreateCommand();
                        OracleParameter[] parampo = new OracleParameter[]
                        {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                        };

                        oraCommandpo.BindByName = true;
                        oraCommandpo.Parameters.AddRange(parampo);
                        oraCommandpo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) + nvl(:p_qty,0) , fin_date = sysdate where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                        oraCommandpo.ExecuteNonQuery();


                        //Update POR_DET - MO
                        if (mps.ord_type == "MO")
                        {
                            OracleCommand oraCommandmo = conn.CreateCommand();
                            OracleParameter[] parammo = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.ref_no),
                            };

                            oraCommandmo.BindByName = true;
                            oraCommandmo.Parameters.AddRange(parammo);
                            oraCommandmo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) + nvl(:p_qty,0) , fin_date = sysdate where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                            oraCommandmo.ExecuteNonQuery();
                        }

                        //Update MPS_DET_WC
                        string sql7 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                        List<SetDataView> pcs = ctx.Database.SqlQuery<SetDataView>(sql7, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).ToList();

                        foreach (var i in pcs)
                        {
                            OracleCommand oraCommandmps = conn.CreateCommand();
                            OracleParameter[] parammps = new OracleParameter[]
                            {
                                        new OracleParameter("p_doc_no", vdoc_no),
                                        new OracleParameter("p_gen_by", vuser_id),
                                        new OracleParameter("p_entity", mps.po_entity),
                                        new OracleParameter("p_prod_code", mps.prod_code),
                                        new OracleParameter("p_por_no", mps.por_no),
                                        new OracleParameter("p_wc_code", mps.wc_code),
                                        new OracleParameter("p_fin_date", vfin_date),
                                        new OracleParameter("p_pcs_barcode", i.tran_no),
                            };

                            oraCommandmps.BindByName = true;
                            oraCommandmps.Parameters.AddRange(parammps);
                            oraCommandmps.CommandText = "update mps_det_wc set doc_status = 'P' , doc_no = :p_doc_no , gen_by = :p_gen_by , gen_date = sysdate where entity = :p_entity and prod_code = :p_prod_code and por_no = :p_por_no and wc_code = :p_wc_code and gen_date is null and pcs_barcode = :p_pcs_barcode";

                            oraCommandmps.ExecuteNonQuery();
                        }


                        //Update PKG_BARCODE
                        OracleCommand oraCommandbar = conn.CreateCommand();
                        OracleParameter[] parambar = new OracleParameter[]
                        {
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_set_no", vset_no),
                                new OracleParameter("p_fin_date", vfin_date),
                        };

                        oraCommandbar.BindByName = true;
                        oraCommandbar.Parameters.AddRange(parambar);
                        oraCommandbar.CommandText = "update pkg_barcode set ref_pd_docno = :p_doc_no , build_type = 'HMJIT' where entity = :p_entity and pkg_barcode_set = :p_set_no and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy')";

                        oraCommandbar.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }



                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = mps.prod_code,
                        prod_name = mps.prod_name,
                        qty = vqty,
                        fin_date = set_no.fin_date
                    };


                    //return data to contoller
                    return view;

                }
                else   // HMSTK
                {
                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = "",
                        prod_name = "",
                        qty = 0,
                        fin_date = ""
                    };


                    //return data to contoller
                    return view;
                }

            }
        }

        public ScanApproveFinView ScanApvSendCancel(ScanApproveAddView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vdoc_no = model.doc_no;
                var vset_no = model.set_no;
                var vbuild_type = model.build_type;
                var vfin_date = model.fin_date;
                var vuser_id = model.user_id;
                var vwc_code = model.wc_code;


                if (vbuild_type == "HMJIT")
                {
                    string sql1 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no and wc_code = :p_wc_code and ref_pd_docno is not null";
                    SetDataView set_no = ctx.Database.SqlQuery<SetDataView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                    if (set_no == null)
                    {
                        throw new Exception("Set No. ไม่ถูกต้อง / Set No. ยังไม่ได้ Scan ส่งมอบ");
                    }

                    string sql2 = "select entity , req_date , wc_code , pcs_barcode , prod_code , prod_name , por_no , pdjit_grp , wh_code , po_entity , ref_no , doc_code , ord_type , bar_code from mps_det_wc where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code = :p_wc_code";
                    MpsWcDataView mps = ctx.Database.SqlQuery<MpsWcDataView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_pcs_barcode", set_no.tran_no), new OracleParameter("p_wc_code", set_no.wc_code)).FirstOrDefault();


                    string sql4 = "select count(*) from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                    int vqty = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();


                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 15, 0)))
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();


                        string sql6 = "select qty_odt from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code and por_no = :p_por_no and plan_no = :p_plan_no";
                        int chkqty = ctx.Database.SqlQuery<int>(sql6, new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", mps.prod_code), new OracleParameter("p_por_no", mps.por_no), new OracleParameter("p_plan_no", mps.ref_no)).FirstOrDefault();

                        if (chkqty > vqty)
                        {
                           
                            //Update QTY_PDT in PD_DET
                            OracleCommand oraCommandpd = conn.CreateCommand();
                            OracleParameter[] parampd = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_plan_no", mps.ref_no)
                            };

                            oraCommandpd.BindByName = true;
                            oraCommandpd.Parameters.AddRange(parampd);
                            oraCommandpd.CommandText = "update pd_det set qty_pdt = qty_pdt - :p_qty where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code and por_no = :p_por_no and plan_no = :p_plan_no";

                            oraCommandpd.ExecuteNonQuery();
                        }
                        else if (chkqty == vqty)
                        {
                            //Insert into PD_DET
                            OracleCommand oraCommandpd = conn.CreateCommand();
                            OracleParameter[] parampd = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_plan_no", mps.ref_no)
                            };

                            oraCommandpd.BindByName = true;
                            oraCommandpd.Parameters.AddRange(parampd);
                            oraCommandpd.CommandText = "delete pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code and por_no = :p_por_no and plan_no = :p_plan_no";

                            oraCommandpd.ExecuteNonQuery();
                        }


                        //Update POR_DET - PO
                        OracleCommand oraCommandpo = conn.CreateCommand();
                        OracleParameter[] parampo = new OracleParameter[]
                        {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                        };

                        oraCommandpo.BindByName = true;
                        oraCommandpo.Parameters.AddRange(parampo);
                        oraCommandpo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) - nvl(:p_qty,0)  where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                        oraCommandpo.ExecuteNonQuery();


                        //Update POR_DET - MO
                        if (mps.ord_type == "MO")
                        {
                            OracleCommand oraCommandmo = conn.CreateCommand();
                            OracleParameter[] parammo = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.ref_no),
                            };

                            oraCommandmo.BindByName = true;
                            oraCommandmo.Parameters.AddRange(parammo);
                            oraCommandmo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) - nvl(:p_qty,0) , fin_date = sysdate where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                            oraCommandmo.ExecuteNonQuery();
                        }

                        //Update MPS_DET_WC
                        string sql7 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                        List<SetDataView> pcs = ctx.Database.SqlQuery<SetDataView>(sql7, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).ToList();

                        foreach (var i in pcs)
                        {
                            OracleCommand oraCommandmps = conn.CreateCommand();
                            OracleParameter[] parammps = new OracleParameter[]
                            {
                                        //new OracleParameter("p_doc_no", vdoc_no),
                                        //new OracleParameter("p_gen_by", vuser_id),
                                        new OracleParameter("p_entity", mps.po_entity),
                                        new OracleParameter("p_prod_code", mps.prod_code),
                                        new OracleParameter("p_por_no", mps.por_no),
                                        new OracleParameter("p_wc_code", mps.wc_code),
                                        new OracleParameter("p_fin_date", vfin_date),
                                        new OracleParameter("p_pcs_barcode", i.tran_no),
                            };

                            oraCommandmps.BindByName = true;
                            oraCommandmps.Parameters.AddRange(parammps);
                            oraCommandmps.CommandText = "update mps_det_wc set doc_status = null , doc_no = null , gen_by = null , gen_date = null where entity = :p_entity and prod_code = :p_prod_code and por_no = :p_por_no and wc_code = :p_wc_code and pcs_barcode = :p_pcs_barcode";

                            oraCommandmps.ExecuteNonQuery();
                        }


                        //Update PKG_BARCODE
                        OracleCommand oraCommandbar = conn.CreateCommand();
                        OracleParameter[] parambar = new OracleParameter[]
                        {
                                //new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_set_no", vset_no),
                                new OracleParameter("p_fin_date", vfin_date),
                        };

                        oraCommandbar.BindByName = true;
                        oraCommandbar.Parameters.AddRange(parambar);
                        oraCommandbar.CommandText = "update pkg_barcode set ref_pd_docno = null , build_type = null where entity = :p_entity and pkg_barcode_set = :p_set_no and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy')";

                        oraCommandbar.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }

                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = mps.prod_code,
                        prod_name = mps.prod_name,
                        qty = vqty
                        //fin_date = set_no.fin_date
                    };


                    //return data to contoller
                    return view;




                }
                else   // HMSTK
                {
                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = "",
                        prod_name = "",
                        qty = 0,
                        
                    };


                    //return data to contoller
                    return view;
                }

            }
        }

        public ScanApproveFinView ScanApvSendNew(ScanApproveAddView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vdoc_no = model.doc_no;
                var vset_no = model.set_no;
                var vbuild_type = model.build_type;
                var vfin_date = model.fin_date;
                var vuser_id = model.user_id;
                var vdoc_code = "";
                string doc_no = "";
                string dateFormat = "yyMM";
                DateTime dateNow = DateTime.Now;



                if (vbuild_type == "HMJIT")
                {
                    string sql1 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no and ref_pd_docno is null";
                    SetDataView set_no = ctx.Database.SqlQuery<SetDataView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                    if(set_no == null)
                    {
                        throw new Exception("Set No. ไม่ถูกต้อง / Set No. มีการส่งมอบแล้ว");
                    }

                    string sql2 = "select entity , req_date , wc_code , pcs_barcode , prod_code , prod_name , por_no , pdjit_grp , wh_code , po_entity , ref_no , doc_code , ord_type , bar_code from mps_det_wc where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code = :p_wc_code";
                    MpsWcDataView mps = ctx.Database.SqlQuery<MpsWcDataView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_pcs_barcode", set_no.tran_no), new OracleParameter("p_wc_code", set_no.wc_code)).FirstOrDefault();

                    string sql4 = "select count(*) from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                    int vqty = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                    
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 15, 0)))
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());

                        conn.Open();

                        if (vdoc_no == "")
                        {
                            if (mps.po_entity == "H10")
                            {
                                vdoc_code = "PH";
                                string preFix = vdoc_code + string.Format("{0:" + dateFormat + "}", dateNow);

                                string sql3 = "select :p_doc_code||to_char(sysdate,'yymm')||ltrim(to_char(nvl(max(to_number(substr(doc_no,7,4))),0)+1,'0999')) from pd_mast where pd_entity = :p_entity and doc_no like :p_prefix";
                                doc_no = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_doc_code", vdoc_code), new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_prefix", preFix + "%")).FirstOrDefault();
                            }
                            else
                            {
                                vdoc_code = "PP";
                                string preFix = vdoc_code + string.Format("{0:" + dateFormat + "}", dateNow);

                                string sql3 = "select :p_doc_code||to_char(sysdate,'yymm')||ltrim(to_char(nvl(max(to_number(substr(doc_no,7,4))),0)+1,'0999')) from pd_mast where pd_entity = :p_entity and doc_no like :p_prefix";
                                doc_no = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_doc_code", vdoc_code), new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_prefix", preFix + "%")).FirstOrDefault();
                            }

                            vdoc_no = doc_no;

                            //Insert into PD_MAST
                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_wc_code", mps.wc_code),
                                new OracleParameter("p_user_code", vuser_id),
                                new OracleParameter("p_gen_by", vuser_id),
                                new OracleParameter("p_wh_code", mps.wh_code),
                                new OracleParameter("p_doc_code", mps.doc_code)
                            };

                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "insert into pd_mast (pd_entity , doc_no , doc_date , wc_code , doc_status , sys_date , user_code , gen_by , gen_date , wh_code , doc_code) values (:p_entity , :p_doc_no , sysdate , :p_wc_code , 'PAL' , sysdate , :p_user_code , :p_gen_by , sysdate , :p_wh_code , :p_doc_code)";


                            oraCommand.ExecuteNonQuery();

                            OracleCommand oraCommanddet = conn.CreateCommand();
                            OracleParameter[] paramdet = new OracleParameter[]
                            {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_bar_code", mps.bar_code),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_wh_code", mps.wh_code),
                                new OracleParameter("p_ref_no", mps.ref_no)
                            };

                            oraCommanddet.BindByName = true;
                            oraCommanddet.Parameters.AddRange(paramdet);
                            oraCommanddet.CommandText = "insert into pd_det (pd_entity , doc_no , line_no , bar_code , prod_code , qty_pdt , pd_time , pd_manpower , def_status , por_no , wh_code , plan_no) values (:p_entity , :p_doc_no , 1 , :p_bar_code , :p_prod_code , :p_qty , 0 , 0 , 'N' , :p_por_no , :p_wh_code , :p_ref_no)";

                            oraCommanddet.ExecuteNonQuery();

                        }
                        else
                        {
                            string sql6 = "select prod_code from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code and por_no = :p_por_no";
                            string chkdup = ctx.Database.SqlQuery<string>(sql6, new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", mps.prod_code), new OracleParameter("p_por_no)", mps.por_no)).FirstOrDefault();

                            if(chkdup == null)
                            {
                                string sql5 = "select count(*)+1 from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no";
                                int vline_no = ctx.Database.SqlQuery<int>(sql5, new OracleParameter("p_entity", mps.po_entity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();



                                //Insert into PD_DET
                                OracleCommand oraCommandpd = conn.CreateCommand();
                                OracleParameter[] parampd = new OracleParameter[]
                                {
                                new OracleParameter("p_entity",  mps.po_entity),
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_line_no", vline_no),
                                new OracleParameter("p_bar_code", mps.bar_code),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                                new OracleParameter("p_wh_code", mps.wh_code),
                                new OracleParameter("p_ref_no", mps.ref_no)
                                };

                                oraCommandpd.BindByName = true;
                                oraCommandpd.Parameters.AddRange(parampd);
                                oraCommandpd.CommandText = "insert into pd_det (pd_entity , doc_no , line_no , bar_code , prod_code , qty_pdt , pd_time , pd_manpower , def_status , por_no , wh_code , plan_no) values (:p_entity , :p_doc_no , :p_line_no , :p_bar_code , :p_prod_code , :p_qty , 0 , 0 , 'N' , :p_por_no , :p_wh_code , :p_ref_no)";

                                oraCommandpd.ExecuteNonQuery();
                            }
                            else
                            {
                                //Insert into PD_DET
                                OracleCommand oraCommandpd = conn.CreateCommand();
                                OracleParameter[] parampd = new OracleParameter[]
                                {
                                    new OracleParameter("p_entity",  mps.po_entity),
                                    new OracleParameter("p_doc_no", vdoc_no),
                                    new OracleParameter("p_prod_code", mps.prod_code),
                                    new OracleParameter("p_qty", vqty),
                                    new OracleParameter("p_por_no", mps.por_no),
                                    new OracleParameter("p_plan_no", mps.ref_no),
                                };

                                oraCommandpd.BindByName = true;
                                oraCommandpd.Parameters.AddRange(parampd);
                                oraCommandpd.CommandText = "update pd_det set qty_pdt = qty_pdt + :p_qty where pd_entity = :p_entity and doc_no = :p_doc_no and por_no = :p_por_no and plan_no = :p_plan_no";

                                oraCommandpd.ExecuteNonQuery();
                            }
                        }

                        


                        //Update POR_DET - PO
                        OracleCommand oraCommandpo = conn.CreateCommand();
                        OracleParameter[] parampo = new OracleParameter[]
                        {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.por_no),
                        };

                        oraCommandpo.BindByName = true;
                        oraCommandpo.Parameters.AddRange(parampo);
                        oraCommandpo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) + nvl(:p_qty,0) , fin_date = sysdate where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                        oraCommandpo.ExecuteNonQuery();


                        //Update POR_DET - MO
                        if(mps.ord_type == "MO")
                        {
                            OracleCommand oraCommandmo = conn.CreateCommand();
                            OracleParameter[] parammo = new OracleParameter[]
                            {
                                new OracleParameter("p_entity", mps.po_entity),
                                new OracleParameter("p_prod_code", mps.prod_code),
                                new OracleParameter("p_qty", vqty),
                                new OracleParameter("p_por_no", mps.ref_no),
                            };

                            oraCommandmo.BindByName = true;
                            oraCommandmo.Parameters.AddRange(parammo);
                            oraCommandmo.CommandText = "update por_det set qty_pdt = nvl(qty_pdt,0) + nvl(:p_qty,0) , fin_date = sysdate where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code";

                            oraCommandmo.ExecuteNonQuery();
                        }

                        //Update MPS_DET_WC
                        string sql7 = "select entity ,tran_no , tran_date , wc_code  , prod_code , bar_code , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode where entity = :p_entity and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no";
                        List<SetDataView> pcs = ctx.Database.SqlQuery<SetDataView>(sql7, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_set_no", vset_no)).ToList();

                        foreach (var i in pcs)
                        {
                            OracleCommand oraCommandmps = conn.CreateCommand();
                            OracleParameter[] parammps = new OracleParameter[]
                            {
                                        new OracleParameter("p_doc_no", vdoc_no),
                                        new OracleParameter("p_gen_by", vuser_id),
                                        new OracleParameter("p_entity", mps.po_entity),
                                        new OracleParameter("p_prod_code", mps.prod_code),
                                        new OracleParameter("p_por_no", mps.por_no),
                                        new OracleParameter("p_wc_code", mps.wc_code),
                                        new OracleParameter("p_fin_date", vfin_date),
                                        new OracleParameter("p_pcs_barcode", i.tran_no),
                            };

                            oraCommandmps.BindByName = true;
                            oraCommandmps.Parameters.AddRange(parammps);
                            oraCommandmps.CommandText = "update mps_det_wc set doc_status = 'P' , doc_no = :p_doc_no , gen_by = :p_gen_by , gen_date = sysdate where entity = :p_entity and prod_code = :p_prod_code and por_no = :p_por_no and wc_code = :p_wc_code and gen_date is null and pcs_barcode = :p_pcs_barcode";

                            oraCommandmps.ExecuteNonQuery();
                        }

                        //Update PKG_BARCODE
                        OracleCommand oraCommandbar = conn.CreateCommand();
                        OracleParameter[] parambar = new OracleParameter[]
                        {
                                new OracleParameter("p_doc_no", vdoc_no),
                                new OracleParameter("p_entity", ventity),
                                new OracleParameter("p_set_no", vset_no),
                                new OracleParameter("p_fin_date", vfin_date),
                        };

                        oraCommandbar.BindByName = true;
                        oraCommandbar.Parameters.AddRange(parambar);
                        oraCommandbar.CommandText = "update pkg_barcode set ref_pd_docno = :p_doc_no , build_type = 'HMJIT' where entity = :p_entity and pkg_barcode_set = :p_set_no and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy')";

                        oraCommandbar.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }
                    

                    
                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = mps.prod_code,
                        prod_name = mps.prod_name,
                        qty = vqty,
                        fin_date = set_no.fin_date
                    };


                    //return data to contoller
                    return view;

                }
                else   // HMSTK
                {
                    ScanApproveFinView view = new ModelViews.ScanApproveFinView()
                    {
                        doc_no = vdoc_no,
                        set_no = vset_no,
                        prod_code = "",
                        prod_name = "",
                        qty = 0,
                        fin_date = ""
                    };


                    //return data to contoller
                    return view;
                }

            }
            
        }

        public ScanApproveSendView SearchDataScanSend(ScanApproveSendSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vbuild_type = model.build_type;
                string vuser = model.user_id;
                string vdoc_no = model.doc_no;
                string vfin_date = model.fin_date;
                string vsend_type = model.send_type;
                int total_qty = 0;
                int total_set = 0;



                //define model view
                ScanApproveSendView view = new ModelViews.ScanApproveSendView()
                {
                    entity = ventity,
                    build_type = vbuild_type,
                    user_id = vuser,
                    datas = new List<ModelViews.ScanApproveSendDataView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {
                    if(vsend_type == "WAIT")
                    {
                        string sql = "select entity , req_date , pdjit_grp , wc_code , doc_no , sum(qty_pdt) tot_qty , " +
                            "(select count(distinct a.pkg_barcode_set) " +
                            "from pkg_barcode a " +
                            "where a.entity = :p_entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code " +
                            "and a.pkg_barcode_set is not null ) set_qty " +
                        "from mps_det_wc " +
                        "where entity = :p_entity2 " +
                        "and mps_st = 'Y' " +
                        "and (build_type in ('HMJIT','') or build_type is null) " +
                        "and doc_no is null " +
                        "and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') " +
                        "and wc_code in (select dept_code from auth_function where function_id = 'PDOPTHM' and doc_code = 'Y' and user_id = :p_user_id) " +
                        "group by entity , req_date , pdjit_grp , wc_code , doc_no";

                        List<ScanApproveSendDataView> send = ctx.Database.SqlQuery<ScanApproveSendDataView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_entity2", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_user_id", vuser)).ToList();

                        foreach (var x in send)
                        {
                            view.datas.Add(new ModelViews.ScanApproveSendDataView()
                            {
                                req_date = x.req_date,
                                pdjit_grp = x.pdjit_grp,
                                wc_code = x.wc_code,
                                doc_no = x.doc_no,
                                set_qty = x.set_qty,
                                tot_qty = x.tot_qty,
                                status = "รอส่งมอบ",
                                doc_status = "",

                            });
                            total_qty = total_qty + x.tot_qty;
                            total_set = total_set + x.set_qty;
                        }

                        view.total_qty = total_qty;
                        view.total_set = total_set;

                    }
                    else
                    {
                        string sql = "select entity , req_date , pdjit_grp , wc_code , doc_no , " +
                            "(select sum(1) from pkg_barcode " +
                            "where a.ref_pd_docno = doc_no" +
                            "and a.entity = entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code) tot_qty " +
                            "(select count(distinct a.pkg_barcode_set) " +
                            "from pkg_barcode a " +
                            "where a.ref_pd_docno = doc_no " +
                            "and a.entity = entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code) tot_set " +
                        "from mps_det_wc " +
                        "where entity = :p_entity " +
                        "and mps_st = 'Y' " +
                        "and (build_type in ('HMJIT','') or build_type is null) " +
                        "and doc_no like :p_doc_no " +
                        "and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') " +
                        "and wc_code in (select dept_code from auth_function where function_id = 'PDOPTHM' and doc_code = 'Y' and user_id = :p_user_id) " + 
                        "and (entity , req_date , wc_code , prod_code , doc_no) in " +
                            "(select b.entity , b.tran_date , b.wc_code , b.prod_code , b.doc_no " +
                            "from pkg_barcode b " +
                            "where b.wc_code = wc_code " +
                            "and b.tran_date = req_date " +
                            "and b.prod_code = prod_code " +
                            "and (ref_pd_docno is not null or pd_ref_docno <> '')) " +
                        "group by entity , req_date , pdjit_grp , wc_code , doc_no";

                        List<ScanApproveSendDataView> send = ctx.Database.SqlQuery<ScanApproveSendDataView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_user_id", vuser), new OracleParameter("p_doc_no", "%" + vdoc_no + "%") ).ToList();

                        foreach (var x in send)
                        {
                            string sqls = "select doc_status from pd_mast where doc_no = :p_doc_no";
                            string status = ctx.Database.SqlQuery<string>(sqls, new OracleParameter("p_doc_no", x.doc_no)).FirstOrDefault();

                            view.datas.Add(new ModelViews.ScanApproveSendDataView()
                            {
                                req_date = x.req_date,
                                pdjit_grp = x.pdjit_grp,
                                wc_code = x.wc_code,
                                doc_no = x.doc_no,
                                set_qty = x.set_qty,
                                tot_qty = x.tot_qty,
                                status = "ส่งมอบแล้ว",
                                doc_status = status

                            });
                            total_qty = total_qty + x.tot_qty;
                            total_set = total_set + x.set_qty;
                        }

                        view.total_qty = total_qty;
                        view.total_set = total_set;
                    }

                }
                else if (model.build_type == "HMSTK")
                {

                }

                //return data to contoller
                return view;
            }
        }
    }
}