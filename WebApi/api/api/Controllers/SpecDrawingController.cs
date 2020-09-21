using api.Interfaces;
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


    }
}
