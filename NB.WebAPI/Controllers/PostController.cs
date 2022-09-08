using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.CategoryDTO;
using NB.WebAPI.DTO.PostDTO;
using NB.WebAPI.DTO.UserDTO;
using NB.WebAPI.Util;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;
using NyhedsBlog_Backend.Core.Models.Post;
using NyhedsBlog_Backend.Core.Models.Subscription;
using NyhedsBlog_Backend.Core.Models.User;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private int CHARACTERS_FOR_UNAUTHORIZED = 425;
        
        private readonly IPostService _service;
        private readonly ICustomerService _customerService;
        private readonly BasicAuthenticationReader _authenticationReader;

        public PostController(IPostService service, ICustomerService customerService)
        {
            _service = service;
            _customerService = customerService;
            _authenticationReader = new BasicAuthenticationReader();
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Post_DTO_Out>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll().Select(Conversion_Unauthorized));
            }
            catch (Exception e)
            {
                return StatusCode(500, new Error_DTO(500, e.Message));
            }
        }

        
        [HttpGet("{id:int}")]
        public ActionResult<Post_DTO_Out> GetById(int id)
        {
            try
            {
                var username = _authenticationReader.GetUsername(HttpContext);
                var password = _authenticationReader.GetPassword(HttpContext);

                Post toReturn = _service.GetOneById(id);
                try
                {
                    var selectedUser = _customerService.Validate(username, password);
                    if (selectedUser.Subscription.Type >= toReturn.RequiredSubscription)
                        return Ok(Conversion(toReturn));
                }
                catch (InvalidDataException e)
                {
                    return Ok(Conversion_Unauthorized(toReturn));
                }
                
                return Ok(Conversion_Unauthorized(toReturn));
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
        public ActionResult<Post_DTO_Out> GetBySlug(string slug)
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
        public ActionResult<Post_DTO_Out> CreatePost([FromBody] Post_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreatePost(new Post
                {
                    Title = data.Title,
                    Author = new User{Id = data.AuthorId},
                    PrettyDescriptor = data.PrettyDescriptor,
                    Category = new Category{Id = data.CategoryId},
                    FeaturedImageUrl = data.FeaturedImageUrl,
                    Content = data.Content,
                    RequiredSubscription = (SubscriptionType) data.RequiredSubscription,
                    Date = data.Date
                })));
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
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
                    PrettyDescriptor = data.PrettyDescriptor,
                    FeaturedImageUrl = data.FeaturedImageUrl,
                    Category = new Category{Id = data.CategoryId},
                    Content = data.Content,
                    RequiredSubscription = (SubscriptionType) data.RequiredSubscription,
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
                Category = new Category_DTO_Out
                {
                    Id = p.Category.Id,
                    Title = p.Category.Title,
                    Description = p.Category.Description,
                    PrettyDescriptor = p.Category.PrettyDescriptor
                },
                PrettyDescriptor = p.PrettyDescriptor,
                FeaturedImageUrl = p.FeaturedImageUrl,
                Content = p.Content,
                Author = new User_DTO_Out
                {
                    Id = p.Author.Id,
                    Firstname = p.Author.Firstname,
                    Lastname = p.Author.Lastname,
                    Email = p.Author.Email,
                    Username = p.Author.Username,
                    PhoneNumber = p.Author.PhoneNumber,
                    Role = (int) p.Author.Role
                },
                Authorized = true,
                RequiredSubscription = (int)p.RequiredSubscription,
                Date = p.Date
            };
        }
        
        private Post_DTO_Out Conversion_Unauthorized(Post p)
        {
            
            return new Post_DTO_Out
            {
                Id = p.Id,
                Title = p.Title,
                Category = new Category_DTO_Out
                {
                    Id = p.Category.Id,
                    Title = p.Category.Title,
                    Description = p.Category.Description,
                    PrettyDescriptor = p.Category.PrettyDescriptor
                },
                PrettyDescriptor = p.PrettyDescriptor,
                FeaturedImageUrl = p.FeaturedImageUrl,
                Content = p.Content[..CHARACTERS_FOR_UNAUTHORIZED] + "...",
                Author = new User_DTO_Out
                {
                    Id = p.Author.Id,
                    Firstname = p.Author.Firstname,
                    Lastname = p.Author.Lastname,
                    Email = p.Author.Email,
                    Username = p.Author.Username,
                    PhoneNumber = p.Author.PhoneNumber,
                    Role = (int) p.Author.Role
                },
                Authorized = false,
                RequiredSubscription = (int)p.RequiredSubscription,
                Date = p.Date
            };
        }
    }
}
