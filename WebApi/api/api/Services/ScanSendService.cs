using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;

namespace api.Services
{
    public class ScanSendService : IScanSendService
    {
        public void delete(ScanSendView scanView)
        {
            using(var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    // Udpaet MPS_DET_WC
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_pcs_barcode", scanView.pcs_barcode),
                        new OracleParameter("p_entity", scanView.entity),
                        new OracleParameter("p_wc_code", scanView.wc_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc set mps_st='N'  where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";
                    oraCommand.ExecuteNonQuery();


                    // Delete PKG Barcode
                    OracleCommand oraCommandbarcode = conn.CreateCommand();
                    OracleParameter[] parambarcode = new OracleParameter[]
                    {
                        new OracleParameter("p_tran_no", scanView.pcs_barcode),
                        new OracleParameter("p_entity", scanView.entity),
                        new OracleParameter("p_wc_code", scanView.wc_code)
                    };

                    oraCommandbarcode.BindByName = true;
                    oraCommandbarcode.Parameters.AddRange(parambarcode);
                    oraCommandbarcode.CommandText = "delete from pkg_barcode where entity = :p_entity and tran_no = :p_tran_no and wc_code =:p_wc_code";
                    oraCommandbarcode.ExecuteNonQuery();

                    conn.Close();

                    scope.Complete();
                }
            }
        }

        public ScanQtyView getScanQty(ScanSendView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vreq_date = model.req_date;
                var vwc_code = model.wc_code;
                var vprod_code = model.prod_code;
                

                string sqlp = "select nvl(pcs_per_pack,0) from product  where prod_code = :p_prod_code";
                int vset_qty = ctx.Database.SqlQuery<int>(sqlp, new OracleParameter("p_prod_code", vprod_code)).FirstOrDefault();

                string sqls = "select count(*) from pkg_barcode where pkg_barcode_set is null and prod_code=:p_prod_code and wc_code=:p_wc_code";
                int vscan = ctx.Database.SqlQuery<int>(sqls, new OracleParameter("p_prod_code", vprod_code), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault();


                //define model view
                ScanQtyView view = new ModelViews.ScanQtyView()
                {
                    set_qty = vset_qty,
                    scan_qty = vscan,   
                };


                //return data to contoller
                return view;

            }
        }

        public List<SetNoView> getSetNo(SetNoSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vtran_date = model.tran_date;
                var vwc_code = model.wc_code;

                string sql = "select distinct a.pkg_barcode_set set_no , a.prod_code , count(*) scan_qty , b.pcs_per_pack set_qty " +
                    "from pkg_barcode a , product b " +
                    "where a.prod_code = b.prod_code " +
                    "and  a.entity = :p_entity " +
                    "and  a.tran_date = to_date(:p_tran_date,'dd/mm/yyyy') " +
                    "and a.wc_code = :p_wc_code  " +
                    "and a.pkg_barcode_set is not null " +
                    "group by a.pkg_barcode_set , a.prod_code , b.pcs_per_pack " +
                    "order by a.pkg_barcode_set";
                List<SetNoView> set_no = ctx.Database.SqlQuery<SetNoView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_tran_date", vtran_date), new OracleParameter("p_wc_code", vwc_code)).ToList();

                List<SetNoView> setViews = new List<SetNoView>();
               


                foreach (var x in set_no)
                {
                    string sqld = "select a.entity , a.prod_code , b.prod_tname prod_name , a.tran_no pcs_barcode , a.por_no job_no , to_char(a.tran_date,'dd/mm/yyyy') req_date , a.bar_code , a.wc_code , to_char(a.fin_date,'dd/mm/yyyy') fin_date " +
                        "from pkg_barcode a , product b " +
                        "where a.prod_code = b.prod_code " +
                        "and a.entity = :p_entity " +
                        "and a.wc_code = :p_wc_code " +
                        "and a.pkg_barcode_set = :p_set_no";

                    List<SetNoDataView> data = ctx.Database.SqlQuery<SetNoDataView>(sqld, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_set_no", x.set_no)).ToList();

                    List<SetNoDataView> dataViews = new List<SetNoDataView>();

                    foreach (var z in data)
                    {
                        SetNoDataView dView = new SetNoDataView()
                        {
                           entity = z.entity,
                           prod_code = z.prod_code,
                           prod_name = z.prod_name,
                           pcs_barcode = z.pcs_barcode,
                           job_no = z.job_no,
                           bar_code = z.bar_code,
                           req_date = z.req_date,
                           fin_date = z.fin_date,
                           wc_code = z.wc_code

                        };
                        dataViews.Add(dView);

                    }
                    

                    SetNoView view = new SetNoView()
                    {
                        set_no = x.set_no,
                        tran_date = vtran_date,
                        set_qty = x.set_qty,
                        scan_qty = x.scan_qty,
                        datas = dataViews

                    };

                    setViews.Add(view);

                }    

      
                return setViews;

            }
        }

