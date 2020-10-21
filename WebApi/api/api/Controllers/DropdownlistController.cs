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

        [Route("dropdownlist/getDdlMobilePrntJIT")]
        public HttpResponseMessage getDdlMobilePrntJIT()
        {
            try
            {
                var result = ddlSvc.GetDdlMobilePrntJIT();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlMobilePrntSTK")]
        public HttpResponseMessage getDdlMobilePrntSTK()
        {
            try
            {
                var result = ddlSvc.GetDdlMobilePrntSTK();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlWCInprocess/{user}")]
        public HttpResponseMessage getDdlWCInprocess(string user)
        {
            try
            {
                var result = ddlSvc.GetDdlWCInprocess(user);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlWCSend/{user}")]
        public HttpResponseMessage getDdlWCSend(string user)
        {
            try
            {
                var result = ddlSvc.GetDdlWCSend(user);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


    }
}
