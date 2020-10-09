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
    public class JobInProcessController : ApiController
    {
        IJobInProcessService inprocessService;

        public JobInProcessController()
        {

            inprocessService = new JobInProcessService();
        }

        [Route("job-inproces/postSearchScanAdd")]
        public HttpResponseMessage postSearchScanAdd(JobInProcessSearchView model)
        {
            try
            {
                var result = inprocessService.ScanAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("job-inproces/postSearchScanCancel")]
        public HttpResponseMessage postSearchScanCancel(JobInProcessSearchView model)
        {
            try
            {
                var result = inprocessService.ScanCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("job-inproces/postGetProduct")]
        public HttpResponseMessage postGetProduct(ProductSearchView model)
        {
            try
            {
                var result = inprocessService.getProduct(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        
        [Route("job-inproces/postGetProductCancel")]
        public HttpResponseMessage postGetProductCancel(ProductSearchView model)
        {
            try
            {
                var result = inprocessService.getProductCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


        [Route("job-inproces/postSearchEntryAdd")]
        public HttpResponseMessage postSearchEntryAdd(JobInProcessSearchView model)
        {
            try
            {
                var result = inprocessService.EntryAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("job-inproces/postSearchEntryCancel")]
        public HttpResponseMessage postSearchEntryCancel(JobInProcessSearchView model)
        {
            try
            {
                var result = inprocessService.EntryCancel(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

    }
}
