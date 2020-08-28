using api.Interfaces;
using api.ModelViews;
using api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class MenuController : ApiController
    {
        IMenuService menuSvc;

        public MenuController()
        {
            menuSvc = new MenuService();
        }

        //[POST("postSearch")]
        [Route("master-menu/postSearch")]
        public HttpResponseMessage postSearch(MasterMenuSearchView model)
        {
            try
            {


                var result = menuSvc.Search(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("master-menu/getInfo/{code}")]
        public HttpResponseMessage getInfo(string code)
        {
            try
            {
                var result = menuSvc.GetInfo(code);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        

        //[POST("postUpdate")]
        [Route("master-menu/postUpdate")]
        public HttpResponseMessage postUpdate(MasterMenuView model)
        {
            try
            {

                //check dupplicate Code
                //var isDupplicate = menuGroupSvc.CheckDupplicate(model.menuFunctionGroupId);
                //if (isDupplicate)
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("รหัสกลุ่มเมนู {0} มีอยู่ในระบบแล้ว", model.menuFunctionGroupId));
                //}

                menuSvc.Update(model);

                return Request.CreateResponse(HttpStatusCode.OK, "บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
