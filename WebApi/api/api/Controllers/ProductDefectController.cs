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
    public class ProductDefectController : ApiController
    {
        IProductDefectService defectService;

        public ProductDefectController()
        {
            defectService = new ProductDefectService();
        }

        [Route("product-defect/postSearchDataProductDefect")]
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

        [Route("product-defect/getItemNo/{entity}/{por_no}")]
        public HttpResponseMessage getItemNo(string entity, string por_no)
        {
            try
            {
                var result = defectService.GetItemNo(entity, por_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        [Route("product-defect/getItemNoList/{entity}/{por_no}")]
        public HttpResponseMessage getItemNoList(string entity, string por_no)
        {
            try
            {
                var result = defectService.GetItemNoList(entity, por_no);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        [Route("product-defect/postDataQcCutting")]
        public HttpResponseMessage postDataQcCutting(DataQcCuttingView model)
        {
            try
            {

                defectService.DataQcCutting(model);

                CommonResponseView res = new CommonResponseView()
                {
                    status = CommonStatus.SUCCESS,
                    message = "บันทึกข้อมูลสำเร็จ"
                };

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}