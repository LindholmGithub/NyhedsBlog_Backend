using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO.AuthDTO;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Auth_DTO_Out> Validate([FromForm] Auth_DTO_In data)
        {
            if (data.Username == "test@test.com" && data.Password == "test")
            {
                if (data.Redirect is {Length: > 0})
                {
                    return Redirect(data.Redirect + "?status=200&message=base64string");
                }
                
                return Ok(new Auth_DTO_Out
                {
                    Status = 200,
                    Message = "base64stringg"
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