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
    public class AuthenController : ApiController
    {
        IAuthenticationService authService;

        public AuthenController()
        {

            authService = new AuthenticationService();
        }


        [Route("authen/login")]
        public HttpResponseMessage PostLogin([FromBody]AuthenticationParam param)
        {
            try
            {
                var user = authService.login(param.username, param.password);


                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        //[Route("pc/getUserRole")]
        //public HttpResponseMessage PostuserRole([FromBody]AuthenticationUserRoleParam param)
        //{
        //    try
        //    {
        //        var userrole = authService.getUserRole(param.userRoleId);

        //        return Request.CreateResponse(HttpStatusCode.OK, userrole);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
        //    }
        //}
    }
}
