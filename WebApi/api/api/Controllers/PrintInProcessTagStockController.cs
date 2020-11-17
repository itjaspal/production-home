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
    public class PrintInProcessTagStockController : ApiController
    {
        IPrintInProcessTagStockService tagService;

        public PrintInProcessTagStockController()
        {

            tagService = new PrintInProcessTagStockService();
        }

        [Route("print-inproces-tag-stock/postGetProduct")]
        public HttpResponseMessage postGetProduct(TagStockProductSearchView model)
        {
            try
            {
                var result = tagService.getProduct(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("print-inproces-tag-stock/postGetGroup")]
        public HttpResponseMessage postGetGroup(TagStockProductSearchView model)
        {
            try
            {
                var result = tagService.getGroup(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
