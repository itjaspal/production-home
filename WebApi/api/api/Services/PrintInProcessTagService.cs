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
    public class PrintInProcessTagService : IPrintInProcessTagService
    {
        public TagProductModalView getProduct(TagProductSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                //string vwc_code = model.wc_code;
                string vpdjit_grp = model.pdjit_grp;


                TagProductModalView view = new ModelViews.TagProductModalView()
                {

                    datas = new List<ModelViews.TagProductView>()
                };

                string sql = "select distinct a.prod_code , a.prod_tname prod_name , b.bar_code , a.size_name , a.model_name design_name  from mps_det a , product b where a.prod_code=b.prod_code and  a.entity = :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.pdjit_grp = :p_pdjit_grp";
                List<TagProductView> prod = ctx.Database.SqlQuery<TagProductView>(sql, new Oracle.ManagedDataAccess.Client.OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date),  new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.TagProductView()
                    {

                        prod_code = i.prod_code,
                        prod_name = i.prod_name,
                        bar_code = i.bar_code,
                        size_name = i.size_name,
                        design_name = i.design_name

                    });
                }


                return view;

            }
        }

        public PrintInProcessTagView GetProductInfo(string code)
        {
            using (var ctx = new ConXContext())
            {
                string sql = "select prod_code , prod_tname prod_name, bar_code , pdsize_desc size_name , pddsgn_desc design_name   from product where bar_code = :p_bar_code";

                PrintInProcessTagView product = ctx.Database.SqlQuery<PrintInProcessTagView>(sql, new OracleParameter("p_bar_code", code)).SingleOrDefault();

                
                return new PrintInProcessTagView
                {
                    prod_code = product.prod_code,
                    prod_name = product.prod_name,
                    bar_code  = product.bar_code,
                    size_name = product.size_name,
                    design_name = product.design_name    
                };
                
            }
            
        }

        public void PringTag(PrintInProcessTagView model)
        {
            System.Diagnostics.Process.Start("net.exe", @"use L: / delete");
            System.Diagnostics.Process.Start("net.exe", @"use L: \\192.168.8.14\Data TOP@007* /USER:192.168.8.14\webadmin").WaitForExit();

            using (var ctx = new ConXContext())
            {

                
                var vqty = model.qty;
                var vuser_id = model.user_id;
                var vprod_code = model.prod_code;
                var vbar_code = model.bar_code;
                var vprod_name = model.prod_name;
                var vsize = model.size_name;
                var vdesign = model.design_name;
                var vdescription = model.description;
                var vreq_date = model.req_date;
                var vorder_no = "JIT " + model.req_date; 
        



                string sqlp = "select  a.prnt_point_name printer_name , filepath_data , filepath_txt  from whmobileprnt_ctl a , whmobileprnt_default b where a.series_no = b.series_no and b.mc_code= :p_mc_code";
                PrinterDataView printer = ctx.Database.SqlQuery<PrinterDataView>(sqlp, new OracleParameter("p_mc_code", vuser_id)).SingleOrDefault();

                if(printer == null)
                {
                    throw new Exception("บังไม่ได้กำหนด Default Printer");
                }

                string sqlf = "select form_no from doc_mast where systemid='PD' and doc_code='IPT'";
                string vform_no = ctx.Database.SqlQuery<string>(sqlf).FirstOrDefault();

               
                string txtPath = @printer.filepath_txt;
                string dataPath = @printer.filepath_data;


                Console.WriteLine("Data File", printer.filepath_data);
                Console.WriteLine("Data File", printer.filepath_txt);


                string txt = vprod_name + "@" + vsize + "@" + vdesign + "@" + vdescription + "@" + vorder_no + "@" + model.req_date + "@" + vqty + "@" + vform_no + "@" + vbar_code + "|" + vprod_name + "|" + "" + vqty + "|" + vreq_date;


                //File.WriteAllText(Path.Combine(dataPath), all_txt);
                //File.WriteAllText(Path.Combine(txtPath), "");
                File.WriteAllText(dataPath, txt);
                File.WriteAllText(txtPath, "");



                //System.Diagnostics.Process.Start("net.exe", @"use L: / delete");


            }
        }
    }
}