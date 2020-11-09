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
    public class ProductDefectController : ApiController
    {
        IProductDefectService defectService;

        public ProductDefectController()
        {
            defectService = new ProductDefectService();
        }

        [Route("product-defect/postSearchDataScanReceive")]
        public HttpResponseMessage SearchDataProductDefect(ProductDefectSearchView model)
        {
            try
            {
                var result = defectService.SearchDataProductDefect(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
