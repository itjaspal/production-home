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
    public class JobOperationController : ApiController
    {
        IJobOperationService jobService;

        public JobOperationController()
        {

            jobService = new JobOperationService();
        }

        [Route("job-operation/postSearchcurrent")]
        public HttpResponseMessage postSearchCurrent(JobOperationSearchView model)
        {
            try
            {
                var result = jobService.SearchDataCurrent(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-operation/postSearchpending")]
        public HttpResponseMessage postSearchPending(JobOperationSearchView model)
        {
            try
            {
                var result = jobService.SearchDataPending(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-operation/postSearchforward")]
        public HttpResponseMessage postSearchForward(JobOperationSearchView model)
        {
            try
            {
                var result = jobService.SearchDataForward(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("job-operation/getOrderInfo/{entity}/{por_no}")]
        public HttpResponseMessage getOrderInfo(string entity , string por_no)
        {
            try
            {
                var result = jobService.getOrderInfo(entity,por_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                //logSale.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

    }
}
