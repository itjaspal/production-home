﻿using api.Interfaces;
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
    public class SpecDrawingController : ApiController
    {
        ISpecDrawingService specService;

        public SpecDrawingController()
        {

            specService = new SpecDrawingService();
        }

        [Route("spec/postdrawning")]
        public HttpResponseMessage postdrawing(SpecDrawingSearchView model)
        {
            try
            {
                var result = specService.GetSpecInfo(model);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }


    }
}
