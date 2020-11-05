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

        [Route("scan-apvsend/postSearchScanSend")]
        public HttpResponseMessage postSearchDataScanSend(ScanApproveSendSearchView model)
        {
            try
            {
                var result = apvsendService.SearchDataScanSend(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-apvsend/postScanApvSendNew")]
        public HttpResponseMessage postScanApvSendNew(ScanApproveAddView model)
        {
            try
            {
                var result = apvsendService.ScanApvSendNew(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-apvsend/postScanApvSendAdd")]
        public HttpResponseMessage postScanApvSendAdd(ScanApproveAddView model)
        {
            try
            {
                var result = apvsendService.ScanApvSendAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-apvsend/postScanApvSendCancel")]
        public HttpResponseMessage postScanApvSendCancel(ScanApproveAddView model)
        {
            try
            {
                var result = apvsendService.ScanApvSendCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-apvsend/getProductDetail/{entity}/{doc_no}/{wc_code}")]
        public HttpResponseMessage getProductDetail(string entity, string doc_no , string wc_code)
        {
            try
            {
                var result = apvsendService.getProductDetail(entity, doc_no , wc_code);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
    }
}
