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
    public class ProductionRecService : IProductionRecService
    {
        public int getTimeDelay(string entity, string build_type)
        {
            using (var ctx = new ConXContext())
            {
                string sql = "select time_delay from pd_monitor_ctl where pd_entity = :p_entity and build_type = :p_build_type";
                int vtime = ctx.Database.SqlQuery<int>(sql, new OracleParameter("p_entity", entity), new OracleParameter("p_build_type", build_type)).FirstOrDefault();

                return vtime;
            }
        }

        public ProductionRecTotalView SearchProductionRec(ProductionRecSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                //define model view
                ProductionRecTotalView view = new ModelViews.ProductionRecTotalView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    total_rec_qty = 0,

                    recDetails = new List<ModelViews.ProductionRecView>()
                };

                string ventity     = model.entity_code;
                string vreq_date   = model.doc_date;
                string vbuild_type = model.build_type;
                string vdoc_no = model.doc_no;
                string vdoc_status = model.doc_status;


                if (ventity == null || ventity == "")
                {
                    throw new Exception("โปรดระบุข้อมูล กลุ่มสินค้า");
                }

                if (vreq_date == null || vreq_date == "")
                {
                    vreq_date = null;
                }
                

                if (vbuild_type == null || vbuild_type == "")
                {
                    throw new Exception("โปรดระบุข้อมูล Build Type");
                }

                //query data
                string sql = "";
                sql += " select b.tran_date as jit_date, a.doc_no, a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by, sum(1) as conf_qty";
                sql += " from pd_mast a, pkg_barcode b";
                sql += " where a.doc_no = b.ref_pd_docno";
                sql += " and a.pd_entity = b.entity";
                sql += " and a.wc_code = b.wc_code";
                sql += " and trunc(a.doc_date) = nvl(to_date(:pReqDate,'dd/mm/yyyy'),trunc(a.doc_date))";
                sql += " and a.pd_entity = :pEntity";
                sql += " and((a.build_type = :pBuildType) or(a.build_type is null))";
                sql += " and a.doc_no = nvl(:pDocNo,a.doc_no)";
                sql += " and a.doc_status = nvl(:pDocStatus,'PAL')";
                //sql += " and a.doc_status = 'PAL'";
                sql += " group by b.tran_date, a.doc_no,  a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by";

                List<ProductionRecView> productionRecView = ctx.Database.SqlQuery<ProductionRecView>
                                                        (sql, new OracleParameter("pReqDate", vreq_date),
                                                              new OracleParameter("pEntity", ventity),
                                                              new OracleParameter("pBuildType", ventity),
                                                              new OracleParameter("pDocNo", vdoc_no),
                                                              new OracleParameter("pDocStatus", vdoc_status)).ToList();

                if (productionRecView == null)
                {
                    throw new Exception("ไม่มีข้อมูล");
                }

                view.totalItem = productionRecView.Count;
                productionRecView = productionRecView.Skip(view.pageIndex * view.itemPerPage)
                                                   .Take(view.itemPerPage)
                                                   .ToList();
                int vTotal_rec_qty = 0;

                ////prepare model to modelView
                foreach (var i in productionRecView)
                {
                    vTotal_rec_qty += i.conf_qty;

                    //find wc_name
                    string sql1 = "select wc_tdesc from wc_mast where wc_code = :p_wcCode and  rownum = 1";
                    string wcName = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_wcCode", i.wc_code)).SingleOrDefault();
                    //************

                    view.recDetails.Add(new ModelViews.ProductionRecView()
                    {
                        jit_date = i.jit_date,
                        doc_no   = i.doc_no,
                        doc_code = i.doc_code,
                        wh_code = i.wh_code,
                        wc_code  = i.wc_code,
                        wc_name  = wcName,
                        gen_date = i.gen_date,
                        gen_by   = i.gen_by,
                        conf_qty = i.conf_qty,
                    });
                }

                view.total_rec_qty = vTotal_rec_qty;

                //return data to contoller
                return view;
            }
        }

        public ProductionRecDetailTotalView SearchProductionRecDetail(ProductionRecDetailSearchView model)
        {
            using (var ctx = new ConXContext())
            {

                //define model view
                ProductionRecDetailTotalView view = new ModelViews.ProductionRecDetailTotalView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    total_rec_qty = 0,
                    total_prod_item = 0,
                    totalItem = 0,

                    recDetails = new List<ModelViews.ProductionRecDetailView>()

                };

                string ventity     = model.entity_code;
                string vdoc_date   = model.doc_date;
                string vbuild_type = model.build_type;
                string vdoc_no     = model.doc_no;

                if (ventity == null || ventity == "")
                {
                    throw new Exception("โปรดระบุข้อมูล กลุ่มสินค้า");
                }

                if (vdoc_date == null || vdoc_date == "")
                {
                    throw new Exception("โปรดระบุข้อมูล วันที่เอกสาร");
                }

                if (vbuild_type == null || vbuild_type == "")
                {
                    throw new Exception("โปรดระบุข้อมูล Build Type");
                }

                if (vdoc_no == null || vdoc_no == "")
                {
                    throw new Exception("โปรดระบุข้อมูล เลขที่เอกสาร POR");
                }

                //query data
                string sql = "";
                sql += " select a.doc_no, b.wc_code, a.prod_code, c.prod_tname, a.qty_pdt, a.por_no, c.uom_code,";
                sql += " (select tran_date from pkg_barcode where por_no = a.por_no and ref_pd_docno = a.doc_no and rownum = 1) as mps_date";
                sql += " from pd_det a, pd_mast b, product c";
                sql += " where a.pd_entity = b.pd_entity";
                sql += " and a.doc_no = b.doc_no";
                sql += " and a.prod_code = c.prod_code";
                sql += " and trunc(b.doc_date) = nvl(to_date(:pDocDate,'dd/mm/yyyy'),trunc(b.doc_date))";
                sql += " and b.pd_entity = :pEntity";
                sql += " and a.doc_no = :pDocNo";
                sql += " and((b.build_type = :pBuildType) or(b.build_type is null))";
                sql += " and b.doc_status = 'PAL'";

                List<ProductionRecDetailView> productionRecDetailView = ctx.Database.SqlQuery<ProductionRecDetailView>
                                                        (sql, new OracleParameter("pDocDate", vdoc_date),
                                                              new OracleParameter("pEntity", ventity),
                                                              new OracleParameter("pDocNo", vdoc_no),
                                                              new OracleParameter("pBuildType", vbuild_type)).ToList();


                if (productionRecDetailView == null)
                {
                    throw new Exception("ไม่มีข้อมูล");
                }

                view.totalItem = productionRecDetailView.Count;
                productionRecDetailView = productionRecDetailView.Skip(view.pageIndex * view.itemPerPage)
                                                   .Take(view.itemPerPage)
                                                   .ToList();
                int vTotal_rec_qty   = 0;
                int vTotal_prod_item = 0;

                ////prepare model to modelView
                foreach (var i in productionRecDetailView)
                {
                    vTotal_rec_qty   += i.qty_pdt;
                    vTotal_prod_item += 1;

                    //find barcode set
                    // Find Sub Product    
                    List<ProductionRecSetDetailView> setNoDetail = new List<ProductionRecSetDetailView>();

                    string sql2 = "";
                    sql2 += " select pkg_barcode_set, sum(1) as confirm_qty";
                    sql2 += " from pkg_barcode";
                    sql2 += " where entity = :pEntity";
                    sql2 += " and trunc(tran_date) = trunc(:pMpsDate)";
                    sql2 += " and wc_code = :pWcCode";
                    sql2 += " and prod_code = :pProdCode";
                    sql2 += " and ref_pd_docno = :pDocNo";
                    sql2 += " group by pkg_barcode_set";
                    sql2 += " order by pkg_barcode_set";

                    List<ProductionRecSetDetailView> productionRecSetDetailView = ctx.Database.SqlQuery<ProductionRecSetDetailView>
                                                        (sql2, new OracleParameter("pEntity", ventity),
                                                               new OracleParameter("pMpsDate", i.mps_date),
                                                               new OracleParameter("pWcCode", i.wc_code),
                                                               new OracleParameter("pProdCode", i.prod_code),
                                                               new OracleParameter("pDocNo", i.doc_no)).ToList();
                    
                    ////prepare model to modelView
                    foreach (var j in productionRecSetDetailView)
                    {
                        ProductionRecSetDetailView setDetail = new ProductionRecSetDetailView()
                        {
                            pkg_barcode_set = j.pkg_barcode_set,
                            confirm_qty = j.confirm_qty
                        };
                        setNoDetail.Add(setDetail);

                    }   
                    // End find barcode set

                    view.recDetails.Add(new ModelViews.ProductionRecDetailView()
                    {
                        doc_no = i.doc_no,
                        wc_code = i.wc_code,
                        prod_code = i.prod_code,
                        prod_tname = i.prod_tname,
                        qty_pdt = i.qty_pdt,
                        por_no = i.por_no,
                        uom_code = i.uom_code,
                        mps_date = i.mps_date,
                        setDetails = setNoDetail
                    });

                }

                view.total_rec_qty   = vTotal_rec_qty;
                view.total_prod_item = vTotal_prod_item;

                //return data to contoller
                return view;

            }
        }

        public ProductionRecTotalView SearchPutAwayWaiting(ProductionRecSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                //define model view
                ProductionRecTotalView view = new ModelViews.ProductionRecTotalView()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,
                    total_rec_qty = 0,

                    recDetails = new List<ModelViews.ProductionRecView>()
                };

                string ventity = model.entity_code;
                string vreq_date = model.doc_date;
                string vbuild_type = model.build_type;
                string vdoc_no = model.doc_no;
                string vdoc_status = model.doc_status;


                if (ventity == null || ventity == "")
                {
                    throw new Exception("โปรดระบุข้อมูล กลุ่มสินค้า");
                }

                if (vreq_date == null || vreq_date == "")
                {
                    vreq_date = null;
                }


                if (vbuild_type == null || vbuild_type == "")
                {
                    throw new Exception("โปรดระบุข้อมูล Build Type");
                }

                //query data
                string sql = "";
                     sql += " select a.doc_date as jit_date, a.doc_no, a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by, sum(nvl(b.qty_pdt,0)) as conf_qty";
                     sql += " from pd_mast a, pd_det b";
                     sql += " where a.pd_entity = b.pd_entity";
                     sql += " and   a.doc_no    = b.doc_no";
                     sql += " and trunc(a.doc_date) = nvl(to_date(:pReqDate,'dd/mm/yyyy'),trunc(a.doc_date))";
                     sql += " and a.pd_entity = :pEntity";
                     sql += " and((a.build_type = :pBuildType) or(a.build_type is null))";
                     sql += " and a.doc_no = nvl(:pDocNo,a.doc_no)";
                     sql += " and a.doc_status = 'APV'";

                     if (vdoc_status == "APV")
                     {

                         sql += "    and a.doc_no in (select doc_no";
                         sql += "                        from whtran_mast";
                         sql += "                        where ic_entity = a.pd_entity";
                         sql += "                        and trans_code  = 'PTW'";
                         sql += "                        and doc_status  = 'APV')";

                     }
                     else
                     {
                         sql += "    and a.doc_no not in (select doc_no";
                         sql += "                      from whtran_mast";
                         sql += "                      where ic_entity = a.pd_entity";
                         sql += "                      and trans_code  = 'PTW'";
                         sql += "                      and doc_status  = 'APV')";

                     }

                     sql += " group by a.doc_date, a.doc_no,  a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by";
                     

              /*  sql += " select a.doc_date as jit_date, a.doc_no, a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by, sum(nvl(b.qty_pdt, 0)) as conf_qty";
                sql += " from pd_mast a, pd_det b";
                sql += " where a.pd_entity = b.pd_entity";
                sql += " and a.doc_no = b.doc_no";
                sql += " and a.pd_entity = :pEntity";
                sql += " and trunc(a.doc_date) = nvl(to_date(:pReqDate, 'dd/mm/yyyy'), trunc(a.doc_date))";
                sql += " and((a.build_type = :pBuildType) or(a.build_type is null))";
                sql += " and a.doc_no = nvl(:pDocNo, a.doc_no)";
                sql += " and a.doc_status = 'APV'";
                sql += " and a.doc_no not in (select doc_no";
                sql += "                         from whtran_mast";
                sql += "                         where ic_entity = a.pd_entity";
                sql += "                         and trans_code = 'PTW'";
                sql += "                         and doc_status = 'APV')";
                sql += " group by a.doc_date, a.doc_no,  a.doc_code, a.wh_code, a.wc_code, a.gen_date, a.gen_by";
                */

                List<ProductionRecView> productionRecView = ctx.Database.SqlQuery<ProductionRecView>
                                                        (sql, new OracleParameter("pReqDate", vreq_date),
                                                              new OracleParameter("pEntity", ventity),
                                                              new OracleParameter("pBuildType", vbuild_type),
                                                              new OracleParameter("pDocNo", vdoc_no)).ToList();
                                                              //new OracleParameter("pDocStatus", vdoc_status)).ToList();

                if (productionRecView == null)
                {
                    throw new Exception("ไม่มีข้อมูล");
                }

                view.totalItem = productionRecView.Count;
                productionRecView = productionRecView.Skip(view.pageIndex * view.itemPerPage)
                                                   .Take(view.itemPerPage)
                                                   .ToList();
                int vTotal_rec_qty = 0;
                int vTotal_ptw_qty = 0;
                int vTotal_net_qty = 0;

                ////prepare model to modelView
                foreach (var i in productionRecView)
                {
                    vTotal_rec_qty += i.conf_qty;

                    //find wc_name
                    string sql1 = "select wc_tdesc from wc_mast where wc_code = :p_wcCode and  rownum = 1";
                    string wcName = ctx.Database.SqlQuery<string>(sql1, new OracleParameter("p_wcCode", i.wc_code)).SingleOrDefault();
                    //************

                    //find ptw_qty
                   string sql_ptwQty = "select nvl(sum(nvl(qty,0)),0) ptw_qty from whtran_det where ic_entity = :pic_entity and trans_code = 'PTW' and doc_no = :pdoc_no and doc_code = :pdoc_code";
                   int vPtwQty = ctx.Database.SqlQuery<int>(sql_ptwQty, new OracleParameter("pic_entity", ventity),
                                                                   new OracleParameter("pdoc_no", i.doc_no),
                                                                   new OracleParameter("pdoc_code", i.doc_code)).SingleOrDefault();


                    vTotal_ptw_qty += vPtwQty;
                    vTotal_net_qty += (i.conf_qty - vPtwQty);

                    view.recDetails.Add(new ModelViews.ProductionRecView()
                    {
                        jit_date = i.jit_date,
                        doc_no = i.doc_no,
                        doc_code = i.doc_code,
                        wh_code = i.wh_code,
                        wc_code = i.wc_code,
                        wc_name = wcName,
                        gen_date = i.gen_date,
                        gen_by = i.gen_by,
                        conf_qty = i.conf_qty,
                        ptw_qty  = vPtwQty,
                        net_qty  = (i.conf_qty - vPtwQty)
                    });
                }

                view.total_rec_qty = vTotal_rec_qty;
                view.total_ptw_qty = vTotal_ptw_qty;
                view.total_net_qty = vTotal_net_qty;

                //return data to contoller
                return view;
            }
        }
    }
}