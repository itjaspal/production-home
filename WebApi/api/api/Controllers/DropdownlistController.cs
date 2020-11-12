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

        [Route("dropdownlist/getDdlWCPtwByUser/{entity}/{user}")]
        public HttpResponseMessage getDdlWCPtwByUser(string entity, string user)
        {
            try
            {
                var result = ddlSvc.GetDdlWCPutaWay(entity, user);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlPutAwayWHMast")]
        public HttpResponseMessage getDdlPutAwayWHMast()
        {
            try
            {
                var result = ddlSvc.GetDdlPutAwayWHMast();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlPtwSetNoList/{entity}/{doc_code}/{doc_no}")]
        public HttpResponseMessage getDdlPtwSetNoList(string entity, string doc_code, string doc_no)
        {
            try
            {
                var result = ddlSvc.GetDdlPtwSetNoList(entity, doc_code, doc_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("dropdownlist/getDdlPtwProdList/{entity}/{doc_code}/{doc_no}")]
        public HttpResponseMessage getDdlPtwProdList(string entity, string doc_code, string doc_no)
        {
            try
            {
                var result = ddlSvc.GetDdlPtwProdList(entity, doc_code, doc_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
