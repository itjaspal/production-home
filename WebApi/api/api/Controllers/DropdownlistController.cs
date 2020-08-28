using api.Interfaces;
using api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class DropdownlistController : ApiController
    {
        IDropdownlistService ddlSvc;
        public DropdownlistController()
        {
            ddlSvc = new DropdownlistService();
        }



        //[Route("dropdownlist/getDdlProductGroup")]
        //public HttpResponseMessage getDdlProductGroup()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductType();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductType")]
        //public HttpResponseMessage getDdlProductType()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductType();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductBrand")]
        //public HttpResponseMessage getDdlProductBrand()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductBrand();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductDesign")]
        //public HttpResponseMessage getDdlProductDesign()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductDesign();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductModel")]
        //public HttpResponseMessage getDdlProductModel()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductModel();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductSize")]
        //public HttpResponseMessage getDdlProductSize()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlProductSize();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        //[Route("dropdownlist/getDdlProductColor")]
        //public HttpResponseMessage getDdlProductColor()
        //{
        //    try
        //    {
        //        var result = ddlSvc.GetDdlMobilePrnt();

        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
        //    }
        //}

        [Route("dropdownlist/getDdlMobilePrnt")]
        public HttpResponseMessage getDdlMobilePrnt()
        {
            try
            {
                var result = ddlSvc.GetDdlMobilePrnt();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


    }
}
