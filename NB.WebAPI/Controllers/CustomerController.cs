using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NB.WebAPI.DTO;
using NB.WebAPI.DTO.AuthDTO;
using NB.WebAPI.DTO.CustomerDTO;
using NB.WebAPI.DTO.CustomerDTO.FreePay;
using NB.WebAPI.Util;
using NyhedsBlog_Backend.Core.IServices;
using NyhedsBlog_Backend.Core.Models.Customer;
using NyhedsBlog_Backend.Core.Models.Post;

namespace NB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IPostService _postService;
        private readonly BasicAuthenticationReader _authenticationReader;
        private readonly HttpClient _httpClient;

        public CustomerController(ICustomerService service, IPostService postService)
        {
            _service = service;
            _postService = postService;
            _authenticationReader = new BasicAuthenticationReader();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("da000255-75b4-41f3-aa70-3e751570baba");
            _httpClient.BaseAddress = new Uri("https://gw.freepay.dk/api/");
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

        [HttpPost("buy/{postId:int}")]
        public async Task<ActionResult<Payment_DTO_Out>> BuyPost(int postId)
        {
            try
            {
                var post = _postService.GetOneById(postId);

                if (!post.Paid)
                    return BadRequest(new Error_DTO(400, "Post is not a paid article"));
                
                var username = _authenticationReader.GetUsername(HttpContext);
                var password = _authenticationReader.GetPassword(HttpContext);

                try
                {
                    var authUser = _service.Validate(username, password);
                    var newPayment = _service.AddPayment(authUser, new Payment
                    {
                        Amount = post.Price,
                        Post = post,
                        Status = PaymentStatus.CREATED,
                        Timestamp = DateTime.Now
                    }).Payments.Last();

                    var freepayRequest = JsonSerializer.Serialize(new Freepay_DTO_Send
                    {
                        Amount = post.Price * 100,
                        Currency = "DKK",
                        OrderNumber = "S1-" + newPayment.Id,
                        SaveCard = false,
                        CustomerAcceptUrl = "https://status1.dk/payment-confirmed",
                        CustomerDeclineUrl = "https://status1.dk/payment-declined",
                        EnforceLanguage = "da-DK",
                        BillingAddress = new BillingAddress
                        {
                            AddressLine1 = authUser.Address,
                            City = authUser.City,
                            PostCode = authUser.Zipcode.ToString(),
                            Country = "208"
                        },
                        Options = new Options
                        {
                            TestMode = true
                        }
                    });
                    var requestContent = new StringContent(freepayRequest, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("payment", requestContent);
                    if (!response.IsSuccessStatusCode)
                        return StatusCode(500, new Error_DTO(500, ApiStrings.InternalServerError));

                    var content = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonSerializer.Deserialize<Freepay_DTO_Receive>(content);

                    Console.WriteLine(content);

                    return Ok(new Payment_DTO_Out
                    {
                        Amount = newPayment.Amount,
                        PostId = newPayment.Post.Id,
                        Status = newPayment.Status,
                        Timestamp = newPayment.Timestamp,
                        PaymentLink = deserialized.paymentWindowLink
                    });
                }
                catch (InvalidDataException e)
                {
                    return Unauthorized(new Error_DTO(401, e.Message));
                }
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
        public ActionResult<Customer_DTO_Out> CreateCustomer(Customer_DTO_In data)
        {
            try
            {
                
                var createdCustomer = Conversion(_service.CreateCustomer(new Customer
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
                            System.Text.Encoding.UTF8.GetBytes(createdCustomer.Username + ":" + data.Password));
                    
                    var newObject = new Auth_DTO_In
                    {
                        Username = createdCustomer.Username,
                        Redirect = data.Redirect
                    };

                    return Redirect(data.Redirect + "?status=200&userid="+createdCustomer.Id+"&message="+base64string);
                }
                
                return Ok(createdCustomer);
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
                Payments = c.Payments.Select(p => new Payment_DTO_Out
                {
                    Amount = p.Amount,
                    PostId = p.Post.Id,
                    Timestamp = p.Timestamp,
                    Status = p.Status
                }).ToList()
            };
        }
    }
}