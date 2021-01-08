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
    public class JobOperationService : IJobOperationService
    {
        public OrderView getOrderInfo(string entity, string por_no)
        {
            using (var ctx = new ConXContext())
            {

                string sql = "select pd_entity entity , por_no , por_date , req_date , cust_code , cust_name , ref_no  , dept , ord_type , por_type , cntry_code country , decode(por_priority,'H','High','M','Medium','L','Low','P','Pre-Schedule','U','Urgently') por_priority , wh_code , grp_code , por_remark from por_mast where pd_entity = :p_entity and por_no = :p_por_no";
                OrderView por = ctx.Database.SqlQuery<OrderView>(sql, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", por_no)).SingleOrDefault();

                string sql1 = "select entity_namet from entity where entity_code = :p_entity";
                string entity_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_entity", entity)).SingleOrDefault();

                string sql2 = "select dept_namet from department where dept_code = :p_dept";
                string dept_name = ctx.Database.SqlQuery<string>(sql2, new OracleParameter("p_dept", por.dept)).SingleOrDefault();

                string sql3 = "select ordtype_name from ordtype_mast where ord_type = :p_ord_type";
                string ord_type_name = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_ord_type", por.ord_type)).SingleOrDefault();

                string sql4 = "select portype_desc from portype_mast where por_type = :p_por_type";
                string por_type_name = ctx.Database.SqlQuery<string>(sql4, new OracleParameter("p_por_type", por.por_type)).SingleOrDefault();

                string sql5 = "select wh_tdesc from wh_mast where wh_code = :p_wh";
                string wh_name = ctx.Database.SqlQuery<string>(sql5, new OracleParameter("p_wh", por.wh_code)).SingleOrDefault();


                OrderView view = new ModelViews.OrderView()
                {
                    entity = por.entity,
                    entity_name = entity_name,
                    por_no = por.por_no,
                    por_date = por.por_date,
                    req_date = por.req_date,
                    cust_code = por.cust_code,
                    cust_name = por.cust_name,
                    ref_no = por.ref_no,
                    dept = por.dept,
                    dept_name = dept_name,
                    ord_type = por.ord_type,
                    ord_type_name = ord_type_name,
                    por_type = por.por_type,
                    por_type_name = por_type_name,
                    por_priority = por.por_priority,
                    wh_code = por.wh_code,
                    wh_name = wh_name,
                    grp_code = por.grp_code,
                    country = por.country,
                    por_remark = por.por_remark,
                    orderDetail = new List<ModelViews.OrderDetailView>(),
                    orderSpecial = new List<ModelViews.OrderSpecialView>(),
                    remark = new List<ModelViews.OrderRemarkView>()

                };

                var vspec = "";
                string sqld = "select por_no , line_no , prod_code , prod_name , qty_ord , pdgrp_code , pdcolor_code , pdsize_code , design , uom , gplabel_no , skb_flag , dsgn_no, sd_no from por_det where pd_entity = :p_entity and por_no = :p_por_no";
                List<OrderDetailView> por_det = ctx.Database.SqlQuery<OrderDetailView>(sqld, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", por_no)).ToList();

                List<OrderSpecialView> specialViews = new List<OrderSpecialView>();

                foreach (var i in por_det)
                {
                    vspec = "";
                    // Find Special Order
                    string sqlp = "select spc_desc from por_special_det where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code order by item ";
                    List<SpecialDescView> special = ctx.Database.SqlQuery<SpecialDescView>(sqlp, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", por_no), new OracleParameter("p_prod_code", i.prod_code)).ToList();
                    foreach (var z in special)
                    {
                        vspec = vspec + z.spc_desc + '/';
                    }

                    if (vspec != "")
                    {

                        view.orderSpecial.Add(new ModelViews.OrderSpecialView()
                        {
                            por_no = i.por_no,
                            prod_code = i.prod_code,
                            prod_name = i.prod_name,
                            spc_desc = vspec.Trim('/')
                        });
                    }

                    // Find Actual Qty
                    string sql8 = "select nvl(sum(qty_pdt),0) from pdtran where pd_entity = :p_entity and por_no = :p_por_no and prod_code=:p_prod_code";
                    int vqty_act = ctx.Database.SqlQuery<int>(sql8, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", por_no),new OracleParameter("p_prod_code", i.prod_code)).SingleOrDefault();


                    // Find Sub Product    
                    List<SubProductView> subViews = new List<SubProductView>();

                    string sqls = "select por_no , item ,bom_code , description , pack , qty_ord , uom_code , width , length ,height , size_uom from por_det1 where pd_entity = :p_entity and por_no =:p_por_no and line_no = :p_line_no";
                    List<SubProductView> subProd = ctx.Database.SqlQuery<SubProductView>(sqls, new OracleParameter("p_entity", entity), new OracleParameter("p_por_no", por_no), new OracleParameter("p_line_no", i.line_no)).ToList();

                    foreach (var x in subProd)
                    {
                        SubProductView sView = new SubProductView()
                        {
                            por_no = x.por_no,
                            item = x.item,
                            bom_code = x.bom_code,
                            description = x.description,
                            pack = x.pack,
                            qty_ord = x.qty_ord,
                            uom_code = x.uom_code,
                            width = x.width,
                            length = x.length,
                            height = x.height,
                            size_uom = x.size_uom,
                            size = (x.width.ToString() + "X" + x.length.ToString() + "X" + x.height.ToString() + ' ' + x.size_uom).Replace("X0", "")
                            
                        };

                        subViews.Add(sView);

                    }

                    // Find Packageing
                    string sql6 = "select bom_code||packaging_no from product where prod_code = :p_prod_code";
                    string packaging = ctx.Database.SqlQuery<string>(sql6, new OracleParameter("p_prod_code", i.prod_code)).SingleOrDefault();


                    // Find barcode
                    string sql7 = "select bar_code from product where prod_code = :p_prod_code";
                    string vbar_code = ctx.Database.SqlQuery<string>(sql7, new OracleParameter("p_prod_code", i.prod_code)).SingleOrDefault();


                    view.orderDetail.Add(new ModelViews.OrderDetailView()
                    {

                            por_no = i.por_no,
                            line_no = i.line_no,
                            prod_code = i.prod_code,
                            prod_name = i.prod_name,
                            qty_ord = i.qty_ord,
                            qty_act = vqty_act,
                            pdgrp_code = i.pdgrp_code,
                            pdcolor_code = i.pdcolor_code,
                            pdsize_code = i.pdsize_code,
                            design = i.design,
                            uom = i.uom,
                            gplabel_no = i.gplabel_no,
                            skb_flag = i.skb_flag,
                            dsgn_no = i.dsgn_no,
                            sd_no = i.sd_no,
                            packaging = packaging,
                            bar_code = vbar_code,
                            subProduct = subViews,

                    });
                   

                }

                // Find Remark
                string sqlrem = "select line_no , trcmt_desc from jotrcmt_det where trcmt_no = :p_trcmt_no";
                List<OrderRemarkView> por_rem = ctx.Database.SqlQuery<OrderRemarkView>(sqlrem, new OracleParameter("p_trcmt_no", por.por_remark)).ToList();

                foreach (var y in por_rem)
                {
                    view.remark.Add(new ModelViews.OrderRemarkView()
                    {
                        line_no = y.line_no,
                        trcmt_desc = y.trcmt_desc
                    });
                }


                return view;
            }
        }

        public OrderSumView OrderSummary(OrderSumSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                
                string ventity = model.entity_code;
                string vpdjit_grp = model.pdjit_grp;
                DateTime vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vwc_code = model.wc_code;
                string vspec = "";
               

                string sql = "select req_date , pdjit_grp ,  sum(qty_req) total_plan_qty , sum(nvl(qty_fgg,0)) total_actual_qty from mps_det where entity= :p_entity and req_date = trunc(:p_req_date) and nvl(build_type,'HMJIT') = :p_build_type and pdjit_grp = :p_pdjit_grp group by req_date , pdjit_grp";
                OrderSumView mps = ctx.Database.SqlQuery<OrderSumView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_pdjit_grp", vpdjit_grp)).SingleOrDefault();

                string sql2 = "select nvl(sum(qty_pdt),0) from mps_det_wc where entity = :p_entity and req_date = trunc(:p_req_date) and wc_code = :p_wc_code and pdjit_grp = :p_pdjit_grp and mps_st='Y'";
                int total_act_qty = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).SingleOrDefault();


                //define model view
                OrderSumView view = new ModelViews.OrderSumView()
                {
                    req_date = mps.req_date,
                    pdjit_grp = mps.pdjit_grp,
                    build_type = vbuild_type,
                    total_plan_qty = mps.total_plan_qty,
                    total_actual_qty = total_act_qty,
                    total_diff_qty = mps.total_plan_qty - total_act_qty,
                    productDetail = new List<ModelViews.OrderProductView>()
                };

                string sqld = "select a.prod_code , a.prod_tname prod_name , b.bar_code , a.model_name model , b.tick_no spec, a.size_name , b.style_code style , b.weight_net weight , sum(a.qty_req) plan_qty , sum(nvl(a.qty_fgg,0)) act_qty from mps_det a , product b where a.prod_code=b.prod_code and  a.entity=:p_entity and a.req_date = trunc(:p_req_date) and nvl(a.build_type,'HMJIT')= :p_build_type and a.pdjit_grp = :p_pdjit_grp  group by a.prod_code , a.prod_tname , b.bar_code , a.model_name , b.tick_no , a.size_name , b.style_code , b.weight_net";
                List<OrderProductView> prod = ctx.Database.SqlQuery<OrderProductView>(sqld, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                foreach(var x in prod)
                {
                    string sql1 = "select nvl(sum(qty_pdt),0) from mps_det_wc where entity=:p_entity and req_date = trunc(:p_req_date) and wc_code=:p_wc_code and pdjit_grp=:p_pdjit_grp and nvl(build_type,'HMJIT') =:p_build_type and mps_st =  'Y' and prod_code=:p_prod_code";
                    int act_qty = ctx.Database.SqlQuery<int>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();

                    

                    List<PorDetailView> porViews = new List<PorDetailView>();

                    //string sqlp = "select a.por_no , b.qty_req , nvl(b.weight_net,0) weight from mps_det a, por_det b  where a.por_no=b.por_no and a.prod_code=b.prod_code  and a.entity= :p_entity and a.req_date = trunc(:p_req_date) and nvl(a.build_type,'HMJIT')= :p_build_type and b.prod_code= :p_prod_code";
                    string sqlp = "select a.por_no , sum(a.qty_req) qty_req , nvl(b.weight_net,0) weight from mps_det a, por_det b  where a.por_no=b.por_no and a.prod_code=b.prod_code  and a.entity=  :p_entity and a.req_date = trunc(:p_req_date) and nvl(a.build_type,'HMJIT') =  :p_build_type and b.prod_code = :p_prod_code group by a.por_no , b.weight_net";
                    List<PorDetailView> por = ctx.Database.SqlQuery<PorDetailView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_prod_code", x.prod_code)).ToList();

                    foreach (var y in por)
                    {
                        string sqlsp = "select spc_desc from por_special_det where pd_entity = :p_entity and por_no = :p_por_no and prod_code = :p_prod_code order by item ";
                        List<SpecialDescView> special = ctx.Database.SqlQuery<SpecialDescView>(sqlsp, new OracleParameter("p_entity", ventity), new OracleParameter("p_por_no", y.por_no), new OracleParameter("p_prod_code", x.prod_code)).ToList();
                        foreach (var z in special)
                        {
                            vspec = vspec + z.spc_desc + '/';
                        }

                        PorDetailView pView = new PorDetailView()
                        {
                            por_no = y.por_no,
                            qty_req = y.qty_req,
                            weight = Math.Round(y.qty_req * (y.weight/1000),3),
                            special_order = vspec.Trim('/')
                        };

                        porViews.Add(pView);

                    }

                    view.productDetail.Add(new ModelViews.OrderProductView()
                    {
                        prod_code  = x.prod_code,
                        prod_name = x.prod_name,
                        bar_code = x.bar_code,
                        style = x.style,
                        weight = x.weight,
                        model = x.model,
                        size_name = x.size_name,
                        spec = x.spec,
                        weight_kg = (x.weight*x.plan_qty)/1000,
                        plan_qty = x.plan_qty,
                        act_qty = act_qty,
                        diff_qty = x.plan_qty - act_qty,
                        porDetail = porViews,

                    });
                }



                return view;
            }

        }

        public ProductionTrackingView ProductionTracking(ProductionTrackingSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                DateTime vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vpdjit_grp = model.pdjit_grp;
                string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                int vact_qty = 0;

                //int total_plan_qty = 0;


                //DateTime req_tmp = DateTime.Now;

                //string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='Y' and a.user_id=:p_user_id";

                //WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();


                //define model view
                ProductionTrackingView view = new ModelViews.ProductionTrackingView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    entity = ventity,
                    req_date = vreq_date,
                    build_type = model.build_type,
                    productGroups = new List<ModelViews.ProductDataGroupView>(),
                    displayGroups = new List<ModelViews.DisplayWcGroupView>()
                };

                //query data

                string sqlg = "select a.wc_code , b.wc_tdesc wc_name , c.wc_seq from pd_wcctl_pdgrp a , wc_mast b , pd_wcctl_seq c where a.wc_code=b.wc_code  and a.wc_code=c.wc_code and c.pd_entity= :p_entity and a.pdgrp_code=:p_pdjit_grp order by c.wc_seq";
                List<WcGroupView> group = ctx.Database.SqlQuery<WcGroupView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                string sqlp = "select a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code style , sum(a.qty_pdt) plan_qty from mps_det_wc a , product b where a.prod_code=b.prod_code and a.entity= :p_entity and a.req_date = trunc(:p_req_date) and a.wc_code = :p_wc_code and a.pdjit_grp= :p_pdjit_grp group by a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code";
                List<ProductDataGroupView> prod = ctx.Database.SqlQuery<ProductDataGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date) , new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                List<DisplayWcGroupView> displayGroupViews = new List<DisplayWcGroupView>();

                foreach (var x in prod)
                {
                    List<ProductDataGroupDetailView> groupViews = new List<ProductDataGroupDetailView>();

                    foreach (var y in group)
                    {
                        
                            string sql2 = "select nvl(sum(qty_pdt),0) from mps_det_wc where entity= :p_entity and req_date = trunc(:p_req_date) and wc_code = :p_wc_code and pdjit_grp= :p_pdjit_grp and prod_code = :p_prod_code and mps_st='Y'";
                            int group_qty = ctx.Database.SqlQuery<int>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", y.wc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();






                        ProductDataGroupDetailView gView = new ProductDataGroupDetailView()
                        {
                            wc_code = y.wc_code,
                            wc_name = y.wc_name,
                            qty = group_qty

                        };
                        vact_qty = group_qty;

                       
                        groupViews.Add(gView);

                        

                        //DisplayWcGroupView dView = new DisplayWcGroupView()
                        //{
                        //    wc_code = y.wc_code,
                        //    wc_name = y.wc_name,

                        //};

                        //displayGroupViews.Add(dView);


                    }


                    //displayGroupViews.Add(new ModelViews.DisplayWcGroupView()
                    //{
                    //    wc_code = "Diff Qty",
                    //    wc_name = "Diff Qty",
                    //});


                    groupViews.Add(new ModelViews.ProductDataGroupDetailView()
                    {
                        wc_code = "Diff Qty",
                        wc_name = "Diff Qty",
                        qty = x.plan_qty - vact_qty
                    });


                    view.productGroups.Add(new ModelViews.ProductDataGroupView()
                    {
                        prod_code = x.prod_code,
                        prod_name = x.prod_name,
                        style = x.style,
                        model_name = x.model_name,
                        size_name = x.size_name,
                        plan_qty = x.plan_qty,
                        dataGroups = groupViews,


                    });


                }

                foreach (var z in group)
                {
                    DisplayWcGroupView dView = new DisplayWcGroupView()
                    {
                        wc_code = z.wc_code,
                        wc_name = z.wc_name,

                    };

                    displayGroupViews.Add(dView);
                }

                displayGroupViews.Add(new ModelViews.DisplayWcGroupView()
                {
                    wc_code = "Diff Qty",
                    wc_name = "Diff Qty",
                });

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationView SearchDataCurrent(JobOperationSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vwc_code = model.wc_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser = model.user_id;


                DateTime req_tmp = DateTime.Now;


                if(vreq_date == "")
                {
                    vreq_date = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if(vwc_code == "")
                {
                    string sqlw = "select a.dept_code  from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.user_id=:p_user_id and rownum = 1";
                    string wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_user_id", vuser)).FirstOrDefault();
                    vwc_code = wc;
                }

                string sqlj = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string wc_name = ctx.Database.SqlQuery<string>(sqlj, new OracleParameter("param1", vwc_code)).SingleOrDefault();

               
                //define model view
                JobOperationView view = new ModelViews.JobOperationView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    wc_code = vwc_code,
                    wc_name = wc_name,
                    build_type = model.build_type,
                    dataTotals = new List<ModelViews.JobOperationTotalGroupView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {
                    
                    List<JobOperationDetailView> detailViews = new List<JobOperationDetailView>();

                    string sqlt = "select  req_date ,sum(plan_qty) total_plan_qty , sum(act_qty) total_act_qty , sum(cancel_qty) total_cancel_qty , sum(defect_qty) total_defect_qty from (";
                    sqlt += " select a.req_date , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                    sqlt += " from mps_det_wc a";
                    sqlt += " where a.entity= :p_entity ";
                    sqlt += " and trunc(a.req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlt += " and a.mps_st <> 'OCL'";
                    sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                    sqlt += " and a.wc_code = :p_wc_code";
                    sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sqlt += " group by a.req_date";
                    sqlt += " union";
                    sqlt += " select  a.req_date , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                    sqlt += " from mps_det_wc a ";
                    sqlt += " where a.entity= :p_entity ";
                    sqlt += " and trunc(a.req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlt += " and a.mps_st='Y'";
                    sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                    sqlt += " and a.wc_code = :p_wc_code";
                    sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sqlt += " group by a.req_date";
                    sqlt += " union";
                    sqlt += " select a.req_date , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                    sqlt += " from mps_det_wc a";
                    sqlt += " where a.entity= :p_entity ";
                    sqlt += " and trunc(a.req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sqlt += " and a.mps_st='OCL'";
                    sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                    sqlt += " and a.wc_code = :p_wc_code";
                    sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sqlt += " group by a.req_date";
                    sqlt += " ) group by req_date";
                    sqlt += " order by req_date";

                    List<JobOperationTotalGroupView> totalView = ctx.Database.SqlQuery<JobOperationTotalGroupView>(sqlt, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();

                    

                    string sql = "select  req_date , pdjit_grp , wc_code  ,sum(plan_qty) plan_qty , sum(act_qty) act_qty , sum(cancel_qty) cancel_qty , sum(defect_qty) defect_qty from (";
                    sql += " select req_date , pdjit_grp , wc_code , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                    sql += " from mps_det_wc where entity= :p_entity ";
                    sql += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sql += " and mps_st <> 'OCL'";
                    sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                    sql += " and wc_code = :p_wc_code";
                    sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sql += " group by req_date , pdjit_grp , wc_code";
                    sql += " union";
                    sql += " select req_date , pdjit_grp , wc_code , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                    sql += " from mps_det_wc where entity= :p_entity ";
                    sql += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sql += " and mps_st='Y'";
                    sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                    sql += " and wc_code = :p_wc_code";
                    sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sql += " group by req_date , pdjit_grp , wc_code";
                    sql += " union";
                    sql += " select req_date , pdjit_grp , wc_code , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                    sql += " from mps_det_wc where entity= :p_entity ";
                    sql += " and trunc(req_date) = to_date(:p_req_date,'dd/mm/yyyy')";
                    sql += " and mps_st='OCL'";
                    sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                    sql += " and wc_code = :p_wc_code";
                    sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                    sql += " group by req_date , pdjit_grp , wc_code ";
                    sql += " ) group by req_date , pdjit_grp , wc_code";
                    sql += " order by req_date , pdjit_grp";

                    List<JobOperationDetailView> jobcurrentView = ctx.Database.SqlQuery<JobOperationDetailView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();


                    foreach (var i in jobcurrentView)
                    {
                        string sql1 = "select pdgrp_tname from pdgrp_mast where pdgrp_code = :p_pdgrp_code";
                        string group_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_pdgrp_code", i.pdjit_grp)).SingleOrDefault();

                        JobOperationDetailView dView = new JobOperationDetailView()
                        {
                            display_group = group_name,
                            display_type = "",
                            req_date = i.req_date,
                            pdjit_grp = i.pdjit_grp,
                            plan_qty = i.plan_qty,
                            act_qty = i.act_qty,
                            cancel_qty = i.cancel_qty,
                            defect_qty = i.defect_qty,
                            diff_qty = i.plan_qty - (i.act_qty + i.defect_qty) - i.cancel_qty
                        };

                        detailViews.Add(dView);
                    }

                    foreach (var x in totalView)
                    {
                        view.dataTotals.Add(new ModelViews.JobOperationTotalGroupView()
                        {
                            req_date = x.req_date,
                            total_plan_qty = x.total_plan_qty,
                            total_act_qty = x.total_act_qty,
                            total_cancel_qty = x.total_cancel_qty,
                            total_defect_qty = x.total_defect_qty,
                            total_diff_qty = x.total_plan_qty - (x.total_act_qty + x.total_defect_qty) - x.total_cancel_qty,
                            dataGroups = detailViews
                        });
                    }

                }
                else if(model.build_type == "HMSTK")
                {

                }

                //return data to contoller
                return view;
            }
        }

        public JobOperationView SearchDataForward(JobOperationSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vwc_code = model.wc_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser = model.user_id;


                if (vreq_date == "")
                {
                    vreq_date = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if (vwc_code == "")
                {
                    string sqlw = "select a.dept_code  from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.user_id=:p_user_id and rownum = 1";
                    string wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_user_id", vuser)).FirstOrDefault();
                    vwc_code = wc;
                }

                string sqlj = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string wc_name = ctx.Database.SqlQuery<string>(sqlj, new OracleParameter("param1", vwc_code)).SingleOrDefault();


                //define model view
                JobOperationView view = new ModelViews.JobOperationView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    wc_code = vwc_code,
                    wc_name = wc_name,
                    build_type = model.build_type,
                    dataTotals = new List<ModelViews.JobOperationTotalGroupView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {
                    
                    string sqlr = "select distinct req_date from mps_det_wc where entity = :p_entity and wc_code = :p_wc_code and  trunc(req_date) > to_date(:p_req_date,'dd/mm/yyyy') order by req_date";
                    List<PorReqView> req = ctx.Database.SqlQuery<PorReqView>(sqlr, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date", vreq_date)).ToList();

                    foreach(var k in req)
                    {
                        List<JobOperationDetailView> detailViews = new List<JobOperationDetailView>();

                        string sqlt = "select  req_date ,sum(plan_qty) total_plan_qty , sum(act_qty) total_act_qty , sum(cancel_qty) total_cancel_qty , sum(defect_qty) total_defect_qty from (";
                        sqlt += " select a.req_date , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a";
                        sqlt += " where a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_req_date)";
                        sqlt += " and a.mps_st <> 'OCL'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " union";
                        sqlt += " select  a.req_date , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a";
                        sqlt += " where a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_req_date)";
                        sqlt += " and a.mps_st='Y'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " union";
                        sqlt += " select a.req_date , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a";
                        sqlt += " where a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_req_date)";
                        sqlt += " and a.mps_st='OCL'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " ) group by req_date";
                        sqlt += " order by req_date";

                        List<JobOperationTotalGroupView> totalView = ctx.Database.SqlQuery<JobOperationTotalGroupView>(sqlt, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", k.req_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();
                       



                        string sql = "select  req_date , pdjit_grp , wc_code  ,sum(plan_qty) plan_qty , sum(act_qty) act_qty , sum(cancel_qty) cancel_qty , sum(defect_qty) defect_qty from (";
                        sql += " select req_date , pdjit_grp , wc_code , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc where entity= :p_entity ";
                        sql += " and trunc(req_date) = trunc(:p_req_date)";
                        sql += " and mps_st <> 'OCL'";
                        sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                        sql += " and wc_code = :p_wc_code";
                        sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " group by req_date , pdjit_grp , wc_code";
                        sql += " union";
                        sql += " select req_date , pdjit_grp , wc_code , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc where entity= :p_entity ";
                        sql += " and trunc(req_date) = trunc(:p_req_date)";
                        sql += " and mps_st='Y'";
                        sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                        sql += " and wc_code = :p_wc_code";
                        sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " group by req_date , pdjit_grp , wc_code";
                        sql += " union";
                        sql += " select req_date , pdjit_grp , wc_code , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc where entity= :p_entity ";
                        sql += " and trunc(req_date) = trunc(:p_req_date)";
                        sql += " and mps_st='OCL'";
                        sql += " and nvl(build_type,'HMJIT') = :p_build_type";
                        sql += " and wc_code = :p_wc_code";
                        sql += " and pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " group by req_date , pdjit_grp , wc_code ";
                        sql += " ) group by req_date , pdjit_grp , wc_code";
                        sql += " order by req_date , pdjit_grp";

                        List<JobOperationDetailView> jobcurrentView = ctx.Database.SqlQuery<JobOperationDetailView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", k.req_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();


                        foreach (var i in jobcurrentView)
                        {
                            string sql1 = "select pdgrp_tname from pdgrp_mast where pdgrp_code = :p_pdgrp_code";
                            string group_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_pdgrp_code", i.pdjit_grp)).SingleOrDefault();

                            JobOperationDetailView dView = new JobOperationDetailView()
                            {
                                display_group = group_name,
                                display_type = "",
                                req_date = i.req_date,
                                pdjit_grp = i.pdjit_grp,
                                plan_qty = i.plan_qty,
                                act_qty = i.act_qty,
                                cancel_qty = i.cancel_qty,
                                defect_qty = i.defect_qty,
                                diff_qty = i.plan_qty - (i.act_qty + i.defect_qty) - i.cancel_qty
                            };

                            detailViews.Add(dView);
                        }

                        foreach (var x in totalView)
                        {
                            view.dataTotals.Add(new ModelViews.JobOperationTotalGroupView()
                            {
                                req_date = x.req_date,
                                total_plan_qty = x.total_plan_qty,
                                total_act_qty = x.total_act_qty,
                                total_cancel_qty = x.total_cancel_qty,
                                total_defect_qty = x.total_defect_qty,
                                total_diff_qty = x.total_plan_qty - (x.total_act_qty + x.total_defect_qty) - x.total_cancel_qty,
                                dataGroups = detailViews
                            });
                        }
                    }


                }
                else if (model.build_type == "HMSTK")
                {

                }

                //return data to contoller
                return view;
            }
        }

        public JobOperationView SearchDataPending(JobOperationSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vwc_code = model.wc_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser = model.user_id;

                if (vreq_date == "")
                {
                    vreq_date = DateTime.Now.ToString("dd/MM/yyyy");
                }

                if (vwc_code == "")
                {
                    string sqlw = "select a.dept_code  from auth_function a , wc_mast b where a.dept_code = b.wc_code and a.function_id='PDOPTHM' and a.user_id=:p_user_id and rownum = 1";
                    string wc = ctx.Database.SqlQuery<string>(sqlw, new OracleParameter("p_user_id", vuser)).FirstOrDefault();
                    vwc_code = wc;
                }

                string sqlj = "select wc_tdesc from wc_mast where  wc_code = :param1";

                string wc_name = ctx.Database.SqlQuery<string>(sqlj, new OracleParameter("param1", vwc_code)).SingleOrDefault();


                //define model view
                JobOperationView view = new ModelViews.JobOperationView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    wc_code = vwc_code,
                    wc_name = wc_name,
                    build_type = model.build_type,
                    dataTotals = new List<ModelViews.JobOperationTotalGroupView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {
                   
                    string sqlr = "select distinct a.req_date from mps_det_wc a , por_mast b where a.entity = :p_entity and a.wc_code = :p_wc_code and a.por_no = b.por_no and b.por_status not in ('CLS','OCL') and trunc(a.req_date) between to_date(:p_req_date1,'dd/mm/yyyy')-30 and to_date(:p_req_date2,'dd/mm/yyyy')-1 order by req_date";
                    List<PorReqView> req = ctx.Database.SqlQuery<PorReqView>(sqlr, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_req_date1", vreq_date), new OracleParameter("p_req_date2", vreq_date)).ToList();

                    foreach (var k in req)
                    {
                        List<JobOperationDetailView> detailViews = new List<JobOperationDetailView>();

                        string sqlt = "select  req_date ,sum(plan_qty) total_plan_qty , sum(act_qty) total_act_qty , sum(cancel_qty) total_cancel_qty , sum(defect_qty) total_defect_qty from (";
                        sqlt += " select a.req_date , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a , por_mast c";
                        sqlt += " where  a.por_no = c.por_no";
                        sqlt += " and c.por_status not in ('CLS','OCL')";
                        sqlt += " and a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_rq_date) ";
                        sqlt += " and a.mps_st <> 'OCL'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " union";
                        sqlt += " select  a.req_date , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a , por_mast c";
                        sqlt += " where a.por_no = c.por_no";
                        sqlt += " and c.por_status not in ('CLS','OCL')";
                        sqlt += " and a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_rq_date) ";
                        sqlt += " and a.mps_st='Y'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " union";
                        sqlt += " select a.req_date , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                        sqlt += " from mps_det_wc a , por_mast c";
                        sqlt += " where a.por_no = c.por_no";
                        sqlt += " and c.por_status not in ('CLS','OCL')";
                        sqlt += " and a.entity= :p_entity ";
                        sqlt += " and trunc(a.req_date) = trunc(:p_rq_date) ";
                        sqlt += " and a.mps_st='OCL'";
                        sqlt += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sqlt += " and a.wc_code = :p_wc_code";
                        sqlt += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sqlt += " group by a.req_date";
                        sqlt += " ) group by req_date";
                        sqlt += " order by req_date";

                        List<JobOperationTotalGroupView> totalView = ctx.Database.SqlQuery<JobOperationTotalGroupView>(sqlt, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", k.req_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();



                        string sql = "select req_date , pdjit_grp , wc_code  ,sum(plan_qty) plan_qty , sum(act_qty) act_qty , sum(cancel_qty) cancel_qty , sum(defect_qty) defect_qty from (";
                        sql += " select a.req_date , a.pdjit_grp , a.wc_code , count(*) plan_qty ,  0 act_qty , 0 cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc a , por_mast b";
                        sql += " where a.por_no = b.por_no";
                        sql += " and a.entity = :p_entity ";
                        sql += " and trunc(a.req_date) = trunc(:p_req_date) ";
                        sql += " and a.mps_st <> 'OCL'";
                        sql += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sql += " and a.wc_code = :p_wc_code";
                        sql += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " and b.por_status not in ('CLS','OCL')";
                        sql += " group by a.req_date , a.pdjit_grp , a.wc_code ";
                        sql += " union";
                        sql += " select a.req_date , a.pdjit_grp , a.wc_code , 0 plan_qty ,  count(*) act_qty , 0 cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc a , por_mast b";
                        sql += " where a.por_no = b.por_no";
                        sql += " and a.entity = :p_entity ";
                        sql += " and trunc(a.req_date) = trunc(:p_req_date) ";
                        sql += " and a.mps_st='Y'";
                        sql += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sql += " and a.wc_code = :p_wc_code";
                        sql += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " and b.por_status not in ('CLS','OCL')";
                        sql += " group by a.req_date , a.pdjit_grp , a.wc_code ";
                        sql += " union";
                        sql += " select a.req_date , a.pdjit_grp , a.wc_code , 0 plan_qty ,  0 act_qty , count(*) cancel_qty , 0 defect_qty";
                        sql += " from mps_det_wc a , por_mast b";
                        sql += " where a.por_no = b.por_no";
                        sql += " and a.entity = :p_entity ";
                        sql += " and trunc(a.req_date) = trunc(:p_req_date) ";
                        sql += " and a.mps_st='OCL'";
                        sql += " and nvl(a.build_type,'HMJIT') = :p_build_type";
                        sql += " and a.wc_code = :p_wc_code";
                        sql += " and a.pdjit_grp not in ( select jit_grp from pd_disgrp_ctl where entity = :p_entity2  and user_id = :p_user_id)";
                        sql += " and b.por_status not in ('CLS','OCL')";
                        sql += " group by a.req_date , a.pdjit_grp , a.wc_code";
                        sql += " ) group by req_date , pdjit_grp , wc_code ";
                        sql += " order by req_date , pdjit_grp ";

                        List<JobOperationDetailView> jobcurrentView = ctx.Database.SqlQuery<JobOperationDetailView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", k.req_date), new OracleParameter("p_build_type", vbuild_type), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_user_id", vuser)).ToList();


                        foreach (var i in jobcurrentView)
                        {
                            string sql1 = "select pdgrp_tname from pdgrp_mast where pdgrp_code = :p_pdgrp_code";
                            string group_name = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_pdgrp_code", i.pdjit_grp)).SingleOrDefault();

                            JobOperationDetailView dView = new JobOperationDetailView()
                            {
                                display_group = group_name,
                                display_type = "",
                                req_date = i.req_date,
                                pdjit_grp = i.pdjit_grp,
                                plan_qty = i.plan_qty,
                                act_qty = i.act_qty,
                                cancel_qty = i.cancel_qty,
                                defect_qty = i.defect_qty,
                                diff_qty = i.plan_qty - (i.act_qty + i.defect_qty) - i.cancel_qty
                            };

                            detailViews.Add(dView);
                        }

                        foreach (var x in totalView)
                        {
                            view.dataTotals.Add(new ModelViews.JobOperationTotalGroupView()
                            {
                                req_date = x.req_date,
                                total_plan_qty = x.total_plan_qty,
                                total_act_qty = x.total_act_qty,
                                total_cancel_qty = x.total_cancel_qty,
                                total_defect_qty = x.total_defect_qty,
                                total_diff_qty = x.total_plan_qty - (x.total_act_qty + x.total_defect_qty) - x.total_cancel_qty,
                                dataGroups = detailViews
                            });
                        }
                    }

                    

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