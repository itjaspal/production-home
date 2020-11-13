using api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Services;
using api.ModelViews;

namespace api.Controllers
{
    public class JobOperationStockController : ApiController
    {
        IJobOperationStockService jobService;

        public JobOperationStockController()
        {

            jobService = new JobOperationStockSevice();
        }

        [Route("job-operation-stock/postSearchDataPlan")]
        public HttpResponseMessage postSearchDataPlan(JobOperationStockSearchView model)
        {
            try
            {
                var result = jobService.SearchDataPlan(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-operation-stock/postSearchDataFin")]
        public HttpResponseMessage postSearchDataFin(JobOperationStockSearchView model)
        {
            try
            {
                var result = jobService.SearchDataFin(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-operation-stock/postSearchDataDefect")]
        public HttpResponseMessage postSearchDataDefect(JobOperationStockSearchView model)
        {
            try
            {
                var result = jobService.SearchDataDefect(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
