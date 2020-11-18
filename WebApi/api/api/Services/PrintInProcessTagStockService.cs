using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace api.Services
{
    public class PrintInProcessTagStockService : IPrintInProcessTagStockService
    {
        public TagStockGroupModalView getGroup(TagStockProductSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;


                TagStockGroupModalView view = new ModelViews.TagStockGroupModalView()
                {

                    datas = new List<ModelViews.TagStockGroupView>()
                };

                string sql = "select distinct d.disgrp_line_code group_line , d.disgrp_line_desc group_line_name , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = b.bom_code and b.distype_code = c.distype_code and c.entity = d.entity and c.disgrp_line_code = d.disgrp_line_code and a.entity = :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no = :p_por_no and a.ref_no = :p_ref_no and a.wc_code = :p_wc_code order by d.disgrp_sortid";
                List<TagStockGroupView> group = ctx.Database.SqlQuery<TagStockGroupView>(sql, new Oracle.ManagedDataAccess.Client.OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", vwc_code)).ToList();

                foreach (var i in group)
                {

                    view.datas.Add(new ModelViews.TagStockGroupView()
                    {

                        group_line = i.group_line,
                        group_line_name = i.group_line_name,
                       

                    });
                }


                return view;

            }
        }

        public TagStockProductModalView getProduct(TagStockProductSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vreq_date = model.req_date;
                string vwc_code = model.wc_code;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;
                string vgroup_line = model.group_line;



                TagStockProductModalView view = new ModelViews.TagStockProductModalView()
                {

                    datas = new List<ModelViews.TagStockProductView>()
                };

                string sql = "select distinct a.build_no, a.prod_code , a.prod_code_sub sub_prod_code , a.prod_name_sub sub_prod_name , a.por_no , a.ref_no from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c where a.prod_code_sub=b.bom_code and b.distype_code=c.distype_code and a.entity=c.entity and  a.entity=:p_entity and a.req_date = to_date(:req_date,'dd/mm/yyyy') and a.wc_code = :p_wc_code and a.por_no=:p_por_no and a.ref_no=:p_ref_no  and c.disgrp_line_code = :p_group_line order by a.prod_code , a.prod_code_sub";
                List<TagStockProductView> prod = ctx.Database.SqlQuery<TagStockProductView>(sql, new Oracle.ManagedDataAccess.Client.OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_group_line", vgroup_line)).ToList();

                foreach (var i in prod)
                {

                    string sql1 = "select pddsgn_desc design_name , pdsize_desc size_name from product where prod_code = :prod_code";
                    ProductDataView product = ctx.Database.SqlQuery<ProductDataView>(sql1, new Oracle.ManagedDataAccess.Client.OracleParameter("p_prod_code", i.prod_code)).FirstOrDefault();

                    view.datas.Add(new ModelViews.TagStockProductView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_name = i.sub_prod_name,
                        sub_prod_code = i.sub_prod_code,
                        size_name = product.size_name,
                        design_name = product.design_name,
                        por_no = i.por_no,
                        ref_no = i.ref_no,
                        req_date = vreq_date,
                        build_no = i.build_no

                    });
                }


                return view;

            }
        }

        public void PringTagStock(PrintInProcessTagStockView model)
        {
            System.Diagnostics.Process.Start("net.exe", @"use L: / delete");
            System.Diagnostics.Process.Start("net.exe", @"use L: \\192.168.8.14\Data TOP@007* /USER:192.168.8.14\webadmin").WaitForExit();

            using (var ctx = new ConXContext())
            {


                var vqty = model.qty;
                var vuser_id = model.user_id;
                var vprod_code = model.prod_code;
                var vsub_prod_code = model.sub_prod_code;
                var vsub_prod_name = model.sub_prod_name;
                var vsize = model.size_name;
                var vdesign = model.design_name;
                var vdescription = model.description;
                var vreq_date = model.req_date;
                var vpor_no = model.por_no;
                var vref_no = model.ref_no;
                var vbuild_no = model.build_no;



                string sqlp = "select  a.prnt_point_name printer_name , filepath_data , filepath_txt  from whmobileprnt_ctl a , whmobileprnt_default b where a.series_no = b.series_no and b.mc_code= :p_mc_code";
                PrinterDataView printer = ctx.Database.SqlQuery<PrinterDataView>(sqlp, new OracleParameter("p_mc_code", vuser_id)).SingleOrDefault();

                if (printer == null)
                {
                    throw new Exception("บังไม่ได้กำหนด Default Printer");
                }

                string sqlf = "select form_no from doc_mast where systemid='PD' and doc_code='IPT'";
                string vform_no = ctx.Database.SqlQuery<string>(sqlf).FirstOrDefault();


                string txtPath = @printer.filepath_txt;
                string dataPath = @printer.filepath_data;


                Console.WriteLine("Data File", printer.filepath_data);
                Console.WriteLine("Data File", printer.filepath_txt);


                string txt = vsub_prod_name + "@" + vsize + "@" + vdesign + "@" + vdescription + "@" + vpor_no + "@" + vqty + "@" + vform_no + "@" + vprod_code + "|"+ vsub_prod_code + "|" + vpor_no + "|" + vref_no + "|" + "" + vqty + "|" + vreq_date + "|" + vbuild_no;


                File.WriteAllText(dataPath, txt);
                File.WriteAllText(txtPath, "");



                //System.Diagnostics.Process.Start("net.exe", @"use L: / delete");


            }
        }
    }
}