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

        [Route("scan-send/postPrintSticker")]
        public HttpResponseMessage postPrintSticker(ScanSendView model)
        {
            try
            {
                sendService.PringSticker(model);

                return Request.CreateResponse(HttpStatusCode.OK, "ส่งข้อมูลไปยังเครื่องพิมพ์เรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-send/postRePrintSticker")]
        public HttpResponseMessage postRePrintSticker(PringSetNoView model)
        {
            try
            {
                sendService.RePringSticker(model);

                return Request.CreateResponse(HttpStatusCode.OK, "ส่งข้อมูลไปยังเครื่องพิมพ์เรียบร้อยแล้ว");
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

        [Route("scan-send/post/Delete")]
        public HttpResponseMessage postDelete(ScanSendView model)
        {
            try
            {

                sendService.delete(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "ยกเลิกบันทึกส่งมอบสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-send/postGetScanQty")]
        public HttpResponseMessage postGetScanQty(ScanSendView model)
        {
            try
            {
                var result = sendService.getScanQty(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
