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
    public class ScanPutAwayController : ApiController
    {
        IScanPutAwayService scanPutAwayService;

        public ScanPutAwayController()
        {

            scanPutAwayService = new ScanPutAwayService();
        }

        [Route("ScanPutAway/postScanPutAwayAdd")]
        public HttpResponseMessage postScanPutAwayAdd(ScanPutAwaySearchView model)
        {
            try
            {
                var result = scanPutAwayService.ScanPutAwayAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/postSearhPtwDetail")]
        public HttpResponseMessage postSearhPtwDetail(ScanPutAwayDetailSearchView model)
        {
            try
            {
                var result = scanPutAwayService.PostSearhPtwDetail(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/postScanPutAwayManualAdd")]
        public HttpResponseMessage postScanPutAwayManualAdd(ScanPutAwaySearchView model)
        {
            try
            {
                var result = scanPutAwayService.ScanPutAwayManualAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/postScanPutAwayCancel")]
        public HttpResponseMessage postScanPutAwayCancel(ScanPutAwayCancelSearchView model)
        {
            try
            {
                var result = scanPutAwayService.ScanPutAwayCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/getWhDefault/{ic_entity}")]
        public HttpResponseMessage getWhDefault(string ic_entity)
        {
            try
            {
                var result = scanPutAwayService.GetWhDefault(ic_entity);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/getDeptDefault/{ic_entity}/{user_id}")]
        public HttpResponseMessage getDeptDefault(string ic_entity, string user_id)
        {
            try
            {
                var result = scanPutAwayService.GetDeptDefault(ic_entity, user_id);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("ScanPutAway/getVerifyLoc/{loc_code}")]
        public HttpResponseMessage getVerifyLoc(string loc_code)
        {
            try
            {
                var result = scanPutAwayService.GetVerifyLoc(loc_code);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }


    }
}