        public void PringSticker(ScanSendView model)
        {
            System.Diagnostics.Process.Start("net.exe", @"use L: / delete");
            System.Diagnostics.Process.Start("net.exe", @"use L: \\192.168.8.14\Data TOP@007* /USER:192.168.8.14\webadmin").WaitForExit();

            using (var ctx = new ConXContext())
            {
                var vprod_code = model.prod_code;
                var vbar_code = model.bar_code;
                var vprod_name = model.prod_name;
                var vuser_id = model.user_id;
                var vreq_date = model.req_date;
                var vmfg_date = "MFG Date " + model.fin_date;
                var vqty = model.set_qty;
                //var vset_no = "Set No." + model.set_no;
                var vset_no = "";
                var vjob_no = model.job_no;
                var vwc_code = model.wc_code;
                var ventity = model.entity;


                string sqlp = "select  a.prnt_point_name printer_name , filepath_data , filepath_txt  from whmobileprnt_ctl a , whmobileprnt_default b where a.series_no = b.series_no and b.mc_code= :p_mc_code";
                PrinterDataView printer = ctx.Database.SqlQuery<PrinterDataView>(sqlp, new OracleParameter("p_mc_code", vuser_id)).SingleOrDefault();

                if (printer == null)
                {
                    throw new Exception("ยังไม่ได้กำหนด Default Printer");
                }

                string dateFormat = "yyyyMM";
                int running = 4;
                DateTime dateNow = DateTime.Now;
                string nextDocId = "";
                string RunningNo = "";
                string preFix = string.Format("{0:" + dateFormat + "}", dateNow);
                string formatRuning = "00000000000000000000000";
                formatRuning = formatRuning.Substring(1, running);

                string sqld = "select substr(max(pkg_barcode_set),7,4) from pkg_barcode where entity = :p_entity and pkg_barcode_set like :p_preFix and trunc(fin_date) = trunc(sysdate)";
                string setRunning = ctx.Database.SqlQuery<string>(sqld, new OracleParameter("p_entity", ventity), new OracleParameter("p_preFix", preFix + "%")).SingleOrDefault();

                if (setRunning == null)
                {
                    int no = 1;

                    RunningNo = no.ToString(formatRuning);
                    nextDocId = preFix + RunningNo;
                }
                else
                {

                    int no = Int32.Parse(setRunning) + 1;

                    RunningNo = no.ToString(formatRuning);
                    nextDocId = preFix + RunningNo;
                }

                vset_no = nextDocId;

                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    // Udpaet PKG_BARCODE
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_set_no", vset_no),
                        new OracleParameter("p_entity", ventity),
                        new OracleParameter("p_prod_code", vprod_code),
                        new OracleParameter("p_wc_code", vwc_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update pkg_barcode set pkg_barcode_set=:p_set_no , fin_date = SYSDATE where entity = :p_entity and prod_code = :p_prod_code and wc_code =:p_wc_code and pkg_barcode_set is null";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();

                }

                    string txtPath = @printer.filepath_txt;
                string dataPath = @printer.filepath_data;


                Console.WriteLine("Data File", printer.filepath_data);
                Console.WriteLine("Data File", printer.filepath_txt);


                string txt = vprod_code + "@" + "@" + vbar_code + "@QTY@" + vqty + "@" + vmfg_date + "@" +  vset_no + "@" + vprod_name + "@" + vbar_code + "|" + vprod_name + "|" + model.set_no + "|" + vqty + "|" + vreq_date + "|" + vjob_no;


                //File.WriteAllText(Path.Combine(dataPath), all_txt);
                //File.WriteAllText(Path.Combine(txtPath), "");
                File.WriteAllText(dataPath, txt);
                File.WriteAllText(txtPath, "");
            }
        }

