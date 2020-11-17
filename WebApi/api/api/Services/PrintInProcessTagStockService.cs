using api.DataAccess;
using api.Interfaces;
using api.ModelViews;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
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

                string sql = "select distinct d.disgrp_line_code , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = b.bom_code and b.distype_code = c.distype_code and c.entity = d.entity and c.disgrp_line_code = d.disgrp_line_code and entity = :p_entity and req_date = to_date(:p_req_date,'dd/mm/yyyy') and por_no = :p_por_no and ref_no = :p_ref_no and wc_code = :p_wc_code order by d.disgrp_sortid";
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
                //string vwc_code = model.wc_code;
               


                TagStockProductModalView view = new ModelViews.TagStockProductModalView()
                {

                    datas = new List<ModelViews.TagStockProductView>()
                };

                string sql = "select distinct a.prod_code , a.prod_tname prod_name , b.bar_code , a.size_name , a.model_name design_name  from mps_det a , product b where a.prod_code=b.prod_code and  a.entity = :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.pdjit_grp = :p_pdjit_grp";
                List<TagStockProductView> prod = ctx.Database.SqlQuery<TagStockProductView>(sql, new Oracle.ManagedDataAccess.Client.OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.TagStockProductView()
                    {

                        prod_code = i.prod_code,
                        sub_prod_name = i.sub_prod_name,
                        sub_prod_code = i.sub_prod_code,
                        size_name = i.size_name,
                        design_name = i.design_name

                    });
                }


                return view;

            }
        }
    }
}