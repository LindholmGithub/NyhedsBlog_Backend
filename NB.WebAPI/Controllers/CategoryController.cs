using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.CategoryDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Post;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category_DTO_Out>> GetAll()
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
        public ActionResult<Category_DTO_Out> GetById(int id)
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
        public ActionResult<Category_DTO_Out> CreateCategory([FromBody] Category_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreateCategory(new Category
                {
                    Title = data.Title,
                    Description = data.Description
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
        public ActionResult<Category_DTO_Out> UpdateCategory(int id, Category_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdateCategory(new Category
                {
                    Id = id,
                    Title = data.Title,
                    Description = data.Description
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
        public ActionResult<Category_DTO_Out> DeleteCategory(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeleteCategory(new Category{Id = id})));
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

        private Category_DTO_Out Conversion(Category c)
        {
            return new Category_DTO_Out
            {
                Id = c.Id,
                Description = c.Description,
                Title = c.Title
            };
        } 
    }
}