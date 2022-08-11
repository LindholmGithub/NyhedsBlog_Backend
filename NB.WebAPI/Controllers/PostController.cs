using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.PostDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _service;

        public PostController(IPostService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Post_DTO_Out>> GetAll()
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
        public ActionResult<Post_DTO_Out> GetById(int id)
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

        
        [HttpPost]
        public ActionResult<Post_DTO_Out> CreatePost([FromBody] Post_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreatePost(new Post
                {
                    Title = data.Title,
                    Author = new User{Id = data.AuthorId},
                    Category = new Category{Id = data.CategoryId},
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

        
        [HttpPut("{id:int}")]
        public ActionResult<Post_DTO_Out> UpdatePost(int id, [FromBody] Post_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdatePost(new Post{
                    Id = id,
                    Title = data.Title,
                    Author = new User{Id = data.AuthorId},
                    Category = new Category{Id = data.CategoryId},
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
        public ActionResult<Post_DTO_Out> DeletePost(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeletePost(new Post {Id = id})));
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
        
        //Conversion from Post to Post_DTO_Out
        private Post_DTO_Out Conversion(Post p)
        {
            return new Post_DTO_Out
            {
                Id = p.Id,
                Title = p.Title,
                Category = p.Category,
                Content = p.Content,
                Author = p.Author,
                Date = p.Date
            };
        }
    }
}
