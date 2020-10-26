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
    public class ScanApproveSendService : IScanApproveSendService
    {
        public ScanApproveSendView SearchDataScanSend(ScanApproveSendSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string ventity = model.entity;
                string vbuild_type = model.build_type;
                string vuser = model.user_id;
                string vdoc_no = model.doc_no;
                string vfin_date = model.fin_date;
                string vsend_type = model.send_type;
                int total_qty = 0;
                int total_set = 0;



                //define model view
                ScanApproveSendView view = new ModelViews.ScanApproveSendView()
                {
                    entity = ventity,
                    build_type = vbuild_type,
                    user_id = vuser,
                    datas = new List<ModelViews.ScanApproveSendDataView>()
                };

                //query data
                if (model.build_type == "HMJIT")
                {
                    if(vsend_type == "WAIT")
                    {
                        string sql = "select entity , req_date , pdjit_grp , wc_code , doc_no , sum(qty_pdt) tot_qty , " +
                            "(select count(distinct a.pkg_barcode_set) " +
                            "from pkg_barcode a " +
                            "where a.entity = :p_entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code " +
                            "and a.pkg_barcode_set is not null ) set_qty " +
                        "from mps_det_wc " +
                        "where entity = :p_entity2 " +
                        "and mps_st = 'Y' " +
                        "and (build_type in ('HMJIT','') or build_type is null) " +
                        "and doc_no is null " +
                        "and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') " +
                        "and wc_code in (select dept_code from auth_function where function_id = 'PDOPTHM' and doc_code = 'Y' and user_id = :p_user_id) " +
                        "group by entity , req_date , pdjit_grp , wc_code , doc_no";

                        List<ScanApproveSendDataView> send = ctx.Database.SqlQuery<ScanApproveSendDataView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_entity2", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_user_id", vuser)).ToList();

                        foreach (var x in send)
                        {
                            view.datas.Add(new ModelViews.ScanApproveSendDataView()
                            {
                                req_date = x.req_date,
                                pdjit_grp = x.pdjit_grp,
                                wc_code = x.wc_code,
                                doc_no = x.doc_no,
                                set_qty = x.set_qty,
                                tot_qty = x.tot_qty,
                                status = "รอส่งมอบ"

                            });
                            total_qty = total_qty + x.tot_qty;
                            total_set = total_set + x.set_qty;
                        }

                        view.total_qty = total_qty;
                        view.total_set = total_set;

                    }
                    else
                    {
                        string sql = "select entity , req_date , pdjit_grp , wc_code , doc_no , " +
                            "(select sum(1) from pkg_barcode " +
                            "where a.ref_pd_docno = doc_no" +
                            "and a.entity = entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code) tot_qty " +
                            "(select count(distinct a.pkg_barcode_set) " +
                            "from pkg_barcode a " +
                            "where a.ref_pd_docno = doc_no " +
                            "and a.entity = entity " +
                            "and a.tran_date = req_date " +
                            "and a.wc_code = wc_code) tot_set " +
                        "from mps_det_wc " +
                        "where entity = :p_entity " +
                        "and mps_st = 'Y' " +
                        "and (build_type in ('HMJIT','') or build_type is null) " +
                        "and trunc(fin_date) = to_date(:p_fin_date,'dd/mm/yyyy') " +
                        "and wc_code in (select dept_code from auth_function where function_id = 'PDOPTHM' and doc_code = 'Y' and user_id = :p_user_id) " +
                        "and (entity , req_date , wc_code , prod_code , doc_no) in " +
                            "(select b.entity , b.tran_date , b.wc_code , b.prod_code , b.doc_no " +
                            "from pkg_barcode b " +
                            "where b.wc_code = wc_code " +
                            "and b.tran_date = req_date " +
                            "and b.prod_code = prod_code " +
                            "and (ref_pd_docno is not null or pd_ref_docno <> '')) " +
                        "group by entity , req_date , pdjit_grp , wc_code , doc_no";

                        List<ScanApproveSendDataView> send = ctx.Database.SqlQuery<ScanApproveSendDataView>(sql, new OracleParameter("p_entity", ventity), new OracleParameter("p_fin_date", vfin_date), new OracleParameter("p_user_id", vuser)).ToList();

                        foreach (var x in send)
                        {
                            view.datas.Add(new ModelViews.ScanApproveSendDataView()
                            {
                                req_date = x.req_date,
                                pdjit_grp = x.pdjit_grp,
                                wc_code = x.wc_code,
                                doc_no = x.doc_no,
                                set_qty = x.set_qty,
                                tot_qty = x.tot_qty,
                                status = "ส่งมอบแล้ว"

                            });
                            total_qty = total_qty + x.tot_qty;
                            total_set = total_set + x.set_qty;
                        }

                        view.total_qty = total_qty;
                        view.total_set = total_set;
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