        public void RePringSticker(PringSetNoView model)
        {
            System.Diagnostics.Process.Start("net.exe", @"use L: / delete");
            System.Diagnostics.Process.Start("net.exe", @"use L: \\192.168.8.14\Data TOP@007* /USER:192.168.8.14\webadmin").WaitForExit();

            using (var ctx = new ConXContext())
            {
                var vuser_id = model.user_id;
                var vreq_date = model.req_date;
                var vmfg_date = "";
                var vqty = model.scan_qty;
                //var vset_no = "Set No." + model.set_no;
                var vset_no = model.set_no;
                //var vjob_no = model.job_no;
                var vwc_code = model.wc_code;
                var ventity = model.entity;


                string sqlp = "select  a.prnt_point_name printer_name , filepath_data , filepath_txt  from whmobileprnt_ctl a , whmobileprnt_default b where a.series_no = b.series_no and b.mc_code= :p_mc_code";
                PrinterDataView printer = ctx.Database.SqlQuery<PrinterDataView>(sqlp, new OracleParameter("p_mc_code", vuser_id)).SingleOrDefault();

                if (printer == null)
                {
                    throw new Exception("ยังไม่ได้กำหนด Default Printer");
                }


                string sql = "select a.prod_code , a.bar_code , b.prod_tname prod_name , a.por_no job_no , to_char(fin_date,'dd/mm/yyyy') fin_date from pkg_barcode a , product b where a.prod_code=b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and tran_date = to_date(:p_tran_date,'dd/mm/yyyy') and pkg_barcode_set = :p_set_no and rownum = 1";
                ScanSendView prod = ctx.Database.SqlQuery<ScanSendView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_tran_date", vreq_date), new OracleParameter("p_set_no", vset_no)).SingleOrDefault();

                string txtPath = @printer.filepath_txt;
                string dataPath = @printer.filepath_data;


                Console.WriteLine("Data File", printer.filepath_data);
                Console.WriteLine("Data File", printer.filepath_txt);

                vmfg_date = "MFG Date " + prod.fin_date;

                string txt = prod.prod_code + "@" + "@" + prod.bar_code + "@QTY@" + vqty + "@" + vmfg_date + "@" + vset_no + "@" + prod.prod_name + "@" + prod.bar_code + "|" + prod.prod_name + "|" + model.set_no + "|" + vqty + "|" + vreq_date + "|" + prod.job_no;


                //File.WriteAllText(Path.Combine(dataPath), all_txt);
                //File.WriteAllText(Path.Combine(txtPath), "");
                File.WriteAllText(dataPath, txt);
                File.WriteAllText(txtPath, "");
            }
        }

        public ScanSendView ScanSendAdd(ScanSendSearchView model)
        {

            using (var ctx = new ConXContext())
            {
                var ventity = model.entity;
                var vreq_date = model.req_date;
                var vuser_id = model.user_id;
                var vwc_code = model.wc_code;
                var vbuild_type = model.build_type;
                var vpcs_barcode = model.pcs_barcode;
                var vset_no = "";

              
                
                if(vpcs_barcode.Length < 11)
                {
                    String[] strlist = model.req_date.Split('/');
                    int running = 5;
                   
                    string RunningNo = "";
                    string preFix = strlist[0]+ strlist[1]+ strlist[2].Substring(2,2);
                    string formatRuning = "00000000000000000000000";
                    formatRuning = formatRuning.Substring(1, running);

                    int psc_no = int.Parse(vpcs_barcode);
                    RunningNo = psc_no.ToString(formatRuning);
                    vpcs_barcode = preFix + RunningNo;

                }

                string sqlw = "select wc_prev from PD_WCCTL_SEQ  where pd_entity = :p_entity and wc_code = :p_wc_code";
                string vprev_wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code))
                            .FirstOrDefault();

