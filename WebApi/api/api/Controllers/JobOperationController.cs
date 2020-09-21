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
    public class JobOperationController : ApiController
    {
        IJobOperationService jobService;

        public JobOperationController()
        {

            jobService = new JobOperationService();
        }


    }
}
