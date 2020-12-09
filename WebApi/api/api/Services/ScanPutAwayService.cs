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
    public class ScanPutAwayService : IScanPutAwayService
    {

        public ScanPutAwaysTotalView ScanPutAwayCancel(ScanPutAwayCancelSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vdoc_no = model.doc_no;
                string vdoc_code = model.doc_code;
                string vitem = model.item;
                string vbar_code = model.bar_code;
                string vprod_code = model.prod_code;


                ScanPutAwaysTotalView view = new ModelViews.ScanPutAwaysTotalView()
                {
                    ptwDetails = new List<ModelViews.ScanPutAwayDetailView>()
                };

                // Validate Data
                if (ventity == "" || ventity == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล Entity");
                }

                if (vdoc_no == "" || vdoc_no == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล เลขที่เอกสารใบส่งมอบ");
                }

                if (vdoc_code == "" || vdoc_code == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล ประเภทเอกสาร");
                }

                if (vitem == "" || vitem == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล item ที่จะลบ");
                }

                if (vprod_code == "" || vprod_code == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล รหัสสินค้า");
                }


                string sqlptw = "select ic_entity, doc_no, doc_code, item, prod_code, bar_code, wh_code, wh_refer, loc_code, loc_refer, qty";
                sqlptw += " from whtran_det";
                sqlptw += " where ic_entity = :pic_entity";
                sqlptw += " and trans_code = 'PTW'";
                sqlptw += " and doc_no = :pdoc_no";
                sqlptw += " and doc_code = :pdoc_code";
                sqlptw += " and item = :pitem";
                sqlptw += " and prod_code = :pprod_code";

                List<ScanPutAwayCancelView> ptwData = ctx.Database.SqlQuery<ScanPutAwayCancelView>(sqlptw, new OracleParameter("pic_entity", ventity),
                                                                                                           new OracleParameter("pdoc_no", vdoc_no),
                                                                                                           new OracleParameter("pdoc_code", vdoc_code),
                                                                                                           new OracleParameter("pitem", int.Parse(vitem)),
                                                                                                           new OracleParameter("pprod_code", vprod_code)).ToList();

                foreach (var i in ptwData)
                {

                    // delete from whtran_det
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                            {
                                 new OracleParameter("pic_entity",ventity),
                                 new OracleParameter("pdoc_no", vdoc_no),
                                 new OracleParameter("pdoc_code", vdoc_code),
                                 new OracleParameter("pitem", i.item),
                                 new OracleParameter("pprod_code", i.prod_code),
                            };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "delete from whtran_det where ic_entity  = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and  doc_code = :pdoc_code and item = :pitem and prod_code = :pprod_code";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }

                    // find total all Ptw Qty
                    string sql_totPtw = "select nvl(sum(nvl(qty,0)),0) qty from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                    int vTotPtw = ctx.Database.SqlQuery<int>(sql_totPtw, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", vdoc_no), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();

                    // Update Or Delete Whtran_Mast
                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (vTotPtw > 0)
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                            new OracleParameter("ptot_qty", vTotPtw),
                            new OracleParameter("psys_date", DateTime.Now),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pdoc_no", vdoc_no),
                            new OracleParameter("pdoc_code", vdoc_code),

                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "update whtran_mast set tot_qty = :ptot_qty, sys_date = :psys_date, doc_status = 'PAL' where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code ";
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();
                        }
                        else
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pdoc_no", vdoc_no),
                            new OracleParameter("pdoc_code", vdoc_code),
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);
                            oraCommand.CommandText = "delete from whtran_mast  where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code ";
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();
                        }

                    }

                    // Update Stock From Location
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                            new OracleParameter("pqty", i.qty),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pprod_code", i.prod_code),
                            new OracleParameter("pwh_code", i.wh_refer),
                            new OracleParameter("ploc_code",i.loc_refer),
                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) + nvl(:pqty,0),";
                        sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) + nvl(:pqty, 0)";
                        sql_upd_loc += "      where ic_entity = :pic_entity";
                        sql_upd_loc += "      and   prod_code = :pprod_code";
                        sql_upd_loc += "      and   wh_code   = :pwh_code";
                        sql_upd_loc += "      and   loc_code  = :ploc_code";

                        oraCommand.CommandText = sql_upd_loc;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }

                    // Update Stock To Location
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                            new OracleParameter("pqty", i.qty),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pprod_code", i.prod_code),
                            new OracleParameter("pwh_code", i.wh_code),
                            new OracleParameter("ploc_code",i.loc_code),
                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) - nvl(:pqty,0),";
                        sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) - nvl(:pqty, 0)";
                        sql_upd_loc += "      where ic_entity = :pic_entity";
                        sql_upd_loc += "      and   prod_code = :pprod_code";
                        sql_upd_loc += "      and   wh_code   = :pwh_code";
                        sql_upd_loc += "      and   loc_code  = :ploc_code";

                        oraCommand.CommandText = sql_upd_loc;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }


                    // Update qty_ptw -> Trans Code = 'REC' 
                    // Find Rec Qty
                    string sqlRecQty = "select item, prod_code, bar_code, qty, qty_ptw, (nvl(qty,0) - nvl(qty_ptw,0)) net_qty";
                    sqlRecQty += " from whtran_det";
                    sqlRecQty += " where ic_entity = :pic_entity";
                    sqlRecQty += " and trans_code = 'REC'";
                    sqlRecQty += " and doc_no = :pdoc_no";
                    sqlRecQty += " and doc_code = :pdoc_code";
                    sqlRecQty += " and prod_code = :pprod_code";
                    sqlRecQty += " and bar_code = :pbar_code";
                    sqlRecQty += " and nvl(qty_ptw, 0) > 0";

                    List<PutAwayRecDetailView> recQtyData = ctx.Database.SqlQuery<PutAwayRecDetailView>(sqlRecQty, new OracleParameter("pic_entity", ventity),
                                                                                                               new OracleParameter("pdoc_no", vdoc_no),
                                                                                                               new OracleParameter("pdoc_code", vdoc_code),
                                                                                                               new OracleParameter("pprod_code", vprod_code),
                                                                                                               new OracleParameter("pbar_code", vbar_code)).ToList();
                    int vTotScnQty = i.qty;
                    foreach (var m in recQtyData)
                    {
                        if (vTotScnQty <= m.qty_ptw)
                        {
                            // Update PtwQty = Ptw_qty + vTotScnQty
                            // Update PtwQty
                            using (TransactionScope scope = new TransactionScope())
                            {
                                string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                                var dataConn = new OracleConnectionStringBuilder(strConn);
                                OracleConnection conn = new OracleConnection(dataConn.ToString());
                                conn.Open();

                                OracleCommand oraCommand = conn.CreateCommand();
                                OracleParameter[] param = new OracleParameter[]
                                {
                                new OracleParameter("pqty_scan", vTotScnQty),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", vdoc_no),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", vprod_code),
                                new OracleParameter("pbar_code", vbar_code),
                                new OracleParameter("pitem", m.item),
                                };
                                oraCommand.BindByName = true;
                                oraCommand.Parameters.AddRange(param);

                                string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) - nvl(:pqty_scan,0)";
                                sql_upd_ptwQty += " where ic_entity = :pic_entity";
                                sql_upd_ptwQty += " and trans_code = 'REC'";
                                sql_upd_ptwQty += " and doc_no = :pdoc_no";
                                sql_upd_ptwQty += " and doc_code = :pdoc_code";
                                sql_upd_ptwQty += " and prod_code = :pprod_code";
                                sql_upd_ptwQty += " and bar_code = :pbar_code";
                                sql_upd_ptwQty += " and item = :pitem";

                                oraCommand.CommandText = sql_upd_ptwQty;
                                oraCommand.ExecuteNonQuery();

                                conn.Close();
                                scope.Complete();

                            }
                            break;
                        }
                        else
                        {
                            vTotScnQty = vTotScnQty - m.qty_ptw;
                            // Update PtwQty = Ptw_qty + m.net_qty
                            // Update PtwQty
                            using (TransactionScope scope = new TransactionScope())
                            {
                                string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                                var dataConn = new OracleConnectionStringBuilder(strConn);
                                OracleConnection conn = new OracleConnection(dataConn.ToString());
                                conn.Open();

                                OracleCommand oraCommand = conn.CreateCommand();
                                OracleParameter[] param = new OracleParameter[]
                                {
                                new OracleParameter("pqty_scan", m.qty_ptw),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", vdoc_no),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", vprod_code),
                                new OracleParameter("pbar_code", vbar_code),
                                new OracleParameter("pitem", m.item),
                                };
                                oraCommand.BindByName = true;
                                oraCommand.Parameters.AddRange(param);

                                string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) - nvl(:pqty_scan,0)";
                                sql_upd_ptwQty += " where ic_entity = :pic_entity";
                                sql_upd_ptwQty += " and trans_code = 'REC'";
                                sql_upd_ptwQty += " and doc_no = :pdoc_no";
                                sql_upd_ptwQty += " and doc_code = :pdoc_code";
                                sql_upd_ptwQty += " and prod_code = :pprod_code";
                                sql_upd_ptwQty += " and bar_code = :pbar_code";
                                sql_upd_ptwQty += " and item = :pitem";

                                oraCommand.CommandText = sql_upd_ptwQty;
                                oraCommand.ExecuteNonQuery();

                                conn.Close();
                                scope.Complete();

                            }
                        }
                    }
                    // end Update qty_ptw -> Trans Code = 'REC'



                    view.ptwDetails.Add(new ModelViews.ScanPutAwayDetailView()
                    {
                        item_no = i.item,
                        prod_code = i.prod_code,
                        bar_code = i.bar_code,
                        wh_code = i.wh_code,
                        loc_code = i.loc_code,
                        qty = i.qty,
                    });
                }

                //throw new Exception(vTotRec + "|" + vTotPtw);

                return view;

            }
        }


        public ScanPutAwaysTotalView PostSearhPtwDetail(ScanPutAwayDetailSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vdoc_no = model.doc_no;
                string vdoc_code = model.doc_code;
                string vset_no = model.set_no;
                string vbar_code = model.bar_code;
                string vprod_code = model.prod_code;
                int vQty = 0;

                ScanPutAwaysTotalView view = new ModelViews.ScanPutAwaysTotalView()
                {

                    ptwDetails = new List<ModelViews.ScanPutAwayDetailView>()
                };

                string sqlp = "select a.item item_no, to_number(nvl(a.job_no,'0')) set_no, a.prod_code, b.prod_tname prod_name, a.bar_code, a.qty, a.wh_code, a.loc_code";
                sqlp += " from whtran_det a, product b";
                sqlp += " where a.prod_code = b.prod_code";
                sqlp += " and a.ic_entity = :pic_entity";
                sqlp += " and a.trans_code = 'PTW'";
                sqlp += " and a.doc_no = :pdoc_no";
                sqlp += " and a.doc_code = :pdoc_code";
                sqlp += " and nvl(a.job_no,'X') = nvl(:pjob_no, nvl(a.job_no, 'X'))";
                sqlp += " and a.prod_code = nvl(:pprod_code,a.prod_code)";
                sqlp += " and a.bar_code = nvl(:pbar_code,a.bar_code)";
                sqlp += " order by a.item";

                List<ScanPutAwayDetailView> ptwData = ctx.Database.SqlQuery<ScanPutAwayDetailView>(sqlp, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", vdoc_no), new OracleParameter("pdoc_code", vdoc_code), new OracleParameter("pjob_no", vset_no), new OracleParameter("pprod_code", vprod_code), new OracleParameter("pbar_code", vbar_code)).ToList();

                foreach (var i in ptwData)
                {
                    vQty = vQty + i.qty;
                    view.ptwDetails.Add(new ModelViews.ScanPutAwayDetailView()
                    {
                        item_no = i.item_no,
                        set_no = i.set_no,
                        prod_code = i.prod_code,
                        bar_code = i.bar_code,
                        prod_name = i.prod_name,
                        wh_code = i.wh_code,
                        loc_code = i.loc_code,
                        qty = i.qty,
                    });
                }
                view.total_qty = vQty;


                return view;

            }
        }

        public ScanPutAwaysTotalView ScanPutAwayAdd(ScanPutAwaySearchView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vbuild_type = model.build_type;
                var vdoc_no = model.doc_no;
                var vdoc_code = model.doc_code;
                var vdoc_date = model.doc_date;
                var vwc_code = model.wc_code;
                var vfr_wh_code = model.fr_wh_code;
                var vwh_code = model.wh_code;
                var vloc_code = model.loc_code;
                var vuser_id = model.user_id;
                var vqty_scan = model.qty;

                // 
                var vloc_code_def = "";
                var vbar_code = "";
                var vprod_name = "";
                var vquery_qty = "";
                var vSet_no = 0;
                var vQty = 0;
                var vRecQty = 0;
                var vPtwQty = 0;
                var vJIT_Date = "";
                var vItemNo = "";

                String[] strlist = model.bar_code.Split('|');

                // Check Scan QR Or Manual Key Set No and  Validate Data
                if (strlist.Length > 1) //Scan QR Code
                {
                    if (strlist.Length < 5)
                    {
                        throw new Exception("ข้อมูล QR Code ไม่ถูกต้องตาม Format (Product barcode|Product Name|Set No|Qty|JIT Date)");
                    }
                    else
                    {
                        vbar_code = strlist[0];
                        vprod_name = strlist[1];
                        vSet_no = int.Parse(strlist[2]);
                        vQty = int.Parse(strlist[3]);
                        vJIT_Date = strlist[4];
                    }

                }
                else //Manual Key Set NO
                {
                    vSet_no = int.Parse(strlist[0]);
                }

                // Find Doc_no, Prod_Barcode, Qty
                string sqlp = "select ref_pd_docno,prod_code,bar_code,pkg_barcode_set, sum(1) as confirm_qty";
                sqlp += " from pkg_barcode";
                sqlp += " where entity = :p_entity";
                sqlp += " and pkg_barcode_set = :p_setNo";
                sqlp += " group by ref_pd_docno,prod_code,bar_code,pkg_barcode_set";

                BarcodeSetDetailView barcode_Set_detail = ctx.Database.SqlQuery<BarcodeSetDetailView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_setNo", vSet_no)).FirstOrDefault();

                if (barcode_Set_detail == null)
                {
                    throw new Exception("ไม่พบข้อมูล SetNo : " + vSet_no + " ในระบบ");
                }

                //define model view
                BarcodeSetDetailView view = new ModelViews.BarcodeSetDetailView()
                {
                    ref_pd_docno = barcode_Set_detail.ref_pd_docno,
                    prod_code = barcode_Set_detail.prod_code,
                    bar_code = barcode_Set_detail.bar_code,
                    pkg_barcode_set = barcode_Set_detail.pkg_barcode_set,
                    confirm_qty = barcode_Set_detail.confirm_qty,
                };

                // Validate Data
                if (vbar_code != "")
                {
                    if (vbar_code != view.bar_code) { throw new Exception("ข้อมูล Product Barcode ของ QR Barcode ที่ SetNo : " + vSet_no + " ไม่ตรงกันกับในระบบ"); }
                }

                if (vQty > 0)
                {
                    if (vQty != view.confirm_qty) { throw new Exception("ข้อมูล จำนวนสินค้าที่ส่งมอบ ของ QR Barcode ที่ SetNo : " + vSet_no + " ไม่ตรงกันกับในระบบ"); }
                }

                if (vdoc_no != "")
                {
                    if (vdoc_no != view.ref_pd_docno) { throw new Exception("ข้อมูล เลขที่เอกสารใบส่งมอบ ของ SetNo : " + vSet_no + " ไม่ตรงกันกับในระบบ"); }
                }
                // Validate Location Default 
                string sqlw = "select rec_loc_def from wh_mast where wh_code = :p_whCode and rownum = 1";
                vloc_code_def = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_whCode", vfr_wh_code)).FirstOrDefault();
                if (vloc_code_def == null)
                {
                    throw new Exception("กรุณากำหนด Receive Location Default ของคลังสินค้า : " + vfr_wh_code + " ในระบบก่อนทำการ PutAway");
                }

                // validate Location
                string sql_loc = "select loc_code from whloc_mast where  wh_code = :p_whCode and  loc_code = :p_locCode and rownum = 1";
                string vChk_loc_code = ctx.Database.SqlQuery<string>(sql_loc, new OracleParameter("p_whCode", vwh_code), new OracleParameter("p_locCode", vloc_code)).FirstOrDefault();
                if (vChk_loc_code == null)
                {
                    throw new Exception("กรุณากำหนดข้อมูล Location : " + vloc_code + " ของคลังสินค้า : " + vwh_code + " ในระบบ Master ก่อนทำการ PutAway");
                }

                // validate จำนวน เกินยอดที่ putaway ไปแล้วหรือไม่
                vRecQty = 0;
                vPtwQty = 0;
                vqty_scan = view.confirm_qty;

                //ยอดที่ ยืนยันรับเข้าคลังที่ Loc H (Receive Loction Default)
                string sql_rec = "select to_char(nvl(sum(nvl(a.qty,0)),0)) rec_qty";
                sql_rec += " from  whtran_det a";
                sql_rec += " where a.trans_code = 'REC'";
                sql_rec += " and a.ic_entity = :p_entity";
                sql_rec += " and a.doc_no = :p_docNo";
                sql_rec += " and a.doc_code = :p_docCode";
                sql_rec += " and a.prod_code = :p_prodCode";
                sql_rec += " and a.bar_code = :p_barCode";
                sql_rec += " and a.loc_code  in (select rec_loc_def";
                sql_rec += "                     from wh_mast";
                sql_rec += "                     where wh_code = a.wh_code";
                sql_rec += "                     union";
                sql_rec += "                     select rtn_loc_def";
                sql_rec += "                     from wh_mast";
                sql_rec += "                     where wh_code = a.wh_code)";

                vquery_qty = ctx.Database.SqlQuery<string>(sql_rec, new OracleParameter("p_entity", ventity),
                                                                    new OracleParameter("p_docNo", view.ref_pd_docno),
                                                                    new OracleParameter("p_docCode", vdoc_code),
                                                                    new OracleParameter("p_prodCode", view.prod_code),
                                                                    new OracleParameter("p_barCode", view.bar_code)).FirstOrDefault();
                if (vquery_qty == "0" || vquery_qty == "" || vquery_qty == null)
                {
                    throw new Exception("ไม่พบสินค้า รหัส : " + view.prod_code + " ในเอกสารรับมอบเลขที่ : " + view.ref_pd_docno);
                }
                vRecQty = int.Parse(vquery_qty);

                //ยอดที่ Putaway แล้ว
                string sql_ptw = "select to_char(nvl(sum(nvl(a.qty,0)),0)) ptw_qty";
                sql_ptw += " from  whtran_det a";
                sql_ptw += " where a.trans_code = 'PTW'";
                sql_ptw += " and a.ic_entity = :p_entity";
                sql_ptw += " and a.doc_no = :p_docNo";
                sql_ptw += " and a.doc_code = :p_docCode";
                sql_ptw += " and a.prod_code = :p_prodCode";
                sql_ptw += " and a.bar_code = :p_barCode";

                vquery_qty = ctx.Database.SqlQuery<string>(sql_ptw, new OracleParameter("p_entity", ventity),
                                                                    new OracleParameter("p_docNo", view.ref_pd_docno),
                                                                    new OracleParameter("p_docCode", vdoc_code),
                                                                    new OracleParameter("p_prodCode", view.prod_code),
                                                                    new OracleParameter("p_barCode", view.bar_code)).FirstOrDefault();

                vPtwQty = int.Parse(vquery_qty);

                if ((vqty_scan + vPtwQty) > vRecQty)
                {
                    throw new Exception("PutAway เกินจำนวนที่รับมอบสินค้า ปัจจุบัน PutAway ไปแล้ว : " + vPtwQty + " ชิ้น จากยอดที่รับมอบทั้งหมด : " + vRecQty + " ชิ้น");
                }

                // Insert Transaction 
                // Insert whtran_mast 
                string sql_whMast = "select doc_no from whtran_mast where ic_entity = :p_entity and trans_code = 'PTW' and doc_no = :p_docNo and doc_code = :p_doc_Code";
                string vCheckDoc = ctx.Database.SqlQuery<string>(sql_whMast, new OracleParameter("p_entity", ventity), new OracleParameter("p_docNo", view.ref_pd_docno), new OracleParameter("p_doc_Code", vdoc_code)).SingleOrDefault();

                using (TransactionScope scope = new TransactionScope())
                {
                    if (vCheckDoc == null)
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                            {
                                    new OracleParameter("pic_entity",ventity),
                                    new OracleParameter("ptrans_code", "PTW"),
                                    new OracleParameter("pdoc_code", vdoc_code),
                                    new OracleParameter("pdoc_no", view.ref_pd_docno),
                                    new OracleParameter("pdoc_date", vdoc_date),
                                    new OracleParameter("pyr", int.Parse(DateTime.Now.ToString("yyyy"))),
                                    new OracleParameter("pprd", int.Parse(DateTime.Now.ToString("MM"))),
                                    new OracleParameter("psys_date", DateTime.Now),
                                    new OracleParameter("puser_code", vuser_id),
                                    new OracleParameter("pdoc_status", "PAL"),
                                    new OracleParameter("pwh_code", vfr_wh_code),
                                    new OracleParameter("ptot_qty", vqty_scan),
                                    new OracleParameter("pdel_status", "N"),

                            };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "insert into whtran_mast(ic_entity, trans_code, doc_code, doc_no, doc_date, yr, prd, sys_date, user_code, doc_status, wh_code, tot_qty, del_status) values(:pic_entity, :ptrans_code, :pdoc_code, :pdoc_no, to_date(:pdoc_date,'dd/mm/yyyy'), :pyr, :pprd, :psys_date, :puser_code, :pdoc_status, :pwh_code, :ptot_qty, :pdel_status)";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }
                    else
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                            new OracleParameter("ptot_qty", vqty_scan + vPtwQty),
                            new OracleParameter("puser_code", model.user_id),
                            new OracleParameter("psys_date", DateTime.Now),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pdoc_no", view.ref_pd_docno),
                            new OracleParameter("pdoc_code", vdoc_code),

                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "update whtran_mast set tot_qty = :ptot_qty, user_code = :puser_code, sys_date = :psys_date where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code ";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }
                }

                // Insert whtran_det
                // หา Max item
                string sql_ItemNo = "select to_char(nvl(max(nvl(item,0)),0)+1) from whtran_det where  ic_entity  = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and  doc_code   = :pdoc_code";
                vItemNo = ctx.Database.SqlQuery<string>(sql_ItemNo, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", view.ref_pd_docno), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());
                    conn.Open();

                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                        {
                             new OracleParameter("pic_entity",ventity),
                             new OracleParameter("ptrans_code", "PTW"),
                             new OracleParameter("pdoc_code", vdoc_code),
                             new OracleParameter("pdoc_no", view.ref_pd_docno),
                             new OracleParameter("pitem", int.Parse(vItemNo)),
                             new OracleParameter("pprod_code", view.prod_code),
                             new OracleParameter("pwh_code", vwh_code),
                             new OracleParameter("pwh_refer", vfr_wh_code),
                             new OracleParameter("ploc_code", vloc_code),
                             new OracleParameter("ploc_refer", vloc_code_def),
                             new OracleParameter("pqty", vqty_scan),
                             new OracleParameter("pjob_no", vSet_no.ToString()),
                             new OracleParameter("pbar_code", view.bar_code),
                             new OracleParameter("psys_date", DateTime.Now),
                             new OracleParameter("puser_code", model.user_id),

                        };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "insert into whtran_det(ic_entity, trans_code, doc_code, doc_no, item, prod_code, wh_code, wh_refer, loc_code, loc_refer, qty, job_no, bar_code, sys_date, user_code) values (:pic_entity, :ptrans_code, :pdoc_code, :pdoc_no, :pitem, :pprod_code, :pwh_code, :pwh_refer, :ploc_code, :ploc_refer, :pqty, :pjob_no, :pbar_code, :psys_date, :puser_code)";
                    oraCommand.ExecuteNonQuery();

                    conn.Close();
                    scope.Complete();

                }

                // Update Stock From Location
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());
                    conn.Open();

                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                            new OracleParameter("pqty_scan", vqty_scan),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pprod_code", view.prod_code),
                            new OracleParameter("pwh_code", vfr_wh_code),
                            new OracleParameter("ploc_code", vloc_code_def),
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);

                    string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) - nvl(:pqty_scan,0),";
                    sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) - nvl(:pqty_scan, 0)";
                    sql_upd_loc += "      where ic_entity = :pic_entity";
                    sql_upd_loc += "      and   prod_code = :pprod_code";
                    sql_upd_loc += "      and   wh_code   = :pwh_code";
                    sql_upd_loc += "      and   loc_code  = :ploc_code";

                    oraCommand.CommandText = sql_upd_loc;
                    oraCommand.ExecuteNonQuery();

                    conn.Close();
                    scope.Complete();

                }
                // Update Stock TO Location
                string sql_icLoc = "select prod_code from  ic_mast_loc where ic_entity = :pic_entity and prod_code = :pprod_code and wh_code = :pwh_code and loc_code = :ploc_code and rownum = 1";
                string vChkStockLoc = ctx.Database.SqlQuery<string>(sql_icLoc, new OracleParameter("pic_entity", ventity), new OracleParameter("pprod_code", view.prod_code), new OracleParameter("pwh_code", vwh_code), new OracleParameter("ploc_code", vloc_code)).SingleOrDefault();
                using (TransactionScope scope = new TransactionScope())
                {
                    if (vChkStockLoc == null)
                    {

                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                            {
                             new OracleParameter("pic_entity",ventity),
                             new OracleParameter("pprod_code", view.prod_code),
                             new OracleParameter("pwh_code", vwh_code),
                             new OracleParameter("ploc_code", vloc_code),
                             new OracleParameter("pqty_oh", vqty_scan),
                             new OracleParameter("pqty_avai", vqty_scan),
                             new OracleParameter("plin_date", DateTime.Now),
                             new OracleParameter("psys_date", DateTime.Now),
                             new OracleParameter("puser_code", model.user_id),
                            };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "insert into ic_mast_loc(ic_entity, prod_code, wh_code, loc_code, qty_oh, qty_ship, qty_all, qty_build, qty_avai, status, lin_date, sys_date, user_code) values (:pic_entity, :pprod_code, :pwh_code, :ploc_code, :pqty_oh, 0, 0, 0, :pqty_avai, 'A', :plin_date, :psys_date, :puser_code)";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }
                    else
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                            new OracleParameter("pqty_scan", vqty_scan),
                            new OracleParameter("pic_entity", ventity),
                            new OracleParameter("pprod_code", view.prod_code),
                            new OracleParameter("pwh_code", vwh_code),
                            new OracleParameter("ploc_code", vloc_code),

                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) + nvl(:pqty_scan,0),";
                        sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) + nvl(:pqty_scan, 0)";
                        sql_upd_loc += "      where ic_entity = :pic_entity";
                        sql_upd_loc += "      and   prod_code = :pprod_code";
                        sql_upd_loc += "      and   wh_code   = :pwh_code";
                        sql_upd_loc += "      and   loc_code  = :ploc_code";

                        oraCommand.CommandText = sql_upd_loc;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }

                }
                // ***** End Insert *****

                // Check Total Receive = Total Ptw ?
                // find total all Rec Qty
                string sql_totRec = "select sum(nvl(qty,0)) qty from whtran_det where ic_entity = :pic_entity and trans_code = 'REC' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                int vTotRec = ctx.Database.SqlQuery<int>(sql_totRec, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", view.ref_pd_docno), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();

                // find total all Rec Qty
                string sql_totPtw = "select sum(nvl(qty,0)) qty from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                int vTotPtw = ctx.Database.SqlQuery<int>(sql_totPtw, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", view.ref_pd_docno), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();

                if (vTotRec == vTotPtw)
                {
                    // Update Doc Status
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", view.ref_pd_docno),
                                new OracleParameter("pdoc_code", vdoc_code),
                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_status = "update whtran_mast set doc_status = 'APV'";
                        sql_upd_status += " where ic_entity = :pic_entity";
                        sql_upd_status += " and trans_code = 'PTW'";
                        sql_upd_status += " and doc_no = :pdoc_no";
                        sql_upd_status += " and doc_code = :pdoc_code";

                        oraCommand.CommandText = sql_upd_status;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }
                }
                // End Check Total Receive = Total Ptw ?

                // Update qty_ptw -> Trans Code = 'REC' 
                // Find Rec Qty
                string sqlRecQty = "select item, prod_code, bar_code, qty, qty_ptw, (nvl(qty,0) - nvl(qty_ptw,0)) net_qty";
                sqlRecQty += " from whtran_det";
                sqlRecQty += " where ic_entity = :pic_entity";
                sqlRecQty += " and trans_code = 'REC'";
                sqlRecQty += " and doc_no = :pdoc_no";
                sqlRecQty += " and doc_code = :pdoc_code";
                sqlRecQty += " and prod_code = :pprod_code";
                sqlRecQty += " and bar_code = :pbar_code";
                sqlRecQty += " and(nvl(qty, 0) - nvl(qty_ptw, 0)) > 0";

                List<PutAwayRecDetailView> recQtyData = ctx.Database.SqlQuery<PutAwayRecDetailView>(sqlRecQty, new OracleParameter("pic_entity", ventity), 
                                                                                                           new OracleParameter("pdoc_no", view.ref_pd_docno),
                                                                                                           new OracleParameter("pdoc_code", vdoc_code),
                                                                                                           new OracleParameter("pprod_code", view.prod_code),
                                                                                                           new OracleParameter("pbar_code", view.bar_code)).ToList();
                int vTotScnQty = vqty_scan;
                foreach (var m in recQtyData)
                {
                    if  (vTotScnQty <= m.net_qty)
                    {
                        // Update PtwQty = Ptw_qty + vTotScnQty
                        // Update PtwQty
                        using (TransactionScope scope = new TransactionScope())
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("pqty_scan", vTotScnQty),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", view.ref_pd_docno),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", view.prod_code),
                                new OracleParameter("pbar_code", view.bar_code),
                                new OracleParameter("pitem", m.item),
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);

                            string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) + nvl(:pqty_scan,0)";
                            sql_upd_ptwQty += " where ic_entity = :pic_entity";
                            sql_upd_ptwQty += " and trans_code = 'REC'";
                            sql_upd_ptwQty += " and doc_no = :pdoc_no";
                            sql_upd_ptwQty += " and doc_code = :pdoc_code";
                            sql_upd_ptwQty += " and prod_code = :pprod_code";
                            sql_upd_ptwQty += " and bar_code = :pbar_code";
                            sql_upd_ptwQty += " and item = :pitem";

                            oraCommand.CommandText = sql_upd_ptwQty;
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();

                        }
                        break;
                    }
                    else
                    {
                        vTotScnQty = vTotScnQty - m.net_qty;
                        // Update PtwQty = Ptw_qty + m.net_qty
                        // Update PtwQty
                        using (TransactionScope scope = new TransactionScope())
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("pqty_scan", m.net_qty),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", view.ref_pd_docno),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", view.prod_code),
                                new OracleParameter("pbar_code", view.bar_code),
                                new OracleParameter("pitem", m.item),
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);

                            string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) + nvl(:pqty_scan,0)";
                            sql_upd_ptwQty += " where ic_entity = :pic_entity";
                            sql_upd_ptwQty += " and trans_code = 'REC'";
                            sql_upd_ptwQty += " and doc_no = :pdoc_no";
                            sql_upd_ptwQty += " and doc_code = :pdoc_code";
                            sql_upd_ptwQty += " and prod_code = :pprod_code";
                            sql_upd_ptwQty += " and bar_code = :pbar_code";
                            sql_upd_ptwQty += " and item = :pitem";

                            oraCommand.CommandText = sql_upd_ptwQty;
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();

                        }
                    }
                }
                // end Update qty_ptw -> Trans Code = 'REC'

                // find prod name
                string sql_prodtname = "select prod_tname from product where prod_code = :pprod_code and rownum = 1";
                vprod_name = ctx.Database.SqlQuery<string>(sql_prodtname, new OracleParameter("pprod_code", view.prod_code)).SingleOrDefault();

                //define model view
                ScanPutAwaysTotalView ptwTotview = new ModelViews.ScanPutAwaysTotalView()
                {
                    total_qty = vqty_scan + vPtwQty,
                    ptwDetails = new List<ModelViews.ScanPutAwayDetailView>()
                };
                ptwTotview.ptwDetails.Add(new ModelViews.ScanPutAwayDetailView()
                {
                    item_no = int.Parse(vItemNo),
                    set_no = vSet_no,
                    prod_code = view.prod_code,
                    bar_code = view.bar_code,
                    prod_name = vprod_name,
                    wh_code = vwh_code,
                    loc_code = vloc_code,
                    qty = vqty_scan

                });

                //return data to contoller
                return ptwTotview;
                //throw new Exception(ventity + "|" + view.prod_code + "|" + vfr_wh_code + "|" + vloc_code_def);
            }
        }

        public ScanPutAwaysTotalView ScanPutAwayManualAdd(ScanPutAwaySearchView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vbuild_type = model.build_type;
                var vdoc_no = model.doc_no;
                var vdoc_code = model.doc_code;
                var vdoc_date = model.doc_date;
                var vwc_code = model.wc_code;
                var vfr_wh_code = model.fr_wh_code;
                var vwh_code = model.wh_code;
                var vloc_code = model.loc_code;
                var vuser_id = model.user_id;
                var vqty_scan = model.qty;
                var vbar_code = model.bar_code;


                // 
                var vloc_code_def = "";
                var vprod_code = "";
                var vprod_name = "";
                var vquery_qty = "";
                var vSet_no = 0;
                //var vQty = 0;
                var vRecQty = 0;
                var vPtwQty = 0;
                //var vJIT_Date = "";
                var vItemNo = "";

                // Check Scan QR Or Manual Key Set No and  Validate Data
                if (vbar_code.Length != 13) //Scan QR Code
                {
                    throw new Exception("ข้อมูล BarCode ไม่ถูกต้องตาม Format");
                }

                // find product Code 
                string sqlprod = "select prod_code from product where bar_code = :pbar_code and rownum = 1";
                vprod_code = ctx.Database.SqlQuery<string>(sqlprod, new OracleParameter("pbar_code", vbar_code)).FirstOrDefault();
                if (vprod_code == null)
                {
                    throw new Exception("ไม่พบข้อมูลสินค้า ของรหัส Barcode : " + vbar_code + " ในระบบ");
                }


                // Validate Location Default 
                string sqlw = "select rec_loc_def from wh_mast where wh_code = :p_whCode and rownum = 1";
                vloc_code_def = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_whCode", vfr_wh_code)).FirstOrDefault();
                if (vloc_code_def == null)
                {
                    throw new Exception("กรุณากำหนด Receive Location Default ของคลังสินค้า : " + vfr_wh_code + " ในระบบก่อนทำการ PutAway");
                }

                // validate Location
                string sql_loc = "select loc_code from whloc_mast where  wh_code = :p_whCode and  loc_code = :p_locCode and rownum = 1";
                string vChk_loc_code = ctx.Database.SqlQuery<string>(sql_loc, new OracleParameter("p_whCode", vwh_code), new OracleParameter("p_locCode", vloc_code)).FirstOrDefault();
                if (vChk_loc_code == null)
                {
                    throw new Exception("กรุณากำหนดข้อมูล Location : " + vloc_code + " ของคลังสินค้า : " + vwh_code + " ในระบบ Master ก่อนทำการ PutAway");
                }

                // validate จำนวน เกินยอดที่ putaway ไปแล้วหรือไม่
                vRecQty = 0;
                vPtwQty = 0;

                //ยอดที่ ยืนยันรับเข้าคลังที่ Loc H (Receive Loction Default)
                string sql_rec = "select to_char(nvl(sum(nvl(a.qty,0)),0)) rec_qty";
                sql_rec += " from  whtran_det a";
                sql_rec += " where a.trans_code = 'REC'";
                sql_rec += " and a.ic_entity = :p_entity";
                sql_rec += " and a.doc_no = :p_docNo";
                sql_rec += " and a.doc_code = :p_docCode";
                sql_rec += " and a.prod_code = :p_prodCode";
                sql_rec += " and a.bar_code = :p_barCode";
                sql_rec += " and a.loc_code  in (select rec_loc_def";
                sql_rec += "                     from wh_mast";
                sql_rec += "                     where wh_code = a.wh_code";
                sql_rec += "                     union";
                sql_rec += "                     select rtn_loc_def";
                sql_rec += "                     from wh_mast";
                sql_rec += "                     where wh_code = a.wh_code)";

                vquery_qty = ctx.Database.SqlQuery<string>(sql_rec, new OracleParameter("p_entity", ventity),
                                                                    new OracleParameter("p_docNo", vdoc_no),
                                                                    new OracleParameter("p_docCode", vdoc_code),
                                                                    new OracleParameter("p_prodCode", vprod_code),
                                                                    new OracleParameter("p_barCode", vbar_code)).FirstOrDefault();

                if (vquery_qty == "0" || vquery_qty == "" || vquery_qty == null)
                {
                    throw new Exception("ไม่พบสินค้า รหัส : " + vprod_code + " ในเอกสารรับมอบเลขที่ : " + vdoc_no);
                }
                vRecQty = int.Parse(vquery_qty);



                //ยอดที่ Putaway แล้ว
                string sql_ptw = "select to_char(nvl(sum(nvl(a.qty,0)),0)) ptw_qty";
                sql_ptw += " from  whtran_det a";
                sql_ptw += " where a.trans_code = 'PTW'";
                sql_ptw += " and a.ic_entity = :p_entity";
                sql_ptw += " and a.doc_no = :p_docNo";
                sql_ptw += " and a.doc_code = :p_docCode";
                sql_ptw += " and a.prod_code = :p_prodCode";
                sql_ptw += " and a.bar_code = :p_barCode";

                vquery_qty = ctx.Database.SqlQuery<string>(sql_ptw, new OracleParameter("p_entity", ventity),
                                                                    new OracleParameter("p_docNo", vdoc_no),
                                                                    new OracleParameter("p_docCode", vdoc_code),
                                                                    new OracleParameter("p_prodCode", vprod_code),
                                                                    new OracleParameter("p_barCode", vbar_code)).FirstOrDefault();

                vPtwQty = int.Parse(vquery_qty);

                if ((vqty_scan + vPtwQty) > vRecQty)
                {
                    throw new Exception("PutAway เกินจำนวนที่รับมอบสินค้า ปัจจุบัน PutAway ไปแล้ว : " + vPtwQty + " ชิ้น จากยอดที่รับมอบทั้งหมด : " + vRecQty + " ชิ้น");
                }

                // Insert Transaction 
                // Insert whtran_mast 
                string sql_whMast = "select doc_no from whtran_mast where ic_entity = :p_entity and trans_code = 'PTW' and doc_no = :p_docNo and doc_code = :p_doc_Code";
                string vCheckDoc = ctx.Database.SqlQuery<string>(sql_whMast, new OracleParameter("p_entity", ventity), new OracleParameter("p_docNo", vdoc_no), new OracleParameter("p_doc_Code", vdoc_code)).SingleOrDefault();

                using (TransactionScope scope = new TransactionScope())
                {
                    if (vCheckDoc == null)
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                            {
                                                    new OracleParameter("pic_entity",ventity),
                                                    new OracleParameter("ptrans_code", "PTW"),
                                                    new OracleParameter("pdoc_code", vdoc_code),
                                                    new OracleParameter("pdoc_no", vdoc_no),
                                                    new OracleParameter("pdoc_date", vdoc_date),
                                                    new OracleParameter("pyr", int.Parse(DateTime.Now.ToString("yyyy"))),
                                                    new OracleParameter("pprd", int.Parse(DateTime.Now.ToString("MM"))),
                                                    new OracleParameter("psys_date", DateTime.Now),
                                                    new OracleParameter("puser_code", vuser_id),
                                                    new OracleParameter("pdoc_status", "PAL"),
                                                    new OracleParameter("pwh_code", vfr_wh_code),
                                                    new OracleParameter("ptot_qty", vqty_scan),
                                                    new OracleParameter("pdel_status", "N"),

                            };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "insert into whtran_mast(ic_entity, trans_code, doc_code, doc_no, doc_date, yr, prd, sys_date, user_code, doc_status, wh_code, tot_qty, del_status) values(:pic_entity, :ptrans_code, :pdoc_code, :pdoc_no, to_date(:pdoc_date,'dd/mm/yyyy'), :pyr, :pprd, :psys_date, :puser_code, :pdoc_status, :pwh_code, :ptot_qty, :pdel_status)";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }
                    else
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                                            new OracleParameter("ptot_qty", vqty_scan + vPtwQty),
                                            new OracleParameter("puser_code", model.user_id),
                                            new OracleParameter("psys_date", DateTime.Now),
                                            new OracleParameter("pic_entity", ventity),
                                            new OracleParameter("pdoc_no", vdoc_no),
                                            new OracleParameter("pdoc_code", vdoc_code),

                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "update whtran_mast set tot_qty = :ptot_qty, user_code = :puser_code, sys_date = :psys_date where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code ";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }
                }

                // Insert whtran_det
                // หา Max item
                string sql_ItemNo = "select to_char(nvl(max(nvl(item,0)),0)+1) from whtran_det where  ic_entity  = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and  doc_code   = :pdoc_code";
                vItemNo = ctx.Database.SqlQuery<string>(sql_ItemNo, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", vdoc_no), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());
                    conn.Open();

                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                        {
                                             new OracleParameter("pic_entity",ventity),
                                             new OracleParameter("ptrans_code", "PTW"),
                                             new OracleParameter("pdoc_code", vdoc_code),
                                             new OracleParameter("pdoc_no", vdoc_no),
                                             new OracleParameter("pitem", int.Parse(vItemNo)),
                                             new OracleParameter("pprod_code", vprod_code),
                                             new OracleParameter("pwh_code", vwh_code),
                                             new OracleParameter("pwh_refer", vfr_wh_code),
                                             new OracleParameter("ploc_code", vloc_code),
                                             new OracleParameter("ploc_refer", vloc_code_def),
                                             new OracleParameter("pqty", vqty_scan),
                                             new OracleParameter("pbar_code", vbar_code),
                                             new OracleParameter("psys_date", DateTime.Now),
                                             new OracleParameter("puser_code", model.user_id),

                        };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "insert into whtran_det(ic_entity, trans_code, doc_code, doc_no, item, prod_code, wh_code, wh_refer, loc_code, loc_refer, qty, bar_code, sys_date, user_code) values (:pic_entity, :ptrans_code, :pdoc_code, :pdoc_no, :pitem, :pprod_code, :pwh_code, :pwh_refer, :ploc_code, :ploc_refer, :pqty, :pbar_code, :psys_date, :puser_code)";
                    oraCommand.ExecuteNonQuery();

                    conn.Close();
                    scope.Complete();

                }

                // Update Stock From Location
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());
                    conn.Open();

                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                                            new OracleParameter("pqty_scan", vqty_scan),
                                            new OracleParameter("pic_entity", ventity),
                                            new OracleParameter("pprod_code", vprod_code),
                                            new OracleParameter("pwh_code", vfr_wh_code),
                                            new OracleParameter("ploc_code", vloc_code_def),
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);

                    string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) - nvl(:pqty_scan,0),";
                    sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) - nvl(:pqty_scan, 0)";
                    sql_upd_loc += "      where ic_entity = :pic_entity";
                    sql_upd_loc += "      and   prod_code = :pprod_code";
                    sql_upd_loc += "      and   wh_code   = :pwh_code";
                    sql_upd_loc += "      and   loc_code  = :ploc_code";

                    oraCommand.CommandText = sql_upd_loc;
                    oraCommand.ExecuteNonQuery();

                    conn.Close();
                    scope.Complete();

                }
                // Update Stock TO Location
                string sql_icLoc = "select prod_code from  ic_mast_loc where ic_entity = :pic_entity and prod_code = :pprod_code and wh_code = :pwh_code and loc_code = :ploc_code and rownum = 1";
                string vChkStockLoc = ctx.Database.SqlQuery<string>(sql_icLoc, new OracleParameter("pic_entity", ventity), new OracleParameter("pprod_code", vprod_code), new OracleParameter("pwh_code", vwh_code), new OracleParameter("ploc_code", vloc_code)).SingleOrDefault();
                using (TransactionScope scope = new TransactionScope())
                {
                    if (vChkStockLoc == null)
                    {

                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                            {
                                             new OracleParameter("pic_entity",ventity),
                                             new OracleParameter("pprod_code", vprod_code),
                                             new OracleParameter("pwh_code", vwh_code),
                                             new OracleParameter("ploc_code", vloc_code),
                                             new OracleParameter("pqty_oh", vqty_scan),
                                             new OracleParameter("pqty_avai", vqty_scan),
                                             new OracleParameter("plin_date", DateTime.Now),
                                             new OracleParameter("psys_date", DateTime.Now),
                                             new OracleParameter("puser_code", model.user_id),
                            };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);
                        oraCommand.CommandText = "insert into ic_mast_loc(ic_entity, prod_code, wh_code, loc_code, qty_oh, qty_ship, qty_all, qty_build, qty_avai, status, lin_date, sys_date, user_code) values (:pic_entity, :pprod_code, :pwh_code, :ploc_code, :pqty_oh, 0, 0, 0, :pqty_avai, 'A', :plin_date, :psys_date, :puser_code)";
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }
                    else
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                                            new OracleParameter("pqty_scan", vqty_scan),
                                            new OracleParameter("pic_entity", ventity),
                                            new OracleParameter("pprod_code", vprod_code),
                                            new OracleParameter("pwh_code", vwh_code),
                                            new OracleParameter("ploc_code", vloc_code),

                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_loc = "update ic_mast_loc  set qty_oh = nvl(qty_oh,0) + nvl(:pqty_scan,0),";
                        sql_upd_loc += "                          qty_avai = nvl(qty_avai, 0) + nvl(:pqty_scan, 0)";
                        sql_upd_loc += "      where ic_entity = :pic_entity";
                        sql_upd_loc += "      and   prod_code = :pprod_code";
                        sql_upd_loc += "      and   wh_code   = :pwh_code";
                        sql_upd_loc += "      and   loc_code  = :ploc_code";

                        oraCommand.CommandText = sql_upd_loc;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();
                    }

                }
                // ***** End Insert *****

                // Check Total Receive = Total Ptw ?
                // find total all Rec Qty
                string sql_totRec = "select sum(nvl(qty,0)) qty from whtran_det where ic_entity = :pic_entity and trans_code = 'REC' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                int vTotRec = ctx.Database.SqlQuery<int>(sql_totRec, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", vdoc_no), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();

                // find total all Rec Qty
                string sql_totPtw = "select sum(nvl(qty,0)) qty from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                int vTotPtw = ctx.Database.SqlQuery<int>(sql_totPtw, new OracleParameter("pic_entity", ventity), new OracleParameter("pdoc_no", vdoc_no), new OracleParameter("pdoc_code", vdoc_code)).SingleOrDefault();

                if (vTotRec == vTotPtw)
                {
                    // Update Doc Status
                    using (TransactionScope scope = new TransactionScope())
                    {
                        string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                        var dataConn = new OracleConnectionStringBuilder(strConn);
                        OracleConnection conn = new OracleConnection(dataConn.ToString());
                        conn.Open();

                        OracleCommand oraCommand = conn.CreateCommand();
                        OracleParameter[] param = new OracleParameter[]
                        {
                                                new OracleParameter("pic_entity", ventity),
                                                new OracleParameter("pdoc_no", vdoc_no),
                                                new OracleParameter("pdoc_code", vdoc_code),
                        };
                        oraCommand.BindByName = true;
                        oraCommand.Parameters.AddRange(param);

                        string sql_upd_status = "update whtran_mast set doc_status = 'APV'";
                        sql_upd_status += " where ic_entity = :pic_entity";
                        sql_upd_status += " and trans_code = 'PTW'";
                        sql_upd_status += " and doc_no = :pdoc_no";
                        sql_upd_status += " and doc_code = :pdoc_code";

                        oraCommand.CommandText = sql_upd_status;
                        oraCommand.ExecuteNonQuery();

                        conn.Close();
                        scope.Complete();

                    }
                }
                // End Check Total Receive = Total Ptw ?


                // Update qty_ptw -> Trans Code = 'REC' 
                // Find Rec Qty
                string sqlRecQty = "select item, prod_code, bar_code, qty, qty_ptw, (nvl(qty,0) - nvl(qty_ptw,0)) net_qty";
                sqlRecQty += " from whtran_det";
                sqlRecQty += " where ic_entity = :pic_entity";
                sqlRecQty += " and trans_code = 'REC'";
                sqlRecQty += " and doc_no = :pdoc_no";
                sqlRecQty += " and doc_code = :pdoc_code";
                sqlRecQty += " and prod_code = :pprod_code";
                sqlRecQty += " and bar_code = :pbar_code";
                sqlRecQty += " and(nvl(qty, 0) - nvl(qty_ptw, 0)) > 0";

                List<PutAwayRecDetailView> recQtyData = ctx.Database.SqlQuery<PutAwayRecDetailView>(sqlRecQty, new OracleParameter("pic_entity", ventity),
                                                                                                           new OracleParameter("pdoc_no", vdoc_no),
                                                                                                           new OracleParameter("pdoc_code", vdoc_code),
                                                                                                           new OracleParameter("pprod_code", vprod_code),
                                                                                                           new OracleParameter("pbar_code", vbar_code)).ToList();
                int vTotScnQty = vqty_scan;
                foreach (var m in recQtyData)
                {
                    if (vTotScnQty <= m.net_qty)
                    {
                        // Update PtwQty = Ptw_qty + vTotScnQty
                        // Update PtwQty
                        using (TransactionScope scope = new TransactionScope())
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("pqty_scan", vTotScnQty),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", vdoc_no),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", vprod_code),
                                new OracleParameter("pbar_code", vbar_code),
                                new OracleParameter("pitem", m.item),
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);

                            string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) + nvl(:pqty_scan,0)";
                            sql_upd_ptwQty += " where ic_entity = :pic_entity";
                            sql_upd_ptwQty += " and trans_code = 'REC'";
                            sql_upd_ptwQty += " and doc_no = :pdoc_no";
                            sql_upd_ptwQty += " and doc_code = :pdoc_code";
                            sql_upd_ptwQty += " and prod_code = :pprod_code";
                            sql_upd_ptwQty += " and bar_code = :pbar_code";
                            sql_upd_ptwQty += " and item = :pitem";

                            oraCommand.CommandText = sql_upd_ptwQty;
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();

                        }
                        break;
                    }
                    else
                    {
                        vTotScnQty = vTotScnQty - m.net_qty;
                        // Update PtwQty = Ptw_qty + m.net_qty
                        // Update PtwQty
                        using (TransactionScope scope = new TransactionScope())
                        {
                            string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                            var dataConn = new OracleConnectionStringBuilder(strConn);
                            OracleConnection conn = new OracleConnection(dataConn.ToString());
                            conn.Open();

                            OracleCommand oraCommand = conn.CreateCommand();
                            OracleParameter[] param = new OracleParameter[]
                            {
                                new OracleParameter("pqty_scan", m.net_qty),
                                new OracleParameter("pic_entity", ventity),
                                new OracleParameter("pdoc_no", vdoc_no),
                                new OracleParameter("pdoc_code", vdoc_code),
                                new OracleParameter("pprod_code", vprod_code),
                                new OracleParameter("pbar_code", vbar_code),
                                new OracleParameter("pitem", m.item),
                            };
                            oraCommand.BindByName = true;
                            oraCommand.Parameters.AddRange(param);

                            string sql_upd_ptwQty = "update  whtran_det set qty_ptw = nvl(qty_ptw,0) + nvl(:pqty_scan,0)";
                            sql_upd_ptwQty += " where ic_entity = :pic_entity";
                            sql_upd_ptwQty += " and trans_code = 'REC'";
                            sql_upd_ptwQty += " and doc_no = :pdoc_no";
                            sql_upd_ptwQty += " and doc_code = :pdoc_code";
                            sql_upd_ptwQty += " and prod_code = :pprod_code";
                            sql_upd_ptwQty += " and bar_code = :pbar_code";
                            sql_upd_ptwQty += " and item = :pitem";

                            oraCommand.CommandText = sql_upd_ptwQty;
                            oraCommand.ExecuteNonQuery();

                            conn.Close();
                            scope.Complete();

                        }
                    }
                }
                // end Update qty_ptw -> Trans Code = 'REC'


                // find prod name
                string sql_prodtname = "select prod_tname from product where prod_code = :pprod_code and rownum = 1";
                vprod_name = ctx.Database.SqlQuery<string>(sql_prodtname, new OracleParameter("pprod_code", vprod_code)).SingleOrDefault();

                //define model view
                ScanPutAwaysTotalView ptwTotview = new ModelViews.ScanPutAwaysTotalView()
                {
                    total_qty = vqty_scan + vPtwQty,
                    ptwDetails = new List<ModelViews.ScanPutAwayDetailView>()
                };
                ptwTotview.ptwDetails.Add(new ModelViews.ScanPutAwayDetailView()
                {
                    item_no = int.Parse(vItemNo),
                    set_no = vSet_no,
                    prod_code = vprod_code,
                    bar_code = vbar_code,
                    prod_name = vprod_name,
                    wh_code = vwh_code,
                    loc_code = vloc_code,
                    qty = vqty_scan

                });

                //return data to contoller
                return ptwTotview;
                //throw new Exception(vTotRec + "|" + vTotPtw);
            }
        }

        public WhDefaultView GetWhDefault(string ic_entity)
        {
            using (var ctx = new ConXContext())
            {

                if (ic_entity == "" || ic_entity == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล Entity");
                }

                string sql = "select wh_default wh_code from  ic_control where ic_entity= :pic_entity and rownum = 1";
                WhDefaultView datas = ctx.Database.SqlQuery<WhDefaultView>(sql, new OracleParameter("pic_entity", ic_entity)).SingleOrDefault();

                if (datas == null)
                {
                    throw new Exception("ไม่มีข้อมูลคลัง Default");
                }

                //define model view
                WhDefaultView view = new ModelViews.WhDefaultView()
                {
                    wh_code = datas.wh_code
                };
                //return data to contoller
                return view;
            }

        }

        public DeptDefaultView GetDeptDefault(string ic_entity, string user_id)
        {
            using (var ctx = new ConXContext())
            {

                if (ic_entity == "" || ic_entity == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล Entity");
                }

                if (user_id == "" || user_id == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล Os User");
                }

                string sql = "SELECT dept_code from auth_function, wc_mast where function_id = 'ICPWSCFM' and user_id = :puser_id and entity_code = :pentity_code and dept_code = wc_code  and rownum=1 ORDER BY 1";
                DeptDefaultView datas = ctx.Database.SqlQuery<DeptDefaultView>(sql, new OracleParameter("puser_id", user_id),
                                                                                    new OracleParameter("pentity_code", ic_entity)).SingleOrDefault();

                if (datas == null)
                {
                    throw new Exception("ไม่มีข้อมูลหน่วย Putaway");
                }

                //define model view
                DeptDefaultView view = new ModelViews.DeptDefaultView()
                {
                    dept_code = datas.dept_code
                };
                //return data to contoller
                return view;
            }
        }

        public VerifyLocView GetVerifyLoc(string loc_code)
        {
            using (var ctx = new ConXContext())
            {

                if (loc_code == "" || loc_code == null)
                {
                    throw new Exception("กรุณาระบุข้อมูล Location");
                }


                string sql = "select loc_code from whloc_mast where loc_st = 'A' and   loc_code = :ploc_code and   rownum = 1";
                VerifyLocView datas = ctx.Database.SqlQuery<VerifyLocView>(sql, new OracleParameter("ploc_code", loc_code)).SingleOrDefault();

                if (datas == null)
                {
                    throw new Exception("ไม่มีข้อมูล Location : " + loc_code + " ในแฟ้ม Master");
                }

                //define model view
                VerifyLocView view = new ModelViews.VerifyLocView()
                {
                    loc_code = datas.loc_code
                };
                //return data to contoller
                return view;
            }
        }


    }
}