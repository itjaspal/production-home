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
    public class ProductionRecController : ApiController
    {
        IProductionRecService jobService;
        public ProductionRecController()
        {

            jobService = new ProductionRecService();
        }

        [Route("productionRec/postSearchProductionRec")]
        public HttpResponseMessage postSearchProductionRec(ProductionRecSearchView model)
        {
            try
            {
                var result = jobService.SearchProductionRec(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("productionRecDetail/postSearchProductionRecDetail")]
        public HttpResponseMessage postSearchProductionRecDetail(ProductionRecDetailSearchView model)
        {
            try
            {
                var result = jobService.SearchProductionRecDetail(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


    }
}
