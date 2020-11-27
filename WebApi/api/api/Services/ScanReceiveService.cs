using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Transactions;
using System.Web;

namespace api.Services
{
    public class ScanReceiveService : IScanReceiveService
    {
        public SendDataView getProductDetail(string entity, string doc_no)
        {
            using (var ctx = new ConXContext())
            {
                int vtotal_qty = 0;

                //define model view
                SendDataView view = new ModelViews.SendDataView()
                {
                    doc_no = doc_no,
                    total_item = 0,
                    total_qty = 0,
                    datas = new List<ModelViews.SendDataDetailView>()
                };

                
                string sql1 = "select a.line_no , a.prod_code , a.qty_pdt , a.por_no job_no , a.plan_no ref_no , b.prod_tname prod_name , b.uom_code  from pd_det a , product b  where a.prod_code = b.prod_code and  a.pd_entity=:p_entity and a.doc_no=:p_doc_no";
                List<SendDataDetailView> send = ctx.Database.SqlQuery<SendDataDetailView>(sql1, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", doc_no)).ToList();

                
                view.total_item = send.Count;

                string vset_no = "";

                foreach (var x in send)
                {
                    // Find Set No.
                    string sql2 = "select to_char(pkg_barcode_set) set_no , count(*) qty from pkg_barcode where entity = :p_entity and por_no = :p_por_no and ref_pd_docno = :p_doc_no and prod_code = :p_prod_code group by pkg_barcode_set order by pkg_barcode_set";
                    List<SendSetNoView> set = ctx.Database.SqlQuery<SendSetNoView>(sql2, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", x.job_no), new OracleParameter("p_doc_no", doc_no), new OracleParameter("p_prod_code", x.prod_code)).ToList();

                    foreach(var i in set)
                    {
                        vset_no = vset_no + i.set_no + "(" + i.qty + ") ,";
                    }

                    string sql = "select distinct to_char(b.req_date,'dd/mm/yyyy') from pd_det a , mps_det b where a.pd_entity = :p_entity and a.doc_no = :p_doc_no and a.plan_no = b.por_no  and a.por_no = b.ref_no and rownum=1";
                    //string sql = "select to_char(req_date,'dd/mm/yyyy') from mps_det where entity = :p_entity and por_no= :p_por_no and ref_no=:p_ref_no and rownum = 1";
                    string vreq_date = ctx.Database.SqlQuery<string>(sql, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", doc_no)).FirstOrDefault();


                    view.datas.Add(new ModelViews.SendDataDetailView()
                    {

                        line_no = x.line_no,
                        prod_code = x.prod_code,
                        prod_name = x.prod_name,
                        uom_code = x.uom_code,
                        qty_pdt = x.qty_pdt,
                        req_date = vreq_date,
                        job_no = x.job_no,
                        set_no = vset_no
                    });

                    vtotal_qty = vtotal_qty + x.qty_pdt;
                   
                }
                view.total_qty = vtotal_qty;
                view.set_no = vset_no;

                return view;

            }
        }

        public ScanReceiveView ScanReceiveAdd(ScanReceiveSearchView model)
        {
            string ventity = model.entity;
            string vdoc_no = model.doc_no;
            string vscan_type = model.scan_type;
            string vscan_data = model.scan_data;
            string vuser_id = model.user_id;
            int vscan_qty = model.scan_qty;
            string vbuild_type = model.build_type;

            string vset_no = ""; 


            using (var ctx = new ConXContext())
            {
                if (vbuild_type == "HMJIT")
                {
                    if (vscan_type == "SETNO")
                    {
                        String[] strlist = model.scan_data.Split('|');
                        vset_no = strlist[2];

                        string sql1 = "select max(prod_code) prod_code , max(bar_code) bar_code , to_char(max(pkg_barcode_set)) set_no , count(*) scan_qty from pkg_barcode where entity = :p_entity and ref_pd_docno = :p_doc_no and pkg_barcode_set = :p_set_no";
                        ScanDataView scan = ctx.Database.SqlQuery<ScanDataView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_set_no", vset_no)).FirstOrDefault();

                        if (scan.prod_code == null)
                        {
                            throw new Exception("Set No. ไม่ถูกต้อง");
                        }

                        string sql2 = "select sum(qty_pdt) from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_pdt = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();


                        string sql4 = "select nvl(sum(qty_pdt_cf),0) from pd_det_whcf where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_rec = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();


                        if (scan.scan_qty > (qty_pdt - qty_rec))
                        {
                            throw new Exception("จำนวนรับ มากกว่า จำนวนส่งมอบ");
                        }

                        string sql3 = "select nvl(max(line_no),0)+1 from pd_det_whcf where pd_entity= :p_entity and doc_no=:p_doc_no";
                        int vline_no = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();

                        // Insert into PD_DET_WHCF
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
                                    new OracleParameter("p_doc_no", vdoc_no),
                                    new OracleParameter("p_line_no", vline_no),
                                    new OracleParameter("p_prod_code", scan.prod_code),
                                    new OracleParameter("p_bar_code", scan.bar_code),
                                    new OracleParameter("p_set_no", vset_no),
                                    new OracleParameter("p_qty", scan.scan_qty),
                                    new OracleParameter("p_cf_by", vuser_id),
                                    new OracleParameter("p_upd_by", vuser_id)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "insert into pd_det_whcf (pd_entity , doc_no , line_no , bar_code, prod_code , ref_set_no , qty_pdt_cf , cf_by , cf_date , upd_by , upd_date) values (:p_entity , :p_doc_no , :p_line_no , :p_bar_code, :p_prod_code , :p_set_no , :p_qty , :p_cf_by , sysdate , :p_upd_by , sysdate)";


                            oraCommand.ExecuteNonQuery();

                            conn.Close();


                            scope.Complete();


                        }

                        ScanReceiveView view = new ModelViews.ScanReceiveView()
                        {
                            entity = ventity,
                            doc_no = vdoc_no,
                            set_no = vset_no,
                            prod_code = scan.prod_code,
                            qty = scan.scan_qty,
                            line_no = vline_no

                        };

                        //return data to contoller
                        return view;


                    }
                    else if (vscan_type == "PCS")
                    {

                        string sql1 = "select prod_code , bar_code from mps_det_wc where entity = :p_entity and doc_no = :p_doc_no and pcs_barcode = :p_pcs_barcode";
                        ScanDataView scan = ctx.Database.SqlQuery<ScanDataView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_pcs_barcode", vscan_data)).FirstOrDefault();

                        if (scan == null)
                        {
                            throw new Exception("PCS Barcode ไม่ถูกต้อง");
                        }

                        string sql2 = "select sum(qty_pdt) from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_pdt = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();

                        string sql4 = "select nvl(sum(qty_pdt_cf),0) from pd_det_whcf where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_rec = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();


                        if (vscan_qty > (qty_pdt - qty_rec))
                        {
                            throw new Exception("จำนวนรับ มากกว่า จำนวนส่งมอบ");
                        }

                        string sql3 = "select nvl(max(line_no),0)+1 from pd_det_whcf where pd_entity= :p_entity and doc_no=:p_doc_no";
                        int vline_no = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();

                        // Insert into PD_DET_WHCF
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
                                    new OracleParameter("p_doc_no", vdoc_no),
                                    new OracleParameter("p_line_no", vline_no),
                                    new OracleParameter("p_prod_code", scan.prod_code),
                                    new OracleParameter("p_bar_code", scan.bar_code),
                                    new OracleParameter("p_qty", vscan_qty),
                                    new OracleParameter("p_cf_by", vuser_id),
                                    new OracleParameter("p_upd_by", vuser_id)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "insert into pd_det_whcf (pd_entity , doc_no , line_no , bar_code, prod_code , ref_set_no , qty_pdt_cf , cf_by , cf_date , upd_by , upd_date) values (:p_entity , :p_doc_no , :p_line_no , :p_bar_code, :p_prod_code , '' , :p_qty , :p_cf_by , sysdate , :p_upd_by , sysdate)";


                            oraCommand.ExecuteNonQuery();

                            conn.Close();


                            scope.Complete();


                        }

                        ScanReceiveView view = new ModelViews.ScanReceiveView()
                        {
                            entity = ventity,
                            doc_no = vdoc_no,
                            set_no = "",
                            prod_code = scan.prod_code,
                            qty = vscan_qty,
                            line_no = vline_no


                        };

                        //return data to contoller
                        return view;
                    }
                    else
                    {
                        string sql1 = "select prod_code , bar_code from product where bar_code = :p_barcode";
                        ScanDataView scan = ctx.Database.SqlQuery<ScanDataView>(sql1, new OracleParameter("p_bar_code", vscan_data)).FirstOrDefault();

                        if (scan == null)
                        {
                            throw new Exception("Barcode ไม่ถูกต้อง");
                        }

                        string sql2 = "select sum(qty_pdt) from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_pdt = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();

                        string sql4 = "select nvl(sum(qty_pdt_cf),0) from pd_det_whcf where pd_entity = :p_entity and doc_no = :p_doc_no and prod_code = :p_prod_code";
                        int qty_rec = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no), new OracleParameter("p_prod_code", scan.prod_code)).FirstOrDefault();


                        if (vscan_qty > (qty_pdt - qty_rec))
                        {
                            throw new Exception("จำนวนรับ มากกว่า จำนวนส่งมอบ");
                        }

                        string sql3 = "select nvl(max(line_no),0)+1 from pd_det_whcf where pd_entity= :p_entity and doc_no=:p_doc_no";
                        int vline_no = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();

                        // Insert into PD_DET_WHCF
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
                                    new OracleParameter("p_doc_no", vdoc_no),
                                    new OracleParameter("p_line_no", vline_no),
                                    new OracleParameter("p_prod_code", scan.prod_code),
                                    new OracleParameter("p_bar_code", scan.bar_code),
                                    new OracleParameter("p_qty", vscan_qty),
                                    new OracleParameter("p_cf_by", vuser_id),
                                    new OracleParameter("p_upd_by", vuser_id)
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "insert into pd_det_whcf (pd_entity , doc_no , line_no , bar_code, prod_code , ref_set_no , qty_pdt_cf , cf_by , cf_date , upd_by , upd_date) values (:p_entity , :p_doc_no , :p_line_no , :p_bar_code, :p_prod_code , '' , :p_qty , :p_cf_by , sysdate , :p_upd_by , sysdate)";


                            oraCommand.ExecuteNonQuery();

                            conn.Close();


                            scope.Complete();


                        }


                        string sql5 = "select sum(qty_pdt) from pd_det where pd_entity = :p_entity and doc_no = :p_doc_no";
                        int total_qty_pdt = ctx.Database.SqlQuery<int>(sql5, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();

                        string sql6 = "select sum(qty_pdt_cf) from pd_det_whcf where pd_entity = :p_entity and doc_no = :p_doc_no";
                        int total_qty_rec = ctx.Database.SqlQuery<int>(sql6, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", vdoc_no)).FirstOrDefault();


                        // Send Mail
                        if(total_qty_pdt == total_qty_rec)
                        {
                            SendMail(ventity , vdoc_no);
                        }

                        ScanReceiveView view = new ModelViews.ScanReceiveView()
                        {
                            entity = ventity,
                            doc_no = vdoc_no,
                            set_no = "",
                            prod_code = scan.prod_code,
                            qty = vscan_qty,
                            line_no = vline_no

                        };

                        //return data to contoller
                        return view;
                    }
                }
                else
                {
                    ScanReceiveView view = new ModelViews.ScanReceiveView()
                    {
                        entity = ventity,
                        doc_no = vdoc_no,
                        set_no = "",
                        //prod_code = scan.prod_code,
                        //qty = vscan_qty,
                        //line_no = vline_no

                    };

                    //return data to contoller
                    return view;
                }

            }
        }

