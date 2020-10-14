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
    public class DefaultPrinterController : ApiController
    {
        IDefaultPrinterService printerService;

        public DefaultPrinterController()
        {

            printerService = new DefaultPrinterService();
        }

        [Route("defprinter/getInfo/{code}")]
        public HttpResponseMessage getInfo(string code)
        {
            try
            {
                var result = printerService.GetInfo(code);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("defprinter/postSetDefault")]
        public HttpResponseMessage postSetDefault(DefaultPrinterView model)
        {
            try
            {

                printerService.SetDefault(model);

                return Request.CreateResponse(HttpStatusCode.OK, "บันทึกข้อมูลสำเร็จ");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

    }
}
