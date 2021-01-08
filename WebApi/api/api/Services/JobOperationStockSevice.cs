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
    public class JobOperationStockSevice : IJobOperationStockService
    {
        public ProductionTrackingStockView ProductionTrackingDetailStock(ProductGroupSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;
                //string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                int vact_qty = 0;
                int vdefect_qty = 0;

                string sql3 = "select a.dept_code  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id and rownum = 1";

                string vwc_code = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();


                //define model view
                ProductionTrackingStockView view = new ModelViews.ProductionTrackingStockView()
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

                string sql1 = "select c.disgrp_line_code distype_code ,d.disgrp_line_desc type , a.prod_code_sub prod_code , a.prod_name_sub prod_name, nvl(sum(a.qty_plan),0) qty "+
                    "from mps_det_wc_stk a , bm_sub_bom_code b, pd_distype_mast c , pd_disgrp_line d " +
                    "where a.prod_code_sub = b.bom_code " +
                    "and b.distype_code = c.distype_code " +
                    "and c.disgrp_line_code = d.disgrp_line_code " +
                    "and a.entity = :p_entity " +
                    "and a.req_date = to_date(:p_req_date, 'dd/mm/yyyy') " +
                    "and a.por_no = :p_por_no " +
                    "and a.ref_no = :p_ref_no " +
                    "and a.wc_code = :p_wc_code " +
                    "group by c.disgrp_line_code ,d.disgrp_line_desc , a.prod_code_sub , a.prod_name_sub " +
                    "union " +
                    "select 'OTHER' distype_code  ,'OTHER' type ,a1.prod_code_sub prod_code , a1.prod_name_sub prod_name , nvl(sum(a1.qty_plan), 0) qty " +
                    "from mps_det_wc_stk a1, bm_sub_bom_code a2 " +
                    "where a1.entity = :p_entity2 " +
                    "and a1.req_date = to_date(:p_req_date2, 'dd/mm/yyyy') " +
                    "and a1.por_no = :p_por_no2 " +
                    "and a1.ref_no = :p_ref_no2 " +
                    "and a1.wc_code = :p_wc_code2 " +
                    "and a1.prod_code_sub = a2.bom_code " +
                    "and a2.distype_code is null " +
                    "having nvl(sum(a1.qty_plan),0) > 0 " +
                    "group by a1.prod_code_sub , a1.prod_name_sub " +
                    "order by 3";

                List<ProductDataGroupView> prod = ctx.Database.SqlQuery<ProductDataGroupView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_por_no2", vpor_no), new OracleParameter("p_ref_no2", vref_no), new OracleParameter("p_wc_code2", vwc_code)).ToList();

                string sqlg = "select a.wc_code , b.wc_tdesc wc_name , c.wc_seq from pd_wcctl_pdgrp a , wc_mast b , pd_wcctl_seq c where a.wc_code=b.wc_code  and a.wc_code=c.wc_code and c.pd_entity= :p_entity and a.pdgrp_code='ZZ' order by c.wc_seq";
                List<WcGroupView> group = ctx.Database.SqlQuery<WcGroupView>(sqlg, new OracleParameter("p_entity", ventity)).ToList();

                //string sqlp = "select a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code style , sum(a.qty_pdt) plan_qty from mps_det_wc a , product b where a.prod_code=b.prod_code and a.entity= :p_entity and a.req_date = trunc(:p_req_date) and a.wc_code = :p_wc_code and a.pdjit_grp= :p_pdjit_grp group by a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code";
                //List<ProductDataGroupView> prod = ctx.Database.SqlQuery<ProductDataGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                List<DisplayWcGroupView> displayGroupViews = new List<DisplayWcGroupView>();

                foreach (var x in prod)
                {
                    List<ProductDataGroupDetailView> groupViews = new List<ProductDataGroupDetailView>();

                    foreach (var y in group)
                    {

                        string sql2 = "select nvl(sum(a.qty_fin),0) qty_fin , nvl(sum(a.qty_defect),0) qty_defect " +
                            "from mps_det_wc_stk a , bm_sub_bom_code b, pd_distype_mast c , pd_disgrp_line d " +
                            "where a.prod_code_sub = b.bom_code " +
                            "and b.distype_code = c.distype_code " +
                            "and c.disgrp_line_code = d.disgrp_line_code " +
                            "and a.entity = :p_entity " +
                            "and a.req_date = to_date(:p_req_date, 'dd/mm/yyyy') " +
                            "and a.por_no = :p_por_no " +
                            "and a.ref_no = :p_ref_no " +
                            "and a.wc_code = :p_wc_code " +
                            "and a.prod_code_sub = :p_prod_code";
                        QtyGroupView group_qty = ctx.Database.SqlQuery<QtyGroupView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", y.wc_code), new OracleParameter("p_prod_code", x.prod_code)).FirstOrDefault();






                        ProductDataGroupDetailView gView = new ProductDataGroupDetailView()
                        {
                            wc_code = y.wc_code,
                            wc_name = y.wc_code + " : " + y.wc_name,
                            qty = group_qty.qty_fin

                        };
                        vact_qty = group_qty.qty_fin;
                        vdefect_qty = group_qty.qty_defect;

                        groupViews.Add(gView);





                    }


                    groupViews.Add(new ModelViews.ProductDataGroupDetailView()
                    {
                        wc_code = "ZQ : Defect",
                        wc_name = "ZQ : Defect",
                        qty = vdefect_qty
                    });


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
                        type = x.type,
                        plan_qty = x.plan_qty,
                        dataGroups = groupViews,


                    });


                }

                foreach (var z in group)
                {
                    DisplayWcGroupView dView = new DisplayWcGroupView()
                    {
                        wc_code = z.wc_code,
                        wc_name = z.wc_code + " : " +z.wc_name,

                    };

                    displayGroupViews.Add(dView);
                }

                displayGroupViews.Add(new ModelViews.DisplayWcGroupView()
                {
                    wc_code = "ZQ : Defect",
                    wc_name = "ZQ : Defect",
                });

                displayGroupViews.Add(new ModelViews.DisplayWcGroupView()
                {
                    wc_code = "Diff Qty",
                    wc_name = "Diff Qty",
                });

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public ProductionTrackingStockView ProductionTrackingStock(ProductGroupSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;
                //string vwc_code = model.wc_code;
                string vuser_id = model.user_id;
                int vact_qty = 0;
                int vdefect_qty = 0;

                string sql3 = "select a.dept_code  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id and rownum = 1";

                string vwc_code = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();


                //define model view
                ProductionTrackingStockView view = new ModelViews.ProductionTrackingStockView()
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

                string sql1 = "select distinct c.disgrp_line_code distype_code ,d.disgrp_line_desc distype_desc, nvl(sum(a.qty_plan),0) qty, c.distype_sortid " +
                    "from mps_det_wc_stk a , bm_sub_bom_code b, pd_distype_mast c , pd_disgrp_line d " +
                    "where a.prod_code_sub = b.bom_code " +
                    "and b.distype_code = c.distype_code " +
                    "and c.disgrp_line_code = d.disgrp_line_code " +
                    "and a.entity = :p_entity " +
                    "and a.req_date = to_date(:p_req_date, 'dd/mm/yyyy') " +
                    "and a.por_no = :p_por_no " +
                    "and a.ref_no = :p_ref_no " +         
                    "and a.wc_code = :p_wc_code " +
                    "group by c.disgrp_line_code ,d.disgrp_line_desc , c.distype_sortid " +
                    "union " +
                    "select 'OTHER' distype_code  ,'OTHER' distype_desc , nvl(sum(a1.qty_plan),0) qty , 99 distype_sortid " +
                    "from mps_det_wc_stk a1, bm_sub_bom_code a2 " +
                    "where a1.entity = :p_entity2 " +
                    "and a1.req_date = to_date(:p_req_date2, 'dd/mm/yyyy') " +
                    "and a1.por_no = :p_por_no2 " +
                    "and a1.ref_no = :p_ref_no2 " +
                    "and a1.prod_code_sub = a2.bom_code " +
                    "and a2.distype_code is null " +
                    "and wc_code = :p_wc_code2 " +
                    "having nvl(sum(a1.qty_plan),0) > 0 " +
                    "order by 4";

                List<PorTypeDetailView> prod = ctx.Database.SqlQuery<PorTypeDetailView>(sql1, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_entity2", ventity), new OracleParameter("p_req_date2", vreq_date), new OracleParameter("p_por_no2", vpor_no), new OracleParameter("p_ref_no2", vref_no), new OracleParameter("p_wc_code2", vwc_code)).ToList();

                string sqlg = "select a.wc_code , b.wc_tdesc wc_name , c.wc_seq from pd_wcctl_pdgrp a , wc_mast b , pd_wcctl_seq c where a.wc_code=b.wc_code  and a.wc_code=c.wc_code and c.pd_entity= :p_entity and a.pdgrp_code='ZZ' order by c.wc_seq";
                List<WcGroupView> group = ctx.Database.SqlQuery<WcGroupView>(sqlg, new OracleParameter("p_entity", ventity)).ToList();

                //string sqlp = "select a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code style , sum(a.qty_pdt) plan_qty from mps_det_wc a , product b where a.prod_code=b.prod_code and a.entity= :p_entity and a.req_date = trunc(:p_req_date) and a.wc_code = :p_wc_code and a.pdjit_grp= :p_pdjit_grp group by a.prod_code , a.prod_name , a.model_name , a.size_name , b.style_code";
                //List<ProductDataGroupView> prod = ctx.Database.SqlQuery<ProductDataGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", vwc_code), new OracleParameter("p_pdjit_grp", vpdjit_grp)).ToList();

                List<DisplayWcGroupView> displayGroupViews = new List<DisplayWcGroupView>();

                foreach (var x in prod)
                {
                    List<ProductDataGroupDetailView> groupViews = new List<ProductDataGroupDetailView>();

                    foreach (var y in group)
                    {

                        string sql2 = "select nvl(sum(a.qty_fin),0) qty_fin , nvl(sum(a.qty_defect),0) qty_defect " +
                            "from mps_det_wc_stk a , bm_sub_bom_code b, pd_distype_mast c , pd_disgrp_line d " +
                            "where a.prod_code_sub = b.bom_code " +
                            "and b.distype_code = c.distype_code " +
                            "and c.disgrp_line_code = d.disgrp_line_code " +
                            "and a.entity = :p_entity " +
                            "and a.req_date = to_date(:p_req_date, 'dd/mm/yyyy') " +
                            "and a.por_no = :p_por_no " +
                            "and a.ref_no = :p_ref_no " +
                            "and a.wc_code = :p_wc_code " +
                            "and c.disgrp_line_code = :p_group";
                        QtyGroupView group_qty = ctx.Database.SqlQuery<QtyGroupView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", y.wc_code), new OracleParameter("p_group",x.distype_code)).FirstOrDefault();






                        ProductDataGroupDetailView gView = new ProductDataGroupDetailView()
                        {
                            wc_code = y.wc_code,
                            wc_name = y.wc_code + " : " +y.wc_name,
                            qty = group_qty.qty_fin

                        };
                        vact_qty = group_qty.qty_fin;
                        vdefect_qty = group_qty.qty_defect;

                        groupViews.Add(gView);



                 

                    }


                    groupViews.Add(new ModelViews.ProductDataGroupDetailView()
                    {
                        wc_code = "ZQ : Defect",
                        wc_name = "ZQ : Defect",
                        qty = vdefect_qty
                    });


                    groupViews.Add(new ModelViews.ProductDataGroupDetailView()
                    {
                        wc_code = "Diff Qty",
                        wc_name = "Diff Qty",
                        qty = x.qty - vact_qty
                    });


                    view.productGroups.Add(new ModelViews.ProductDataGroupView()
                    {
                        type = x.distype_desc,
                        plan_qty = x.qty,
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
                    wc_code = "ZQ : Defect",
                    wc_name = "ZQ : Defect",
                });

                displayGroupViews.Add(new ModelViews.DisplayWcGroupView()
                {
                    wc_code = "Diff Qty",
                    wc_name = "Diff Qty",
                });

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationStockView SearchDataDefect(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;

                //int total_plan_qty = 0;


                DateTime req_tmp = DateTime.Now;

                string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();


                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = wc.wc_code,
                    wc_name = wc.wc_name,
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , nvl(sum(qty_defect),0) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select nvl(sum(a.qty_defect),0) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }


                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,

                        };

                        displayGroupViews.Add(dView);


                    }


                    


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,


                    });


                }

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationStockView SearchDataFin(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;

                //int total_plan_qty = 0;


                DateTime req_tmp = DateTime.Now;

                string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();


                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = wc.wc_code,
                    wc_name = wc.wc_name,
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_fin) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select sum(a.qty_fin) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }


                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,

                        };

                        displayGroupViews.Add(dView);


                    }


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,


                    });


                }

                view.displayGroups = displayGroupViews;


                return view;
            }
        }

        public JobOperationStockView SearchDataPlan(JobOperationStockSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;

                //int total_plan_qty = 0;


                DateTime req_tmp = DateTime.Now;

                string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";

                WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();

                if(wc == null)
                {
                    throw new Exception("ไม่ได้กำหนดหน่วยผลิตสุดท้าย");
                }

                //define model view
                JobOperationStockView view = new ModelViews.JobOperationStockView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    wc_code = wc.wc_code,
                    wc_name = wc.wc_name,                
                    build_type = model.build_type,
                    porGroups = new List<ModelViews.PorStockGroupView>(),
                    displayGroups = new List<ModelViews.DisplayGroupView>()
                };

                //query data

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc , d.disgrp_sortid from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by d.disgrp_sortid";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_plan) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no  and a.req_date = b.req_date and a.ref_no = b.ref_no and a.prod_code = b.prod_code and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();
                    
                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc , distype_sortid from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line order by distype_sortid";
                        List<DisTypeView> disType = ctx.Database.SqlQuery<DisTypeView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_disgrp_line", y.disgrp_line_code)).ToList();

                        var dis_qty = "";


                        foreach (var z in disType)
                        {
                            string sql2 = "select sum(a.qty_plan) qty , b.distype_code , c.distype_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c  where a.prod_code_sub = b.bom_code and a.entity = c.entity and b.distype_code  = c.distype_code  and a.entity=:p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.por_no=:p_por_no and a.ref_no= :p_ref_no and a.wc_code = :p_wc_code and c.disgrp_line_code= :p_disgrp_line and b.distype_code = :p_distype_code group by b.distype_code , c.distype_desc order by b.distype_code";
                            PorTypeDetailView group_qty = ctx.Database.SqlQuery<PorTypeDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_disgrp_line", y.disgrp_line_code), new OracleParameter("p_distype_code", z.distype_code)).FirstOrDefault();

                            if (group_qty == null)
                            {
                                //dis_qty = dis_qty + z.distype_code + "-" + "/";
                                dis_qty = dis_qty + "0/";
                            }
                            else
                            {
                                //dis_qty = dis_qty + group_qty.distype_code + "-" + group_qty.qty + "/";
                                dis_qty = dis_qty + group_qty.qty + "/";
                            }
                            

                        }

                        PorStockGroupDetailView gView = new PorStockGroupDetailView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                            qty = dis_qty.TrimEnd('/')

                        };

                        groupViews.Add(gView);


                        DisplayGroupView dView = new DisplayGroupView()
                        {
                            disgroup_code = y.disgrp_line_code,
                            disgroup_desc = y.disgrp_line_desc,
                          
                        };

                        displayGroupViews.Add(dView);


                    }

                    //displayGroupViews.Add(new ModelViews.DisplayGroupView()
                    //{
                    //    disgroup_code = "OTHER",
                    //    disgroup_desc = "OTHER",
                    //});


                    string sql3 = "select to_char(nvl(sum(a1.qty_plan),0)) " +
                    "from mps_det_wc_stk a1 , bm_sub_bom_code a2 " +
                    "where a1.entity = :p_entity " +
                    "and a1.req_date =to_date(:p_req_date,'dd/mm/yyyy') " +
                    "and a1.por_no = :p_por_no " +
                    "and a1.ref_no = :p_ref_no " +
                    "and a1.prod_code_sub = a2.bom_code " +
                    "and a2.distype_code is null " +
                    "and wc_code = :p_wc_code";
                    string qty_other = ctx.Database.SqlQuery<string>(sql3, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", x.por_no), new OracleParameter("p_ref_no", x.ref_no), new OracleParameter("p_wc_code", wc.wc_code)).FirstOrDefault();

                    if(qty_other == null)
                    {
                        qty_other = "0";
                    }

                    if(qty_other != "0")
                    {
                        displayGroupViews.Add(new ModelViews.DisplayGroupView()
                        {
                            disgroup_code = "OTHER",
                            disgroup_desc = "OTHER",
                        });


                        groupViews.Add(new ModelViews.PorStockGroupDetailView()
                        {
                            disgroup_code = "OTHER",
                            disgroup_desc = "OTHER",
                            qty = qty_other.ToString()
                        });

                    }


                    //groupViews.Add(new ModelViews.PorStockGroupDetailView()
                    //{
                    //    disgroup_code = "OTHER",
                    //    disgroup_desc = "OTHER",
                    //    qty = qty_other.ToString()
                    //});


                    view.porGroups.Add(new ModelViews.PorStockGroupView()
                    {
                        entity = ventity,
                        por_no = x.por_no,
                        ref_no = x.ref_no,
                        design_name = x.design_name,
                        qty = x.qty,
                        dataGroups = groupViews,
                        

                    });


                }

                view.displayGroups = displayGroupViews;

              
                return view;
            }
            
        }

        public ProductGroupView SearchSummaryProdcutGroup(ProductGroupSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity_code;
                string vreq_date = model.req_date;
                string vbuild_type = model.build_type;
                string vuser_id = model.user_id;
                string vpor_no = model.por_no;
                string vref_no = model.ref_no;

                ProductGroupView view = new ModelViews.ProductGroupView()
                {

                    datas = new List<ModelViews.ProductGroupDetailView>()
                };

                string sql1 = "select a.dept_code wc_code , b.wc_tdesc wc_name  from auth_function a, wc_mast b where a.dept_code = b.wc_code and  a.function_id='PDOPTHM' and a.doc_code='STK' and a.user_id=:p_user_id";
                WcDataView wc = ctx.Database.SqlQuery<WcDataView>(sql1, new OracleParameter("p_user_id", vuser_id)).SingleOrDefault();

                string sql2 = "select c.disgrp_line_code ,d.disgrp_line_desc , b.distype_code , c.distype_desc , sum(a.qty_plan) qty_plan , sum(a.qty_fin) qty_fin , nvl(sum(a.qty_defect),0) qty_defect , c.distype_sortid " +
                    "from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d " +
                    "where a.prod_code_sub = b.bom_code " +
                    "and b.distype_code = c.distype_code " +
                    "and c.disgrp_line_code = d.disgrp_line_code " +
                    "and a.entity = :p_entity " +
                    "and a.req_date =to_date(:p_req_date,'dd/mm/yyyy') " +
                    "and a.por_no = :p_por_no " +
                    "and a.ref_no = :p_ref_no " +
                    "and a.wc_code = :p_wc_code " +
                    "group by c.disgrp_line_code  , b.distype_code , d.disgrp_line_desc , c.distype_desc ,  c.distype_sortid " +
                    //"order by c.disgrp_line_code , c.distype_sortid" +
                    "union " +
                    "select 'OTHER' disgrp_line_code  ,'OTHER' disgrp_line_desc , 'OTHER' distype_code  ,'OTHER' distype_desc , nvl(sum(a1.qty_plan),0) , nvl(sum(a1.qty_fin),0) , nvl(sum(a1.qty_defect),0) , 99 distype_sortid " +
                    "from mps_det_wc_stk a1 , bm_sub_bom_code a2 " +
                    "where a1.entity = :p_entity " +
                    "and a1.req_date =to_date(:p_req_date,'dd/mm/yyyy') " +
                    "and a1.por_no = :p_por_no " +
                    "and a1.ref_no = :p_ref_no " +
                    "and a1.prod_code_sub = a2.bom_code " +
                    "and a2.distype_code is null " +
                    "and wc_code = :p_wc_code " +
                    "having nvl(sum(a1.qty_plan),0) > 0  " +
                    "order by 1 , 8";

                List<ProductGroupDetailView> prod = ctx.Database.SqlQuery<ProductGroupDetailView>(sql2, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_por_no", vpor_no), new OracleParameter("p_ref_no", vref_no), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                foreach (var i in prod)
                {

                    view.datas.Add(new ModelViews.ProductGroupDetailView()
                    {
                        disgrp_line_desc = i.disgrp_line_desc,
                        distype_desc = i.distype_desc,
                        qty_plan = i.qty_plan,
                        qty_fin = i.qty_fin,
                        qty_defect = i.qty_defect,
                        

                    });
                }


                //return data to contoller
                return view;


            }
        } 
        
        }
}