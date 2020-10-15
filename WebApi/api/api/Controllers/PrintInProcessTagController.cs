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
    public class PrintInProcessTagController : ApiController
    {
        IPrintInProcessTagService tagService;

        public PrintInProcessTagController()
        {

            tagService = new PrintInProcessTagService();
        }

        [Route("print-inprocess-tag/getProductInfo/{code}")]
        public HttpResponseMessage getProductInfo(string code)
        {
            try
            {
                var result = tagService.GetProductInfo(code);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("print-inproces-tag/postGetProduct")]
        public HttpResponseMessage postGetProduct(TagProductSearchView model)
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

        [Route("print-inprocess-tag/postPrintTag")]
        public HttpResponseMessage postPrintTag(PrintInProcessTagView model)
        {
            try
            {
                tagService.PringTag(model);

                return Request.CreateResponse(HttpStatusCode.OK, "ส่งข้อมูลไปยังเครื่องพิมพ์เรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
