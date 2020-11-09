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
    public class ProductDefectService : IProductDefectService
    {
        public ProductDefectView SearchDataProductDefect(ProductDefectSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vbuild_type = model.build_type;
                string vpor_no = model.por_no;
                string vreq_date = model.req_date;
                string vuser_id = model.user_id;
                int total_qty_pdt = 0;
                int total_qty_cutting = 0;
                int total_qty_wip = 0;
                int total_qty_fin = 0;



                //define model view
                ProductDefectView view = new ModelViews.ProductDefectView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    entity = ventity,
                    build_type = vbuild_type,
                    total_qty_pdt = 0,
                    total_qty_cutting = 0,
                    total_qty_wip = 0,
                    total_qty_fin = 0,
                    datas = new List<ModelViews.ProductDefectDetailView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {

                    string sql1 = "select a.entity , a.por_no , nvl(a.ref_no,a.por_no) a.ref_no , a.prod_code , max(a.prod_name) prod_name , max(b.pdbrnd_tname) brand_name , max(a.model_name) deisdn_name , max(a.size_name) size_name , sum(qty_req) qty_pdt " +
                        "from mps_det a , pdbrnd_mast b " +
                        "where a.pdbrnd_code = b.pdbrnd_code " +
                        "and a.entity = :p_entity " +
                        "and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy') " +
                        "and a.por_no like :p_doc_no " +
                        "and (a.build_qty is mull or a.build_type = 'HMJIT'" +
                        "group by  a.entity , a.por_no , a.ref_no , a.prod_code order by a.por_no , a.ref_no";

                    List<ProductDefectDetailView> prod = ctx.Database.SqlQuery<ProductDefectDetailView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_date", vreq_date), new OracleParameter("p_doc_no", vpor_no + "%")).ToList();

                    view.totalItem = prod.Count;
                    prod = prod.Skip(view.pageIndex * view.itemPerPage)
                        .Take(view.itemPerPage)
                        .ToList();

                    foreach (var x in prod)
                    {

                        string sql2 = "select sum(qc_pdt) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no , ref_por_no = :p_ref_no and qc_process='CUTTING'";
                        int vqty_cutting = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no)).FirstOrDefault();

                        string sql3 = "select sum(qc_pdt) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no , ref_por_no = :p_ref_no and qc_process='WIP'";
                        int vqty_wip = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no)).FirstOrDefault();

                        string sql4 = "select sum(qc_pdt) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no , ref_por_no = :p_ref_no and qc_process='FG'";
                        int vqty_fin = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no)).FirstOrDefault();


                        view.datas.Add(new ModelViews.ProductDefectDetailView()
                        {
                            entity = ventity,
                            por_no = x.por_no,
                            ref_no = x.ref_no,
                            prod_code = x.prod_code,
                            prod_name = x.prod_name,
                            design_name = x.design_name,
                            size_name = x.size_name,
                            qty_pdt = x.qty_pdt,
                            qty_cutting = vqty_cutting,
                            qty_wip = vqty_wip,
                            qty_fin = vqty_fin

                        });

                        total_qty_pdt = total_qty_pdt + x.qty_pdt;
                        total_qty_cutting = total_qty_cutting + vqty_cutting;
                        total_qty_wip = total_qty_wip + vqty_wip;
                        total_qty_fin = total_qty_fin + vqty_fin;
                    }

                    view.total_qty_pdt = total_qty_pdt;
                    view.total_qty_cutting = total_qty_cutting;
                    view.total_qty_wip = total_qty_wip;
                    view.total_qty_fin = total_qty_fin;


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