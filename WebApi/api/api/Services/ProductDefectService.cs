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
    public class ProductDefectService : IProductDefectService
    {
        public void DataQcCutting(DataQcCuttingView model)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    string sql = "select doc_no from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_por_no and item_no = :p_item_no";
                    string chk = ctx.Database.SqlQuery<string>(sql, new OracleParameter("p_entity", model.entity), new OracleParameter("p_doc_no", model.por_no), new OracleParameter("p_doc_no", model.item_no)).FirstOrDefault();



                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    if(chk != null)
                    {
                        // Delete PD_QC_DET
                        OracleCommand oraCommanddel1 = conn.CreateCommand();
                        OracleParameter[] paramdel1 = new OracleParameter[]
                        {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),
                       
                        };
                        oraCommanddel1.BindByName = true;
                        oraCommanddel1.Parameters.AddRange(paramdel1);
                        oraCommanddel1.CommandText = "delete pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and item_no = :p_item_no";

                        oraCommanddel1.ExecuteNonQuery();

                        // Delete PD_QC_DET
                        OracleCommand oraCommanddel2 = conn.CreateCommand();
                        OracleParameter[] paramdel2 = new OracleParameter[]
                        {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),

                        };
                        oraCommanddel2.BindByName = true;
                        oraCommanddel2.Parameters.AddRange(paramdel2);
                        oraCommanddel2.CommandText = "delete pd_qc_det where pd_entity = :p_entity and doc_no = :p_doc_no and item_no = :p_item_no";

                        oraCommanddel2.ExecuteNonQuery();
                    }


                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),
                        new OracleParameter("p_build_type", model.build_type),
                        new OracleParameter("p_prod_code", model.prod_code),
                        new OracleParameter("p_qc_process", model.qc_process),
                        new OracleParameter("p_ref_no", model.ref_no),
                        //new OracleParameter("p_qc_date", model.qc_date),
                        new OracleParameter("p_qc_qty", model.qc_qty),
                        new OracleParameter("p_no_pass_qty", model.no_pass_qty),
                        new OracleParameter("p_remark1", model.remark1),
                        new OracleParameter("p_remark2", model.remark2),
                        new OracleParameter("p_remark3", model.remark3),
                        new OracleParameter("p_upd_by", model.user_id),
                        new OracleParameter("p_cre_by", model.user_id)
                    };
                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "insert into pd_qc_mast (pd_entity , doc_no , item_no , prod_code, build_type , qc_process , ref_por_no , qc_date , qc_qty , no_pass_qty , remark1 , remark2 , remark3 , upd_by , upd_date , cre_by , cre_date) values (:p_entity , :p_doc_no , :p_item_no , :p_prod_code, :p_build_type , :p_qc_process , :p_ref_no , sysdate , :p_qc_qty , :p_no_pass_qty , :p_remark1 , :p_remark2 , :p_remark3 , :p_upd_by , sysdate , :p_cre_by , sysdate)";

                    oraCommand.ExecuteNonQuery();


                    OracleCommand oraCommanddet1 = conn.CreateCommand();
                    OracleParameter[] paramdet1 = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),
                        new OracleParameter("p_qc_value", model.width),
                        
                    };
                    oraCommanddet1.BindByName = true;
                    oraCommanddet1.Parameters.AddRange(paramdet1);
                    oraCommanddet1.CommandText = "insert into pd_qc_det (pd_entity , doc_no , item_no , item_no_det, item_desc , qc_value ) values (:p_entity , :p_doc_no , :p_item_no , 1, 'กว้าง' , :p_qc_value )";

                    oraCommanddet1.ExecuteNonQuery();

                    OracleCommand oraCommanddet2 = conn.CreateCommand();
                    OracleParameter[] paramdet2 = new OracleParameter[]
                    {
                        new OracleParameter("p_entity", model.entity),
                        new OracleParameter("p_doc_no", model.por_no),
                        new OracleParameter("p_item_no", model.item_no),
                        new OracleParameter("p_qc_value", model.lenght),

                    };
                    oraCommanddet2.BindByName = true;
                    oraCommanddet2.Parameters.AddRange(paramdet2);
                    oraCommanddet2.CommandText = "insert into pd_qc_det (pd_entity , doc_no , item_no , item_no_det, item_desc , qc_value ) values (:p_entity , :p_doc_no , :p_item_no , 2, 'ยาว' , :p_qc_value )";

                    oraCommanddet2.ExecuteNonQuery();


                    conn.Close();


                    scope.Complete();


                }
            }
        }

        

        public int GetItemNo(string entity, string por_no)
        {
            int seq;

            using (var ctx = new ConXContext())
            {
                string sql = "select nvl(max(item_no),0)+1 from pd_qc_mast where pd_entity= :p_entity and doc_no=:p_por_no and qc_process='CUTTING'";
                seq = ctx.Database.SqlQuery<int>(sql, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", por_no)).FirstOrDefault();
            }

            return seq;
        }

       
        public ItemNoModalView GetItemNoList(string entity, string por_no)
        {
            using (var ctx = new ConXContext())
            {
                ItemNoModalView view = new ModelViews.ItemNoModalView()
                {

                    datas = new List<ModelViews.ItemNoView>()
                };

                string sql1 = "select pd_entity entity , doc_no por_no, item_no , prod_code, build_type , qc_process , ref_por_no ref_no , to_char(qc_date,'dd/mm/yyyy') qc_date , to_char(qc_date,'hh24:mi') qc_time , qc_qty , no_pass_qty , remark1 , remark2 , remark3 from pd_qc_mast where pd_entity= :p_entity and doc_no=:p_por_no and qc_process='CUTTING' order  by item_no";
                List<ItemNoView> seq = ctx.Database.SqlQuery<ItemNoView>(sql1, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", por_no)).ToList();

                
                foreach (var i in seq)
                {
                    string sql2 = "select to_char(qc_value) from pd_qc_det where  pd_entity= :p_entity and doc_no=:p_por_no and item_no = :p_item_no and item_no_det=1";
                    string vwidth = ctx.Database.SqlQuery<string>(sql2, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", por_no),new OracleParameter("p_item_no", i.item_no)).FirstOrDefault();

                    string sql3 = "select to_char(qc_value) from pd_qc_det where  pd_entity= :p_entity and doc_no=:p_por_no and item_no = :p_item_no and item_no_det=2";
                    string vlenght = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_entity", entity), new OracleParameter("p_doc_no", por_no), new OracleParameter("p_item_no", i.item_no)).FirstOrDefault();

                    string sql4 = "select pdsize_desc from product where prod_code = :p_prod_code";
                    string vsize_name = ctx.Database.SqlQuery<string>(sql4, new OracleParameter("p_prod_code", i.prod_code)).FirstOrDefault();

                    view.datas.Add(new ModelViews.ItemNoView()
                    {

                        entity = i.entity,
                        por_no = i.por_no,
                        ref_no = i.ref_no,
                        prod_code = i.prod_code,
                        build_type = i.build_type,
                        qc_date = i.qc_date,
                        qc_process = i.qc_process,
                        item_no = i.item_no,
                        qc_qty = i.qc_qty,
                        no_pass_qty = i.no_pass_qty,
                        width = vwidth,
                        lenght = vlenght,
                        remark1 = i.remark1,
                        remark2 = i.remark2,
                        remark3 = i.remark3,
                        size_name = vsize_name,
                        qc_time = i.qc_time

                    });
                }


                return view;

                
            }
        }

        public DataQcEnrtyView GetQCEntryData(string build_type)
        {
            throw new NotImplementedException();
        }

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

                    string sql1 = "select a.entity , a.por_no , nvl(a.ref_no,a.por_no) ref_no , a.prod_code , max(a.prod_tname) prod_name , max(b.pdbrnd_tname) brand_name , max(a.model_name) design_name , max(a.size_name) size_name , sum(qty_req) qty_pdt " +
                        "from mps_det a , pdbrnd_mast b " +
                        "where a.pdbrnd_code = b.pdbrnd_code " +
                        "and a.entity = :p_entity " +
                        "and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy') " +
                        "and a.por_no like :p_doc_no " +
                        "and (a.build_type is null or a.build_type = 'HMJIT') " +
                        "group by  a.entity , a.por_no , a.ref_no , a.prod_code " +
                        "order by a.por_no , a.ref_no";

                    List<ProductDefectDetailView> prod = ctx.Database.SqlQuery<ProductDefectDetailView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_doc_no", vpor_no + "%")).ToList();

                    view.totalItem = prod.Count;
                    prod = prod.Skip(view.pageIndex * view.itemPerPage)
                        .Take(view.itemPerPage)
                        .ToList();

                    foreach (var x in prod)
                    {

                        string sql2 = "select nvl(sum(qc_qty),0) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and ref_por_no = :p_ref_no and prod_code = :p_prod_code and qc_process='CUTTING'";
                        int vqty_cutting = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();

                        string sql3 = "select nvl(sum(qc_qty),0) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and ref_por_no = :p_ref_no and prod_code = :p_prod_code and qc_process='WIP'";
                        int vqty_wip = ctx.Database.SqlQuery<int>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();

                        string sql4 = "select nvl(sum(qc_qty),0) from pd_qc_mast where pd_entity = :p_entity and doc_no = :p_doc_no and ref_por_no = :p_ref_no and prod_code = :p_prod_code and qc_process='FG'";
                        int vqty_fin = ctx.Database.SqlQuery<int>(sql4, new OracleParameter("p_entity", ventity), new OracleParameter("p_doc_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();


                        view.datas.Add(new ModelViews.ProductDefectDetailView()
                        {
                            entity = ventity,
                            por_no = x.por_no,
                            ref_no = x.ref_no,
                            prod_code = x.prod_code,
                            prod_name = x.prod_name,
                            brand_name = x.brand_name,
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