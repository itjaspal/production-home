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
    public class ScanApproveSendController : ApiController
    {
        IScanApproveSendService apvsendService;

        public ScanApproveSendController()
        {
            apvsendService = new ScanApproveSendService();
        }

        [Route("scanapvsend/postSearchScanSend")]
        public HttpResponseMessage postSearchDataScanSend(ScanApproveSendSearchView model)
        {
            try
            {
                var result = apvsendService.SearchDataScanSend(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
