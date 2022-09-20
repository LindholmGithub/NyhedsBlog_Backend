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
        private const string FREEPAY_API_KEY = "da000255-75b4-41f3-aa70-3e751570baba";
        private const string FREEPAY_GW_URL = "https://gw.freepay.dk/api/";
        private const string FREEPAY_MW_URL = "https://mw.freepay.dk/api/";

        private const string NB_API_URL = "https://status1-azure.azurewebsites.net";
        private const string NB_WEB_URL = "https://status1.dk";
        
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
            _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(FREEPAY_API_KEY);
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

        [HttpGet("decline/{paymentId:int}")]
        public ActionResult<Payment_DTO_Out> DeclinePayment(int paymentId)
        {
            var oldPayment = _service.GetPayment(paymentId);
            oldPayment.Status = PaymentStatus.DECLINED;
            _service.UpdatePayment(oldPayment);

            var currentPost = _postService.GetOneById(oldPayment.Post.Id);

            return Redirect(NB_WEB_URL + "/payment-failed");
        }

        [HttpGet("capture/{paymentId:int}")]
        public async Task<ActionResult<Payment_DTO_Out>> CapturePayment(int paymentId, string authorizationIdentifier)
        {
            if (String.IsNullOrEmpty(authorizationIdentifier) && authorizationIdentifier.Length > 5)
                return BadRequest();

            var urlToSend = FREEPAY_MW_URL + "authorization/" + authorizationIdentifier +
                            "/capture";

            var response = await _httpClient.PostAsync(urlToSend, new StringContent(""));
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response code: " + response.StatusCode);
                return Redirect(NB_WEB_URL + "/payment-failed");
            }

            var content = await response.Content.ReadAsStringAsync();
            var deserialized = JsonSerializer.Deserialize<Freepay_DTO_Auth_Receive>(content);

            if (deserialized.IsSuccess)
            {
                var oldPayment = _service.GetPayment(paymentId);
                oldPayment.Status = PaymentStatus.CAPTURED;
                _service.UpdatePayment(oldPayment);

                var currentPost = _postService.GetOneById(oldPayment.Post.Id);

                return Redirect(NB_WEB_URL + "/indlaeg/" + currentPost.PrettyDescriptor);
            }
            else
            {
                return Redirect(NB_WEB_URL + "/payment-failed");
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
                    var newPayment = _service.AddPayment(new Payment
                    {
                        Amount = post.Price,
                        Post = post,
                        Status = PaymentStatus.CREATED,
                        Timestamp = DateTime.Now,
                        CustomerId = authUser.Id
                    });

                    var freepayRequest = JsonSerializer.Serialize(new Freepay_DTO_Send
                    {
                        Amount = post.Price * 100,
                        Currency = "DKK",
                        OrderNumber = "S1-" + newPayment.Id,
                        SaveCard = false,
                        CustomerAcceptUrl = NB_API_URL + "/api/customer/capture/" + newPayment.Id,
                        CustomerDeclineUrl = NB_API_URL + "/api/customer/decline/" + newPayment.Id,
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

                    var urlToSend = FREEPAY_GW_URL + "payment";
                    
                    var response = await _httpClient.PostAsync(urlToSend, requestContent);
                    if (!response.IsSuccessStatusCode)
                        return StatusCode(500, new Error_DTO(500, ApiStrings.InternalServerError));

                    var content = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonSerializer.Deserialize<Freepay_DTO_Receive>(content);

                    newPayment.PaymentLink = deserialized.paymentWindowLink;
                    _service.UpdatePayment(newPayment);
                    
                    return Ok(new Payment_DTO_Out
                    {
                        Id = newPayment.Id,
                        Amount = newPayment.Amount,
                        PostId = newPayment.Post.Id,
                        Status = (int) newPayment.Status,
                        Timestamp = newPayment.Timestamp,
                        PaymentLink = newPayment.PaymentLink
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
        [Consumes("application/json")]
        public ActionResult<Customer_DTO_Out> CreateCustomer([FromBody] Customer_DTO_In data)
        {
            return CreateCustomer_Private(data);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult<Customer_DTO_Out> CreateCustomer_Form([FromForm] Customer_DTO_In data)
        {
            return CreateCustomer_Private(data);
        }
        
        private ActionResult<Customer_DTO_Out> CreateCustomer_Private(Customer_DTO_In data)
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
                    Id = p.Id,
                    Amount = p.Amount,
                    PostId = p.Post.Id,
                    Timestamp = p.Timestamp,
                    Status = (int) p.Status,
                    PaymentLink = p.PaymentLink
                }).ToList()
            };
        }
    }
}