using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO.AuthDTO;
using NB.WebAPI.Util;
using NyhedsBlog_Backend.Core.IServices;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ICustomerService _service;
        private IUserService _adminService;
        private BasicAuthenticationReader _authReader;

        public AuthController(ICustomerService service, IUserService adminService)
        {
            _service = service;
            _adminService = adminService;
            _authReader = new BasicAuthenticationReader();
        }

        [HttpPost("admin/validate")]
        public ActionResult<Auth_Admin_DTO_Out> ValidateAdminBase64()
        {
            var username = _authReader.GetUsername(HttpContext);
            var password = _authReader.GetPassword(HttpContext);
            try
            {
                var checkUser = _adminService.Validate(username, password);
                if (checkUser != null)
                {
                    var base64string =
                        System.Convert.ToBase64String(
                            System.Text.Encoding.UTF8.GetBytes(username + ":" + password));

                    return Ok(new Auth_Admin_DTO_Out
                    {
                        Status = 200,
                        Message = base64string,
                        UserId = checkUser.Id
                    });
                }

                return Unauthorized(new Auth_Admin_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credetials"
                });
            }
            catch (InvalidDataException e)
            {
                return Unauthorized(new Auth_Admin_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credetials"
                });
            }
        }

        [HttpPost("admin")]
        public ActionResult<Auth_Admin_DTO_Out> ValidateAdmin([FromForm] Auth_DTO_In data)
        {
            try
            {
                var checkUser = _adminService.Validate(data.Username, data.Password);
                if (checkUser != null)
                {
                    var base64string =
                        System.Convert.ToBase64String(
                            System.Text.Encoding.UTF8.GetBytes(data.Username + ":" + data.Password));

                    return Ok(new Auth_Admin_DTO_Out
                    {
                        Status = 200,
                        Message = base64string,
                        UserId = checkUser.Id
                    });
                }

                return Unauthorized(new Auth_Admin_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credetials"
                });
            }
            catch (InvalidDataException e)
            {
                return Unauthorized(new Auth_Admin_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credetials"
                });
            }
        }

        [HttpPost]
        public ActionResult<Auth_DTO_Out> Validate([FromForm] Auth_DTO_In data)
        {
            try
            {
                var checkUser = _service.Validate(data.Username, data.Password);
                if (checkUser != null)
                {
                    var base64string =
                        System.Convert.ToBase64String(
                            System.Text.Encoding.UTF8.GetBytes(data.Username + ":" + data.Password));

                    if (data.Redirect is {Length: > 0})
                    {
                        return Redirect(data.Redirect + "?status=200&userid=" + checkUser.Id + "&message=" +
                                        base64string);
                    }

                    return Ok(new Auth_DTO_Out
                    {
                        Status = 200,
                        Message = base64string
                    });
                }

                if (data.Redirect is {Length: > 0})
                {
                    return Redirect(data.Redirect + "?status=401&message=invalidcredentials");
                }

                return Unauthorized(new Auth_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credentials"
                });
            }
            catch (InvalidDataException e)
            {
                if (data.Redirect is {Length: > 0})
                {
                    return Redirect(data.Redirect + "?status=401&message=invalidcredentials");
                }

                return Unauthorized(new Auth_DTO_Out
                {
                    Status = 401,
                    Message = "Invalid Credentials"
                });
            }
        }
    }
}