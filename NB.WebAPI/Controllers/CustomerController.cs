using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.AuthDTO;
using NB.WebAPI.DTO.CustomerDTO;
using NB.WebAPI.DTO.SubscriptionDTO;
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
        }

        [HttpPost]
        public ActionResult<Customer_DTO_Out> CreateCustomer([FromForm] Customer_DTO_In data)
        {
            try
            {
                
                var CreatedCustomer = Conversion(_service.CreateCustomer(new Customer
                {
                    Firstname = data.Firstname,
                    Lastname = data.Lastname,
                    Address = data.Address,
                    Zipcode = data.Zipcode,
                    City = data.City,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
                }));

                if (data.Redirect is {Length: > 0})
                {
                    var base64string =
                        System.Convert.ToBase64String(
                            System.Text.Encoding.UTF8.GetBytes(CreatedCustomer.Username + ":" + data.Password));
                    
                    var newObject = new Auth_DTO_In
                    {
                        Username = CreatedCustomer.Username,
                        Redirect = data.Redirect
                    };

                    return Redirect(data.Redirect + "?status=200&userid="+CreatedCustomer.Id+"&message="+base64string);
                }
                
                return Ok(CreatedCustomer);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new Error_DTO(400, ae.Message));
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
                    Address = data.Address,
                    Zipcode = data.Zipcode,
                    City = data.City,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    Username = data.Username,
                    Password = data.Password,
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
                Address = c.Address,
                Zipcode = c.Zipcode,
                City = c.City,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Username = c.Username,
                Subscription = new Subscription_DTO_Out
                {
                    DateFrom = c.Subscription.DateFrom,
                    DateTo = c.Subscription.DateTo,
                    Type = c.Subscription.Type
                }
            };
        }
    }
}