                string sqlc = "select mps_st from mps_det_wc";
                sqlc += " where entity = :p_entity";
                sqlc += " and req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                sqlc += " and pcs_barcode = :p_pcs_barcode";
                sqlc += " and wc_code = :p_prev_wc";
                string mps_st = ctx.Database.SqlQuery<string>(sqlc, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pcs_barcode", vpcs_barcode), new OracleParameter("p_prev_wc", vprev_wc)).FirstOrDefault();

                if (mps_st == "N" || mps_st == null)
                {
                    throw new Exception("ยังไม่ได้ Scan หน่วยก่อนหน้านี้ / PCS Barcode ไม่ถูกต้อง");
                }



                string sql = "select req_date ,  prod_code , prod_name , pcs_barcode , por_no job_no , bar_code ";
                sql += " from mps_det_wc";
                sql += " where entity = :p_entity";
                sql += " and req_date = to_date(:p_req_date,'dd/mm/yyyy')";
                sql += " and pcs_barcode = :p_pcs_barcode";
                sql += " and wc_code = :p_wc_code";

                ScanSendView prod = ctx.Database.SqlQuery<ScanSendView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_pcs_barcode", vpcs_barcode), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault();

                if (prod == null)
                {
                    throw new Exception("PCS Barcode ไม่ถูกต้อง");
                }

                string sqlp = "select nvl(pcs_per_pack,0) from product  where prod_code = :p_prod_code";
                int vset_qty = ctx.Database.SqlQuery<int>(sqlp, new OracleParameter("p_prod_code", prod.prod_code)).FirstOrDefault();

                string sqli = "select count(*)+1 from pkg_barcode where pkg_barcode_set is null and prod_code=:p_prod_code and wc_code=:p_wc_code";
                int vitem = ctx.Database.SqlQuery<int>(sqli, new OracleParameter("p_prod_code", prod.prod_code), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault();

                string sqlst = "select tran_no from pkg_barcode where entity = :p_entity and tran_no = :p_tran_no and wc_code = :p_wc_code";
                string chkdup = ctx.Database.SqlQuery<string>(sqlst, new OracleParameter("p_entity", ventity), new OracleParameter("p_pcs_barcode", vpcs_barcode), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault();

                if(chkdup != null)
                {
                    throw new Exception("บันทึก PCS Barcode ซ้ำ");
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    // Udpaet MOS_DET_WC
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", ventity),
                        new OracleParameter("p_fin_by", vuser_id),
                        new OracleParameter("p_pcs_barcode", vpcs_barcode),
                        new OracleParameter("p_wc_code", vwc_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc set mps_st='Y' , fin_by =:p_fin_by , fin_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                    oraCommand.ExecuteNonQuery();

                    // Insert Into PKG_BARCODE
                    OracleCommand oraCommand_barcode = conn.CreateCommand();
                    OracleParameter[] param_barcode = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", ventity),
                        new OracleParameter("p_tran_no", vpcs_barcode),
                        new OracleParameter("p_tran_date", vreq_date),
                        new OracleParameter("p_wc_code", vwc_code),
                        new OracleParameter("p_por_no", prod.job_no),
                        new OracleParameter("p_prod_code", prod.prod_code),
                        new OracleParameter("p_bar_code", prod.bar_code),
                        new OracleParameter("p_pkg_item_no", vitem),
                        new OracleParameter("p_build_type", vbuild_type)
                    };

                    oraCommand_barcode.BindByName = true;
                    oraCommand_barcode.Parameters.AddRange(param_barcode);
                    oraCommand_barcode.CommandText = "insert into pkg_barcode (entity , tran_no , tran_date , wc_code , por_no , prod_code , bar_code , pkg_item_no , pkg_barcode_set , prt_flag , fin_date , build_type) values (:p_entity , :p_tran_no , to_date(:p_tran_date,'dd/mm/yyyy') , :p_wc_code , :p_por_no , :p_prod_code , :p_bar_code , :p_pkg_item_no , '' , 'N' , sysdate , :p_build_type)";


                    oraCommand_barcode.ExecuteNonQuery();

                    conn.Close();


                    scope.Complete();


                }

                string sqls = "select count(*) from pkg_barcode where pkg_barcode_set is null and prod_code=:p_prod_code and wc_code=:p_wc_code";
                int vscan = ctx.Database.SqlQuery<int>(sqls, new OracleParameter("p_prod_code", prod.prod_code), new OracleParameter("p_wc_code", vwc_code)).FirstOrDefault();


                //define model view
                ScanSendView view = new ModelViews.ScanSendView()
                {
                    entity = ventity,
                    pcs_barcode = prod.pcs_barcode,
                    prod_code = prod.prod_code,
                    prod_name = prod.prod_name,
                    bar_code = prod.bar_code,
                    job_no = prod.job_no,
                    req_date = prod.req_date,
                    wc_code = vwc_code,
                    set_no = vset_no,
                    set_qty = vset_qty,
                    scan_qty = vscan,
                    user_id = vuser_id,
                    fin_date = DateTime.Now.ToString("dd/MM/yyyy")
                };


                //return data to contoller
                return view;

            }
        }
    }
}