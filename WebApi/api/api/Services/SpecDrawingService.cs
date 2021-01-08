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
        public SpecDrawingView GetSpecInfo(string barcode)
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


                //string sqls = "select spring_pic_file from pdspring_mast where springtype_code = :p_springtype_code";
                //string spring_file = ctx.Database.SqlQuery<string>(sqls, new OracleParameter("p_springtype_code", datas.springtype_code)).SingleOrDefault();

                if (datas.sd_no == null)
                {
                    string sqlp = "select hm_design_path from bm_basic_mast";
                    file_path = ctx.Database.SqlQuery<string>(sqlp).SingleOrDefault();
                    type = "Design";
                    //file = "*" + datas.design_code + ".pdf";
                    file = datas.design_code + ".pdf";
                    urlPrefix = urlPrefix_spec;
                }
                else
                {
                    string sqlp = "select hm_sd_path from bm_basic_mast";
                    file_path = ctx.Database.SqlQuery<string>(sqlp).SingleOrDefault();
                    type = "SD";
                    //file = "*" + datas.sd_no + ".pdf";
                    file = datas.design_code + ".pdf";
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
    }
}