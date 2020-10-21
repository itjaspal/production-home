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
        public SetNoView getSetNo(SetNoSearchView model)
        {
            throw new NotImplementedException();
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


                string sqlp = "select  a.prnt_point_name printer_name , filepath_data , filepath_txt  from whmobileprnt_ctl a , whmobileprnt_default b where a.series_no = b.series_no and b.mc_code= :p_mc_code";
                PrinterDataView printer = ctx.Database.SqlQuery<PrinterDataView>(sqlp, new OracleParameter("p_mc_code", vuser_id)).SingleOrDefault();

                if (printer == null)
                {
                    throw new Exception("บังไม่ได้กำหนด Default Printer");
                }

                
                string txtPath = @printer.filepath_txt;
                string dataPath = @printer.filepath_data;


                Console.WriteLine("Data File", printer.filepath_data);
                Console.WriteLine("Data File", printer.filepath_txt);


                string txt = vprod_code + "@" + "@" + vbar_code + "@QTY@" + vqty + "@" + vmfg_date + "@" +  vset_no + "@" + vprod_name + "@" + vbar_code + "|" + vprod_name + "|" + model.set_no + "" + vqty + "|" + vreq_date + "|" + vjob_no;


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
                var vpcs_barcode = model.pcs_barcode;
                var vset_no = "";

              
                string dateFormat = "yyyyMM";  
                int running = 4;
                DateTime dateNow = DateTime.Now;
                string nextDocId = "";
                string RunningNo = "";
                string preFix = string.Format("{0:" + dateFormat + "}", dateNow);
                string formatRuning = "00000000000000000000000";
                formatRuning = formatRuning.Substring(1, running);

                string sqld = "select substr(max(pkg_barcode_set),6,4) from pkg_barcode where entity = :p_entity and pkg_barcode_set like :p_preFix and fin_date = trunc(sysdate)";
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



                string sql = "select to_char(req_date,'dd/mm/yyyy') req_date ,  prod_code , prod_name , pcs_barcode , por_no job_no ";
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
                        new OracleParameter("p_fin_by", vuser_id),
                        new OracleParameter("p_pcs_barcode", vpcs_barcode),
                        new OracleParameter("p_wc_code", vwc_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update mps_det_wc set mps_st='Y' , fin_by =:p_fin_by , fin_date = SYSDATE where entity = :p_entity and pcs_barcode = :p_pcs_barcode and wc_code =:p_wc_code";


                    oraCommand.ExecuteNonQuery();

                    conn.Close();


                    scope.Complete();


                }

                //define model view
                ScanSendView view = new ModelViews.ScanSendView()
                {
                    pcs_barcode = prod.pcs_barcode,
                    prod_code = prod.prod_code,
                    prod_name = prod.prod_name,
                    job_no = prod.job_no,
                    req_date = prod.req_date,
                    set_no = vset_no,
                    set_qty = vset_qty,
                };

                //return data to contoller
                return view;

            }
        }
    }
}