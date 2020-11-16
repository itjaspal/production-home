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

        [Route("productionRec/postSearchPutAwayWaiting")]
        public HttpResponseMessage postSearchPutAwayWaiting(ProductionRecSearchView model)
        {
            try
            {
                var result = jobService.SearchPutAwayWaiting(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("productionRec/getTimeDelay/{entity}/{build_type}")]
        public HttpResponseMessage getgetTimeDelay(string entity , string build_type)
        {
            try
            {
                var result = jobService.getTimeDelay(entity,build_type);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


    }
}
