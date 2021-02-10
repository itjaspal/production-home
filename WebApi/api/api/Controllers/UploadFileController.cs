using api.Interfaces;
using api.ModelViews;
using api.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
    public class UploadFileController : ApiController
    {
        IUploadFileService uploadSvc;

        public UploadFileController()
        {
            uploadSvc = new UploadFileService();
        }

        [Route("upload-file/postSearchUploadFile")]
        public HttpResponseMessage postSearchUploadFile(UploadFileSearchView model)
        {
            try
            {


                var result = uploadSvc.SearchUploadFile(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("upload-file/postCreate")]
        public HttpResponseMessage postCreate()
        {
            try
            {

                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                string path_sd = ConfigurationManager.AppSettings["upload.sd"];
                string path_spec = ConfigurationManager.AppSettings["upload.spec"];
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string path = "";

                //check exist folder
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //check exist folder year
                //path += "\\" + year;
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //check exist folder month
                //path += "\\" + month;
                //if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                UploadFileView model = new UploadFileView();
                foreach (string key in files)
                {
                    System.Web.HttpPostedFile htf = files[key];
                    string pddsgn_code = HttpContext.Current.Request.Params["pddsgn_code"];

                    string type = HttpContext.Current.Request.Params["type"];
                    string dsgn_no = HttpContext.Current.Request.Params["dsgn_no"];
                    string dept_code = HttpContext.Current.Request.Params["dept_code"];
                    //rename
                    string[] fileNameOldArr = htf.FileName.Split('.');
                    string fileNameOld = htf.FileName;
                    //string fileNameNew = DateTime.Now.ToString("ddMMyyyy-HHmmss-fff", new CultureInfo("en-US").DateTimeFormat);
                    string fileNameNew = pddsgn_code + "_" +dsgn_no;
                    //fileNameNew = string.Format("{0}_{1}.{2}", fileNameOldArr[0], fileNameNew, fileNameOldArr[fileNameOldArr.Length - 1]);
                    fileNameNew = string.Format("{0}.{1}", fileNameNew, fileNameOldArr[fileNameOldArr.Length - 1]);

                    if(type == "SD")
                    {
                        path = path_sd;
                    }
                    else
                    {
                        path = path_spec;
                    }

                    string physicalPath = path + "\\" + fileNameNew;
                    htf.SaveAs(physicalPath);

                    //string pddsgn_code = HttpContext.Current.Request.Params["pddsgn_code"];

                    //string type = HttpContext.Current.Request.Params["type"];


                    model.pddsgn_code = pddsgn_code;
                    model.type = type;
                    model.dsgn_no = dsgn_no;
                    model.dept_code = dept_code;


                    //model.file_path = string.Format("{0}/{1}/{2}", year, month, fileNameNew);
                    model.file_path = path + "\\"+ string.Format("{0}",fileNameNew);
                    model.file_name = string.Format("{0}", fileNameNew);

                }

                var isDupplicate = uploadSvc.CheckDupplicate(model.pddsgn_code,model.dsgn_no);
                if (isDupplicate)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Design Code {0} มีอยู่ในระบบแล้ว", model.pddsgn_code));
                }

                uploadSvc.Create(model);

                return Request.CreateResponse(HttpStatusCode.OK, "บันทึกข้อมูลสำเร็จ");



            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }

            
        }

        
        [Route("upload-file/postUpdate")]
        public HttpResponseMessage postUpdate()
        {
            try
            {

                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                string path_sd = ConfigurationManager.AppSettings["upload.sd"];
                string path_spec = ConfigurationManager.AppSettings["upload.spec"];
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string path = "";

                
                UploadFileView model = new UploadFileView();
                foreach (string key in files)
                {
                    System.Web.HttpPostedFile htf = files[key];
                    string pddsgn_code = HttpContext.Current.Request.Params["pddsgn_code"];
                    string type = HttpContext.Current.Request.Params["type"];
                    string dsgn_no = HttpContext.Current.Request.Params["dsgn_no"];
                    string dept_code = HttpContext.Current.Request.Params["dept_code"];
                    //rename
                    string[] fileNameOldArr = htf.FileName.Split('.');
                    string fileNameOld = htf.FileName;
                    
                    string fileNameNew = pddsgn_code + "_" + dsgn_no;
                    
                    fileNameNew = string.Format("{0}.{1}", fileNameNew, fileNameOldArr[fileNameOldArr.Length - 1]);

                    if (type == "SD")
                    {
                        path = path_sd;
                    }
                    else
                    {
                        path = path_spec;
                    }

                    string physicalPath = path + "\\" + fileNameNew;
                    htf.SaveAs(physicalPath);

                  
                    model.pddsgn_code = pddsgn_code;
                    model.type = type;
                    model.dsgn_no = dsgn_no;
                    model.dept_code = dept_code;

                    model.file_path = path + "\\" + string.Format("{0}", fileNameNew);
                    model.file_name = string.Format("{0}", fileNameNew);

                }

               
                uploadSvc.Update(model);

                return Request.CreateResponse(HttpStatusCode.OK, "บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("upload-file/getInfo/{code}/{dsgn_no}")]
        public HttpResponseMessage getInfo(string code , string dsgn_no)
        {
            try
            {
                var result = uploadSvc.GetInfo(code , dsgn_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("upload-file/post/Delete")]
        public HttpResponseMessage postDelete(UploadFileView model)
        {
            try
            {

                uploadSvc.Delete(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "ลบข้อมูลสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
