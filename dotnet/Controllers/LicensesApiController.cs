using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Requests.Licenses;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/licenses")]
    [ApiController]
    public class LicensesApiController : BaseApiController
    {
        private ILicensesService _service = null;
        private IAuthenticationService<int> _authService = null;
        private ILicensesRelatedService _relatedService = null;
        private IEmailService _emailService = null;

        public LicensesApiController(ILicensesService service, ILicensesRelatedService relatedService
            , ILogger<LicensesApiController> logger
            , IAuthenticationService<int> authService, IEmailService emailService) : base(logger)
        {
            _service = service;
            _authService = authService;
            _relatedService = relatedService;
            _emailService = emailService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<LicenseExtended>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                LicenseExtended license = _service.Get(id);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");

                }
                else
                {
                    response = new ItemResponse<LicenseExtended> { Item = license };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);

            }

            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<LicenseExtended>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseExtended> paged = _service.GetAll(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseExtended>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet("createdby")]
        public ActionResult<ItemResponse<Paged<LicenseExtended>>> GetbyCreated(int pageIndex, int pageSize)
        {
            int code = 200;
            int userId = _authService.GetCurrentUserId();
            BaseResponse response = null;

            try
            {
                Paged<LicenseExtended> paged = _service.GetByCreatedBy(pageIndex, pageSize, userId);
                if (paged == null)
                {
                    code = 404;
          
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseExtended>> { Item = paged };
                    
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet("licensetype")]
        public ActionResult<ItemResponse<Paged<LicenseExtended>>> GetByLicenseType(int pageIndex, int pageSize, int licenseTypeId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseExtended> paged = _service.GetByLicenseTypeId(pageIndex, pageSize, licenseTypeId);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseExtended>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(LicenseAddRequest model)
        {
            ObjectResult result = null;
            int userId = _authService.GetCurrentUserId();
            try
            {
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(LicenseUpdateRequest model)
        {
            int code = 200;
            int userId = _authService.GetCurrentUserId();
            BaseResponse response = null;

            try
            {
                _service.Update(model, userId );
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);

            }

            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {

                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<LicenseExtended>>> Search(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseExtended> list = _service.Search(pageIndex, pageSize, query);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseExtended>>() { Item = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        //Expiring Licenses

        [HttpPost("notification/expired")]
        public ActionResult<ItemResponse<object>> ExpiredLicenseEmail(LicenseRelated formData)
        {
            ObjectResult result = null;

            try
            {
                _emailService.SendExpiredLicenseEmail(formData);
                ItemResponse<object> response = new ItemResponse<object>();

                response.Item = DateTime.Now.Ticks;

                result = Ok200(response);
            }
            catch (Exception ex)
            {

                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;

        }

        [HttpPost("notification/abouttoexpire")]
        public ActionResult<ItemResponse<object>> AboutToExpireLicenseEmail(LicenseRelated formData)
        {
            ObjectResult result = null;

            try
            {
                _emailService.SendAboutToExpireLicenseEmail(formData);
                ItemResponse<object> response = new ItemResponse<object>();

                response.Item = DateTime.Now.Ticks;

                result = Ok200(response);
            }
            catch (Exception ex)
            {

                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;

        }

        [HttpGet("paginate/expired")]
        public ActionResult<ItemResponse<Paged<License>>> GetAllExpired(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseRelated> paged = _relatedService.GetAllExpiredPaginated(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseRelated>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate/abouttoexpire")]
        public ActionResult<ItemResponse<Paged<License>>> GetAllAboutToExpire(int pageIndex, int pageSize, int daysToExpire)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseRelated> paged = _relatedService.GetAllAboutToExpirePaginated(pageIndex, pageSize, daysToExpire);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseRelated>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate/expireduringjob")]
        public ActionResult<ItemResponse<Paged<License>>> GetAllExpireDuringJob(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseRelated> paged = _relatedService.GetAllExpireDuringJobPaginated(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseRelated>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }

        [HttpGet("related/{id:int}")]
        public ActionResult<ItemResponse<License>> GetRelated(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                LicenseRelated license = _relatedService.GetRelatedById(id);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");

                }
                else
                {
                    response = new ItemResponse<LicenseRelated> { Item = license };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);

            }

            return StatusCode(iCode, response);
        }

        [HttpGet("state")]
        public ActionResult<ItemResponse<Paged<LicenseExtended>>> GetByLicenseState(int pageIndex, int pageSize, string state)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<LicenseExtended> paged = _service.GetByLicenseState(pageIndex, pageSize, state);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<LicenseExtended>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }
            return StatusCode(code, response);
        }


    }
}
