using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.SubscriptionDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Subscription;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _service;

        public SubscriptionController(ISubscriptionService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Subscription_DTO_Out>> GetAll()
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
        public ActionResult<Subscription_DTO_Out> GetById(int id)
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
        public ActionResult<Subscription_DTO_Out> CreateSubscription([FromBody] Subscription_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreateSubscription(new Subscription
                {
                    DateFrom = data.DateFrom,
                    DateTo = data.DateTo,
                    Type = data.Type
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
        public ActionResult<Subscription_DTO_Out> UpdateSubscription(int id, [FromBody] Subscription_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdateSubscription(new Subscription{
                    DateFrom = data.DateFrom,
                    DateTo = data.DateTo,
                    Type = data.Type
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
        public ActionResult<Subscription_DTO_Out> DeleteSubscription(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeleteSubscription(new Subscription{Id = id})));
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
        private Subscription_DTO_Out Conversion(Subscription s)
        {
            return new Subscription_DTO_Out
            {
                DateFrom = s.DateFrom,
                DateTo = s.DateTo,
                Type = s.Type
            };
        }
    }
}