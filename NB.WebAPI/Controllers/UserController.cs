using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.UserDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User_DTO_Out>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll().Select(Conversion));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500, ApiStrings.InternalServerError));
            }
        }
        [HttpGet("{id:int}")]
        public ActionResult<User_DTO_Out> GetById(int id)
        {
            try
            {
                return Ok(Conversion(_service.GetOneById(id)));
            }
            catch (InvalidDataException e)
            {
                return BadRequest(new Error_DTO(400, e.Message));
            }
            catch (FileNotFoundException e)
            {
                return NotFound(new Error_DTO(404, e.Message));
            }
        }
        
        [HttpPost]
        public ActionResult<User_DTO_Out> CreateCustomer([FromBody] User_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreateUser(new User
                {
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
                    Role = (UserRole) data.Role
                })));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
            }
        }
        
        [HttpPut("{id:int}")]
        public ActionResult<User_DTO_Out> UpdateCustomer(int id, User_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdateUser(new User
                {
                    Id = id,
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
                    Role = (UserRole) data.Role
                })));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500,e.Message));
            }
        }
        
        [HttpDelete("{id:int}")]
        public ActionResult<User_DTO_Out> Delete(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeleteUser(new User {Id = id})));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500,e.Message));
            }
        }
        
        private User_DTO_Out Conversion(User u)
        {
            int role = u.Role switch
            {
                UserRole.Administrator => 3,
                UserRole.Moderator => 2,
                _ => 1
            }; 
            
            return new User_DTO_Out
            {
                Id = u.Id,
                Firstname = u.Firstname,
                Lastname = u.Lastname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Username = u.Username,
                Password = u.Password,
                Role = role
            };
        }
    }
}