        public void ScanReceiveCancel(ScanReceiveView scan)
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
                        new OracleParameter("p_doc_no", scan.doc_no),
                        new OracleParameter("p_line_no", scan.line_no),
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "delete pd_det_whcf  where pd_entity = :p_entity and prod_code = :p_prod_code and doc_no =:p_doc_no and line_no = :p_line_no";
                    oraCommand.ExecuteNonQuery();


                    conn.Close();

                    scope.Complete();
                }
            }
        }

        public ScanReceiveDataView SearchDataScanReceive(ScanReceiveDataSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vbuild_type = model.build_type;               
                string vdoc_no = model.doc_no.Trim(' '); ;
                string vdoc_date = model.doc_date;
                string vsend_type = model.send_type;
                string vuser_id = model.user_id;
                int total_qty_pdt = 0;
                int total_qty_rec = 0;



                //define model view
                ScanReceiveDataView view = new ModelViews.ScanReceiveDataView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    entity = ventity,
                    build_type = vbuild_type,
                    total_qty_pdt = 0,
                    total_qty_rec = 0,
                    datas = new List<ModelViews.ScanReceiveDataDetailView>()
                };

                //query data
                //if (model.build_type == "HMJIT")
                //{
                    if (vsend_type == "WAIT")
                    {
                        string sql1 = "select a.doc_no , max(a.wc_code) wc_code , max(a.gen_by) gen_by , to_char(max(a.gen_date),'dd/mm/yyyy hh24:mi') gen_date , max(plan_no) plan_no , sum(b.qty_pdt) qty_pdt " +
                            "from pd_mast a , pd_det b " +
                            "where a.pd_entity = b.pd_entity " +
                            "and a.doc_no = b.doc_no " +
                            "and a.pd_entity = :p_entity " +
                            "and trunc(doc_date) = to_date(:p_doc_date,'dd/mm/yyyy') " +
                            "and a.doc_no like :p_doc_no " +
                            "and a.doc_status = 'PAL' " +
                            "group by  a.doc_no order by a.doc_no";

                        List<ScanReceiveDataDetailView> send = ctx.Database.SqlQuery<ScanReceiveDataDetailView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_date", vdoc_date), new OracleParameter("p_doc_no", vdoc_no+"%")).ToList();

                        view.totalItem = send.Count;
                        send = send.Skip(view.pageIndex * view.itemPerPage)
                            .Take(view.itemPerPage)
                            .ToList();

                        foreach (var x in send)
                        {

                            //string sql2 = "select to_char(max(req_date),'dd/mm/yyyy') from mps_det_wc where doc_no = :p_doc_no";
                            string sql2 = "select to_char(req_date,'dd/mm/yyyy') from por_mast where por_no = :p_por_no";
                            string vreq_date = ctx.Database.SqlQuery<string>(sql2, new OracleParameter("p_por_no", x.plan_no)).FirstOrDefault();

                            if(vreq_date == null)
                            {
                                vreq_date = "";
                            }

                            string sql3 = "select nvl(sum(qty_pdt_cf),0) from pd_det_whcf where doc_no = :p_doc_no";
                            int vqty_rec = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_doc_no", x.doc_no)).FirstOrDefault();

                           

                            view.datas.Add(new ModelViews.ScanReceiveDataDetailView()
                            {
                                entity = ventity,
                                req_date =  vreq_date,
                                doc_no = x.doc_no,
                                wc_code = x.wc_code,
                                gen_by = x.gen_by,
                                gen_date = x.gen_date,
                                qty_pdt = x.qty_pdt,
                                qty_rec = vqty_rec,
                                doc_status = "PAL",
                                user_id = vuser_id

                            });
                            total_qty_pdt = total_qty_pdt + x.qty_pdt;
                            total_qty_rec = total_qty_rec + vqty_rec;
                        }

                        view.total_qty_pdt = total_qty_pdt;
                        view.total_qty_rec = total_qty_rec;

                    }
                    else
                    {
                        string sql1 = "select a.doc_no , max(a.wc_code) wc_code , max(a.gen_by) gen_by , to_char(max(a.gen_date),'dd/mm/yyyy hh24:mi') gen_date , max(plan_no) plan_no , sum(b.qty_pdt) qty_pdt " +
                            "from pd_mast a , pd_det b " +
                            "where a.pd_entity = b.pd_entity " +
                            "and a.doc_no = b.doc_no " +
                            "and a.pd_entity = :p_entity " +
                            "and trunc(doc_date) = to_date(:p_doc_date,'dd/mm/yyyy') " +
                            "and a.doc_no like :p_doc_no " +
                            "and a.doc_status = 'APV' " +
                            "group by  a.doc_no order by a.doc_no";

                        List<ScanReceiveDataDetailView> send = ctx.Database.SqlQuery<ScanReceiveDataDetailView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_date", vdoc_date), new OracleParameter("p_doc_no", vdoc_no + "%")).ToList();

                        view.totalItem = send.Count;
                        send = send.Skip(view.pageIndex * view.itemPerPage)
                            .Take(view.itemPerPage)
                            .ToList();

                        foreach (var x in send)
                        {

                            //string sql2 = "select to_char(max(req_date),'dd/mm/yyyy') from mps_det_wc where doc_no = :p_doc_no";
                            //string vreq_date = ctx.Database.SqlQuery<string>(sql2, new OracleParameter("p_doc_no", x.doc_no)).FirstOrDefault();
                            string sql2 = "select to_char(req_date,'dd/mm/yyyy') from por_mast where por_no = :p_por_no";
                            string vreq_date = ctx.Database.SqlQuery<string>(sql2, new OracleParameter("p_por_no", x.plan_no)).FirstOrDefault();

                            string sql3 = "select nvl(sum(qty_pdt_cf),0) from pd_det_whcf where doc_no = :p_doc_no";
                            int vqty_rec = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_doc_no", x.doc_no)).FirstOrDefault();



                            view.datas.Add(new ModelViews.ScanReceiveDataDetailView()
                            {
                                entity = ventity,
                                req_date = vreq_date,
                                doc_no = x.doc_no,
                                wc_code = x.wc_code,
                                gen_by = x.gen_by,
                                gen_date = x.gen_date,
                                qty_pdt = x.qty_pdt,
                                qty_rec = vqty_rec,
                                doc_status = "APV",
                                user_id = vuser_id

                            });
                            total_qty_pdt = total_qty_pdt + x.qty_pdt;
                            total_qty_rec = total_qty_rec + vqty_rec;
                        }

                        view.total_qty_pdt = total_qty_pdt;
                        view.total_qty_rec = total_qty_rec;
                    }

                //}
                //else if (model.build_type == "HMSTK")
                //{

                //}

                //return data to contoller
                return view;
            }
        }

        public void SendMail(string entity, string doc_no)
        {
            using (var ctx = new ConXContext())
            {

                string sql1 = "select to_char(min(cf_date),'dd/mm/yyyy hh24:mi') start_date , to_char(max(cf_date),'dd/mm/yyyy hh24:mi') end_date from pd_det_whcf where pd_entity = :p_entity and doc_no = :p_doc_no";
                ScanDataDateView vdate = ctx.Database.SqlQuery<ScanDataDateView>(sql1, new OracleParameter("p_entity", entity),new OracleParameter("p_doc_no", doc_no)).FirstOrDefault();

                string sql2 = "select email_address from auth_function where function_id='WHCONFBUIDREC' and rownum = 1";
                string vemail = ctx.Database.SqlQuery<string>(sql2).FirstOrDefault();

                var fromAddress = new MailAddress("itjob@jaspalhome.com", "Production");
                var toAddress = new MailAddress(vemail, "Admin");
                string url = ConfigurationManager.AppSettings["urlDetail"];


                string subject = "ยืนยันรับมอบสินค้า - ผลผลิตที่นอน (JIT : H10) เลขที่ : " + doc_no;
                string body = "<html><body>Scan รับมอบเลขที่ : " + doc_no + "<br>"
                            + "เริ่ม : " + vdate.start_date + "<br>"
                            + "เสร็จ : " + vdate.end_date + "<br>"
                            + "<a href=" + url + "> Click for Detail </a>"
                            + "</body></html>";

                var smtp = new SmtpClient
                {
                 
                    Host = "mail.jaspalhome.com",
                    Port = 25,
                    //EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("consign", "Consign"),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {

                    Subject = subject,
                    Body = body
                })
                {
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
        }
    }
}