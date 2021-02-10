using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace api.Services
{
    public class SpecDrawingService : ISpecDrawingService
    {
        public SpecDrawingView GetSpecInfo(string barcode , string dsgn_no)
        {
            //System.Diagnostics.Process.Start("net.exe", @"use N: / delete");
            //System.Diagnostics.Process.Start("net.exe", @"use \\192.168.8.20\DataCenter TOP@007* /USER:128.1.1.23\webadmin").WaitForExit();

            string type = "";
            string file_path = "";
            //string file_paths = "";
            //string vfile_name = "";

            string urlPrefix_sd = ConfigurationManager.AppSettings["upload.urlPrefixSd"];
            string urlPrefix_spec = ConfigurationManager.AppSettings["upload.urlPrefixSpec"];

            string urlPrefix = "";

            using (var ctx = new ConXContext())
            {
                
                string sql = "select prod_code , prod_tname prod_name , bar_code , bom_code sd_no , packaging_no design_no , pddsgn_code design_code";
                sql += " from product";
                sql += " where bar_code = :p_barcode";


                SpecDrawingView datas = ctx.Database.SqlQuery<SpecDrawingView>(sql, new OracleParameter("p_barcode", barcode)).SingleOrDefault();

                string file = "";

                if (datas == null)
                {
                    throw new Exception("ไม่มีข้อมูลสินค้านี้");
                }


                if (datas.sd_no == null)
                {
                    
                    type = "Design";
                    //file = "*" + datas.design_code + ".pdf";
                    file = datas.design_code + "_" + dsgn_no + ".pdf";
                    urlPrefix = urlPrefix_spec;
                }
                else
                {
                    
                    type = "SD";
                    //file = "*" + datas.sd_no + ".pdf";
                    file = datas.design_code + "_" + dsgn_no + ".pdf";
                    urlPrefix = urlPrefix_sd;
                }


            


                //define model view
                SpecDrawingView view = new ModelViews.SpecDrawingView()
                {
                    prod_code = datas.prod_code,
                    prod_name = datas.prod_name,
                    bar_code = datas.bar_code,
                    design_code = datas.design_code,
                    type = type,
                    sd_no = datas.sd_no,
                    design_no = datas.design_no,
                    file_path = file_path,
                    //file_name = vfile_name
                    file_name = urlPrefix + file
                };

                

                //return data to contoller
                return view;

            }
        }

        
        public SpecDrawingAllView GetSpecInfoBarcode(string barcode)
        {
            string type = "";
            //string file_path = "";
            //string file_paths = "";
            //string vfile_name = "";

            string urlPrefix_sd = ConfigurationManager.AppSettings["upload.urlPrefixSd"];
            string urlPrefix_spec = ConfigurationManager.AppSettings["upload.urlPrefixSpec"];

            string urlPrefix = "";

            using (var ctx = new ConXContext())
            {

                string sql = "select prod_code , prod_tname prod_name , bar_code , bom_code sd_no , packaging_no design_no , pddsgn_code design_code";
                sql += " from product";
                sql += " where bar_code = :p_barcode";

                SpecDrawingView prod = ctx.Database.SqlQuery<SpecDrawingView>(sql, new OracleParameter("p_barcode", barcode)).SingleOrDefault();


                if (prod.sd_no == null)
                {

                    type = "Design";
                    //file = "*" + datas.design_code + ".pdf";
       
                    urlPrefix = urlPrefix_spec;
                }
                else
                {

                    type = "SD";
                    //file = "*" + datas.sd_no + ".pdf";
                  
                    urlPrefix = urlPrefix_sd;
                }

                SpecDrawingAllView view = new ModelViews.SpecDrawingAllView()
                {
                    prod_code = prod.prod_code,
                    prod_name = prod.prod_name,
                    bar_code = prod.bar_code,
                    type = type,
                    sd_no = prod.sd_no,
                    design_no = prod.design_no,
                    datas = new List<ModelViews.SpecDrawinDetailView>()
                };

                string sqld = "select a.pddsgn_code design_code , a.dsgn_no , a.dept_code , a.file_name , b.pddsgn_tname design_name from files_ctl a , pddsgn_mast b where a.pddsgn_code = b.pddsgn_code and a.pddsgn_code = :p_pddsgn_code";
                List<SpecDrawinDetailView> files = ctx.Database.SqlQuery<SpecDrawinDetailView>(sqld, new OracleParameter("p_pddsgn_code", prod.design_code)).ToList();

                foreach (var i in files)
                {
                    view.datas.Add(new ModelViews.SpecDrawinDetailView()
                    {

                        design_code = i.design_code,
                        design_name = i.design_name,
                        dsgn_no = i.dsgn_no,
                        dept_code = i.dept_code,
                        file_name = urlPrefix + i.file_name

                    });
                }
                    //SpecDrawingView datas = ctx.Database.SqlQuery<SpecDrawingView>(sql, new OracleParameter("p_barcode", barcode)).SingleOrDefault();

                    //string file = "";

                    //if (datas == null)
                    //{
                    //    throw new Exception("ไม่มีข้อมูลสินค้านี้");
                    //}


                    //if (datas.sd_no == null)
                    //{

                    //    type = "Design";
                    //    //file = "*" + datas.design_code + ".pdf";
                    //    file = datas.design_code + ".pdf";
                    //    urlPrefix = urlPrefix_spec;
                    //}
                    //else
                    //{

                    //    type = "SD";
                    //    //file = "*" + datas.sd_no + ".pdf";
                    //    file = datas.design_code + ".pdf";
                    //    urlPrefix = urlPrefix_sd;
                    //}





                    ////define model view
                    //SpecDrawingView view = new ModelViews.SpecDrawingView()
                    //{
                    //    prod_code = datas.prod_code,
                    //    prod_name = datas.prod_name,
                    //    bar_code = datas.bar_code,
                    //    design_code = datas.design_code,
                    //    type = type,
                    //    sd_no = datas.sd_no,
                    //    design_no = datas.design_no,
                    //    file_path = file_path,
                    //    //file_name = vfile_name
                    //    file_name = urlPrefix + file
                    //};



                    //return data to contoller
                    return view;
            }
        }
    }
}