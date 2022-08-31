using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.PageDTO;
using NB.WebAPI.DTO.UserDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {

        private readonly IPageService _service;

        public PageController(IPageService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Page_DTO_Out>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll().Select(Conversion));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500, e.Message));
            }
        }
        
        [HttpGet("{id:int}")]
        public ActionResult<Page_DTO_Out> GetById(int id)
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
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500, ApiStrings.InternalServerError));
            }
        }

        [HttpGet("slug/{slug}")]
        public ActionResult<Page_DTO_Out> GetBySlug(string slug)
        {
            try
            {
                return Ok(Conversion(_service.GetOneBySlug(slug)));
            }
            catch (InvalidDataException e)
            {
                return BadRequest(new Error_DTO(400, e.Message));
            }
            catch (FileNotFoundException e)
            {
                return NotFound(new Error_DTO(404, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500, ApiStrings.InternalServerError));
            }
        }

        [HttpPost]
        public ActionResult<Page_DTO_Out> CreatePage([FromBody] Page_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreatePage(new Page
                {
                    Title = data.Title,
                    PrettyDescriptor = data.PrettyDescriptor,
                    Author = new User{Id = data.AuthorId},
                    Content = data.Content,
                    Date = data.Date
                })));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
            }
        }
        
        [HttpPut("{id:int}")]
        public ActionResult<Page_DTO_Out> UpdatePage(int id, [FromBody] Page_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdatePage(new Page{
                    Id = id,
                    Title = data.Title,
                    PrettyDescriptor = data.PrettyDescriptor,
                    Author = new User{Id = data.AuthorId},
                    Content = data.Content,
                    Date = data.Date
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
        public ActionResult<Page_DTO_Out> DeletePage(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeletePage(new Page {Id = id})));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500,ApiStrings.InternalServerError));
            }
        }
        
        //Conversion from Page to Page_DTO_Out
        private Page_DTO_Out Conversion(Page p)
        {
            return new Page_DTO_Out
            {
                Id = p.Id,
                Title = p.Title,
                PrettyDescriptor = p.PrettyDescriptor,
                Content = p.Content,
                Author = new User_DTO_Out
                {
                    Id = p.Author.Id,
                    Firstname = p.Author.Firstname,
                    Lastname = p.Author.Lastname,
                    Email = p.Author.Email,
                    Username = p.Author.Username,
                    Password = p.Author.Password,
                    PhoneNumber = p.Author.PhoneNumber,
                    Role = (int) p.Author.Role
                },
                Date = p.Date
            };
        }
    }
}