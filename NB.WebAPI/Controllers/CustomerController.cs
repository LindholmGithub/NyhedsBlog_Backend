using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.CustomerDTO;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer_DTO_Out>> GetAll()
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
        public ActionResult<Customer_DTO_Out> GetById(int id)
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
        public ActionResult<Customer_DTO_Out> CreateCustomer([FromBody] Customer_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.CreateCustomer(new Customer
                {
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
                    //Subscription = new Subscription {Id = data.SubscriptionId}
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
        public ActionResult<Customer_DTO_Out> UpdateCustomer(int id, Customer_DTO_In data)
        {
            try
            {
                return Ok(Conversion(_service.UpdateCustomer(new Customer
                {
                    Id = id,
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
                    //Subscription = new Subscription {Id = data.SubscriptionId}
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
        public ActionResult<Customer_DTO_Out> Delete(int id)
        {
            try
            {
                return Ok(Conversion(_service.DeleteCustomer(new Customer {Id = id})));
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
        //Conversion from Customer to Customer_DTO_Out
        private Customer_DTO_Out Conversion(Customer c)
        {
            return new Customer_DTO_Out
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Username = c.Username,
                Password = c.Password,
                //Subscription = c.Subscription
            };
        }
    }
}