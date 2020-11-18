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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
