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
    public class ScanSendController : ApiController
    {
        IScanSendService sendService;

        public ScanSendController()
        {
            sendService = new ScanSendService();
        }

        [Route("scan-send/postScanSendAdd")]
        public HttpResponseMessage postSearchScanAdd(ScanSendSearchView model)
        {
            try
            {
                var result = sendService.ScanSendAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-send/postSearchSetNo")]
        public HttpResponseMessage postSearchSetNo(SetNoSearchView model)
        {
            try
            {
                var result = sendService.getSetNo(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
