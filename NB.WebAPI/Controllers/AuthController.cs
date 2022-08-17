using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO.AuthDTO;
using NyhedsBlog_Backend.Core.IServices;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ICustomerService _service;

        public AuthController(ICustomerService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public ActionResult<Auth_DTO_Out> Validate([FromForm] Auth_DTO_In data)
        {
            var checkUser = _service.Validate(data.Username, data.Password);
            if (checkUser != null)
            {
                var base64string =
                    System.Convert.ToBase64String(
                        System.Text.Encoding.UTF8.GetBytes(data.Username + ":" + data.Password));
                
                if (data.Redirect is {Length: > 0})
                {
                    return Redirect(data.Redirect + "?status=200&userid="+checkUser.Id+"&message="+base64string);
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
    }
}