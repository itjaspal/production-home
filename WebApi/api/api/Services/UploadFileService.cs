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
    public class UploadFileService : IUploadFileService
    {
        public bool CheckDupplicate(string code , string dsgn_no)
        {
            using (var ctx = new ConXContext())
            {
                bool isDup = false;

                string sql = "select pddsgn_code from files_ctl where pddsgn_code = :p_code and dsgn_no = :p_dsgn_no";
                string design = ctx.Database.SqlQuery<string>(sql, new OracleParameter("p_code", code) , new OracleParameter("p_dsgn_no", dsgn_no)).SingleOrDefault();
                
                isDup = design != null;

                return isDup;
            }
        }

        public void Create(UploadFileView model)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    // Udpaet PKG_BARCODE
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_pddsgn_code", model.pddsgn_code),
                        new OracleParameter("p_type", model.type),
                        new OracleParameter("p_file_path", model.file_path),
                        new OracleParameter("p_file_name", model.file_name),
                        new OracleParameter("p_dsgn_no", model.dsgn_no),
                        new OracleParameter("p_dept_code", model.dept_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "insert into files_ctl (pddsgn_code , system , physical_path , file_name , file_type , file_grp , dsgn_no , dept_code) values (:p_pddsgn_code , 'HMPROD' , :p_file_path , :p_file_name , 'PDF' , :p_type , :p_dsgn_no , :p_dept_code)";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }
            }
        }

        public void Delete(UploadFileView model)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();


                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_pddsgn_code", model.pddsgn_code),
                        new OracleParameter("p_dsgn_no", model.dsgn_no),

                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "delete files_ctl where pddsgn_code = :p_pddsgn_code and dsgn_no = :p_dsgn_no";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }
            }
        }

        public UploadFileView GetInfo(string code , string dsgn_no)
        {
            using (var ctx = new ConXContext())
            {
                string sql = "select a.pddsgn_code , a.file_grp type , a.physical_path file_path , a.file_name , b.pddsgn_tname pddsgn_name , a.dsgn_no , a.dept_code from files_ctl a , pddsgn_mast b where a.pddsgn_code=b.pddsgn_code and a.pddsgn_code = :p_code and dsgn_no = :p_dsgn_no";
                UploadFileView upload = ctx.Database.SqlQuery<UploadFileView>(sql, new OracleParameter("p_code", code), new OracleParameter("p_dsgn_no", dsgn_no)).SingleOrDefault();


                return new UploadFileView
                {
                    pddsgn_code = upload.pddsgn_code,
                    pddsgn_name = upload.pddsgn_name,
                    type = upload.type,
                    file_path = upload.file_path,
                    file_name = upload.file_name,
                    dsgn_no = upload.dsgn_no,
                    dept_code = upload.dept_code
                    
                };
            }
        }

        public CommonSearchView<UploadFileView> SearchUploadFile(UploadFileSearchView model)
        {
            using (var ctx = new ConXContext())
            {
                string urlPrefix_sd = ConfigurationManager.AppSettings["upload.urlPrefixSd"];
                string urlPrefix_spec = ConfigurationManager.AppSettings["upload.urlPrefixSpec"];

                string urlPrefix = "";

                //define model view
                CommonSearchView<UploadFileView> view = new ModelViews.CommonSearchView<ModelViews.UploadFileView>()
                {
                    pageIndex = model.pageIndex - 1,
                    itemPerPage = model.itemPerPage,
                    totalItem = 0,

                    datas = new List<ModelViews.UploadFileView>()
                };

                //query data
                string sql1 = "select a.pddsgn_code , b.pddsgn_tname pddsgn_name , a.file_grp type , a.physical_path file_path , a.file_name , a.dsgn_no , a.dept_code from files_ctl a , pddsgn_mast b where a.pddsgn_code = b.pddsgn_code and a.pddsgn_code like :p_pddsgn_code and a.file_grp like :p_type order by a.pddsgn_code";
                List<UploadFileView> upload = ctx.Database.SqlQuery<UploadFileView>(sql1, new OracleParameter("p_pddsgn_code", model.pddsgn_code+"%") ,new OracleParameter("p_type", model.type+"%")).ToList();
                
                //count , select data from pageIndex, itemPerPage
                view.totalItem = upload.Count;
                upload = upload.Skip(view.pageIndex * view.itemPerPage)
                    .Take(view.itemPerPage)
                    .ToList();

                //prepare model to modelView
                foreach (var i in upload)
                {
                    if(i.type == "SD")
                    {
                        urlPrefix = urlPrefix_sd;
                    }
                    else
                    {
                        urlPrefix = urlPrefix_spec;
                    }

                    view.datas.Add(new ModelViews.UploadFileView()
                    {
                        pddsgn_code = i.pddsgn_code,
                        pddsgn_name = i.pddsgn_name,
                        type = i.type == "SD" ? "SD" : "Design Spec",
                        file_path = i.file_path,
                        file_name = i.file_name,
                        fullPath = urlPrefix + i.file_name,
                        dept_code = i.dept_code,
                        dsgn_no = i.dsgn_no

                    });
                }

                //return data to contoller
                return view;
            }
        }

        public void Update(UploadFileView model)
        {
            using (var ctx = new ConXContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string strConn = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                    var dataConn = new OracleConnectionStringBuilder(strConn);
                    OracleConnection conn = new OracleConnection(dataConn.ToString());

                    conn.Open();

                    
                    OracleCommand oraCommand = conn.CreateCommand();
                    OracleParameter[] param = new OracleParameter[]
                    {
                        new OracleParameter("p_pddsgn_code", model.pddsgn_code),
                        new OracleParameter("p_type", model.type),
                        new OracleParameter("p_file_path", model.file_path),
                        //new OracleParameter("p_file_name", model.file_name)
                        new OracleParameter("p_dsgn_no", model.dsgn_no),
                        new OracleParameter("p_dept_code", model.dept_code)
                    };

                    oraCommand.BindByName = true;
                    oraCommand.Parameters.AddRange(param);
                    oraCommand.CommandText = "update files_ctl set file_grp = :p_type , physical_path = :p_file_path where pddsgn_code = :p_pddsgn_code";


                    oraCommand.ExecuteNonQuery();
                    conn.Close();


                    scope.Complete();
                }
            }
        }
    }
}