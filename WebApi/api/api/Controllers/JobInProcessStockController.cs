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
    public class JobInProcessStockController : ApiController
    {
        IJobInProcessStockService jobService;

        public JobInProcessStockController()
        {

            jobService = new JobInProcessStockService();
        }

        [Route("job-inprocess-stock/postSearchJobInPorcessPlan")]
        public HttpResponseMessage postSearchDataPlan(JobOperationStockSearchView model)
        {
            try
            {
                var result = jobService.SearchJobInPorcessPlan(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postSearchJobInPorcessFin")]
        public HttpResponseMessage postSearchDataFin(JobOperationStockSearchView model)
        {
            try
            {
                var result = jobService.SearchJobInPorcessFin(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postScanAdd")]
        public HttpResponseMessage postScanAdd(JobInProcessStockSearchView model)
        {
            try
            {
                var result = jobService.ScanAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postEntryAdd")]
        public HttpResponseMessage postEntryAdd(JobInProcessStockSearchView model)
        {
            try
            {
                var result = jobService.EntryAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postCancel")]
        public HttpResponseMessage postCancel(JobInProcessStockView model)
        {
            try
            {

                jobService.Cancel(model);

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

        [Route("job-inprocess-stock/postGetSubProduct")]
        public HttpResponseMessage postGetProduct(ProductSubSearchView model)
        {
            try
            {
                var result = jobService.getSubProduct(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postGetSubProductCancel")]
        public HttpResponseMessage postGetProductCancel(ProductSubSearchView model)
        {
            try
            {
                var result = jobService.getSubProductCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-inprocess-stock/postEntryCancel")]
        public HttpResponseMessage postEntryCancel(JobInProcessStockSearchView model)
        {
            try
            {
                var result = jobService.EntryCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
