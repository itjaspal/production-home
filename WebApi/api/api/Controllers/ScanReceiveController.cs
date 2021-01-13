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
    public class ScanReceiveController : ApiController
    {
        IScanReceiveService receiveService;

        public ScanReceiveController()
        {
            receiveService = new ScanReceiveService();
        }

        [Route("scan-receive/postSearchDataScanReceive")]
        public HttpResponseMessage postSearchDataScanSend(ScanReceiveDataSearchView model)
        {
            try
            {
                var result = receiveService.SearchDataScanReceive(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-receive/postScanReceiveAdd")]
        public HttpResponseMessage postScanReceiveAdd(ScanReceiveSearchView model)
        {
            try
            {
                var result = receiveService.ScanReceiveAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-receive/postScanReceiveCancel")]
        public HttpResponseMessage postScanReceiveCancel(ScanReceiveView model)
        {
            try
            {

                receiveService.ScanReceiveCancel(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "ยกเลิกบันทึกรับมอบสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-receive/getProductDetail/{entity}/{doc_no}")]
        public HttpResponseMessage getProductDetail(string entity, string doc_no)
        {
            try
            {
                var result = receiveService.getProductDetail(entity, doc_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
            
        }

        [Route("scan-receive/getSendMail/{entity}/{doc_no}")]
        public HttpResponseMessage getSendMail(string entity ,string doc_no)
        {
            try
            {

                receiveService.SendMail(entity,doc_no);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "ส่ง Mail สำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-receive/postConfirmStock")]
        public HttpResponseMessage postApproveReceive(ConfirmStockDataView model)
        {
            try
            {
                receiveService.ConfirmStock(model);
                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "Update Stock เรียบร้อยแล้ว"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
                //return Request.CreateResponse(HttpStatusCode.OK, "ยืนยันรับมอบเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

    }
}
