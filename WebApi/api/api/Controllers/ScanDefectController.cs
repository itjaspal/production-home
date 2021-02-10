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
    public class ScanDefectController : ApiController
    {
        IScanDefectService defectService;

        public ScanDefectController()
        {

            defectService = new ScanDefectService();
        }

       

        [Route("scan-defect/postScanAdd")]
        public HttpResponseMessage postScanAdd(ScanDefectSearchView model)
        {
            try
            {
                var result = defectService.ScanAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-defect/postEntryAdd")]
        public HttpResponseMessage postEntryAdd(ScanDefectSearchView model)
        {
            try
            {
                var result = defectService.EntryAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-defect/postCancel")]
        public HttpResponseMessage postCancel(ScanDefectView model)
        {
            try
            {

                defectService.Cancel(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "ยกเลิกบันทึกผลผลิตสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-defect/postGetSubProduct")]
        public HttpResponseMessage postGetProduct(DefectProductSubSearchView model)
        {
            try
            {
                var result = defectService.getSubProduct(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

       

        [Route("scan-defect/postEntryCancel")]
        public HttpResponseMessage postEntryCancel(ScanDefectSearchView model)
        {
            try
            {
                var result = defectService.EntryCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("scan-defect/postGetSubProductCancel")]
        public HttpResponseMessage postGetProductCancel(DefectProductSubSearchView model)
        {
            try
            {
                var result = defectService.getSubProductCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-defect/postEntryRemark")]
        public HttpResponseMessage postEntryRemark(DataQcCuttingView model)
        {
            try
            {

                defectService.EntryRemark(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "บันทึกหมายเหตุสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-defect/postGetSummaryDefect")]
        public HttpResponseMessage postGetSummaryDEfect(DefectProductSubSearchView model)
        {
            try
            {
                var result = defectService.getSummaryDefect(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("scan-defect/postGetOrderReq")]
        public HttpResponseMessage postGetOrderReq(OrderReqSearchView model)
        {
            try
            {
                var result = defectService.getOrderReq(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
