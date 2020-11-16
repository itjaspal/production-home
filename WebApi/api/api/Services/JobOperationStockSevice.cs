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

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by c.disgrp_line_code";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , nvl(sum(qty_defect),0) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line";
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

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by c.disgrp_line_code";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_fin) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();

                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line";
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

                string sqlg = "select distinct c.disgrp_line_code  , d.disgrp_line_desc from mps_det_wc_stk a , bm_sub_bom_code b , pd_distype_mast c , pd_disgrp_line d where a.prod_code_sub = bom_code and b.distype_code = c.distype_code and a.entity = c.entity  and c.entity=d.entity and c.disgrp_line_code = d.disgrp_line_code  and a.entity= :p_entity and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and a.wc_code= :p_wc_code order by c.disgrp_line_code";
                List<GroupStockView> group = ctx.Database.SqlQuery<GroupStockView>(sqlg, new OracleParameter("p_entity", ventity), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_wc_code", wc.wc_code)).ToList();

                string sqlp = "select a.por_no , a.ref_no , max(b.model_name) design_name , sum(qty_plan) qty from mps_det_wc_stk a , mps_det b where a.entity = b.entity and a.por_no = b.por_no and a.entity = :p_entity and wc_code = :p_wc_code and a.req_date = to_date(:p_req_date,'dd/mm/yyyy') and b.build_type = :p_build_type  group by a.por_no , a.ref_no ";
                List<PorStockGroupView> por = ctx.Database.SqlQuery<PorStockGroupView>(sqlp, new OracleParameter("p_entity", ventity), new OracleParameter("p_wc_code", wc.wc_code), new OracleParameter("p_req_date", vreq_date), new OracleParameter("p_build_type", vbuild_type)).ToList();

                List<DisplayGroupView> displayGroupViews = new List<DisplayGroupView>();

                foreach (var x in por)
                {
                    List<PorStockGroupDetailView> groupViews = new List<PorStockGroupDetailView>();
                    
                    foreach (var y in group)
                    {
                        string sql = "select distype_code , distype_desc from pd_distype_mast where entity = :p_entity and  disgrp_line_code = :p_disgrp_line";
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

    }
}