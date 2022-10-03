using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Location;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationApiController : BaseApiController
    {
        private ILocationService _service = null;
        private IAuthenticationService<int> _authService = null;

        public LocationApiController(ILocationService service,
            ILogger<LocationApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService; 
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Location>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Location location = _service.GetById(id);
                if(location == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Location Record Not Found");
                }
                else
                {
                    response = new ItemResponse<Location> { Item = location };
                }

            }
            catch(Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }


            return StatusCode(iCode, response);
        }


        [HttpGet("geosearch")]
        public ActionResult<ItemsResponse<List<Location>>> GetByGeo(int radius, double latitude, double longitude)
        {
            int iCode = 200;
            BaseResponse response;
            List<Location> locations = null;

            try
            {
                locations = _service.GetByGeo(radius,latitude,longitude);
                if (locations == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("No Records Were Found");
                }
                else
                {
                    response = new ItemsResponse<Location> { Items = locations };
                }

            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }


            return StatusCode(iCode, response);
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<Location>>> GetAll(int pageIndex,int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Location> pagedLocations = _service.GetAllPaginated(pageIndex, pageSize);
                if(pagedLocations == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("No Records Found");
                }
                else
                {
                   response = new ItemResponse<Paged<Location>> { Item = pagedLocations };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }


        [HttpGet("createdby")]
        public ActionResult<ItemResponse<Paged<Location>>> GetByCreatedPaginated(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;
            

            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Location> pagedLocations = _service.GetByCreatedByPaginated(pageIndex, pageSize,userId);
                if (pagedLocations == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("No Records Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Location>> { Item = pagedLocations };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(LocationAddRequest model)
        {
            int iCode = 201;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int newId = _service.Create(model, userId);
                response = new ItemResponse<int> { Item = newId };
            }
            catch(Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(LocationUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteById(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }
    }
}
