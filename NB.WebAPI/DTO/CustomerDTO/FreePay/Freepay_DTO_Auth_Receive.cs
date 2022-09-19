namespace NB.WebAPI.DTO.CustomerDTO.FreePay
{
    public class Freepay_DTO_Auth_Receive
    {
        public int GatewayStatusCode { get; set; }
        public string GatewayStatusMessage { get; set; }
        public int AcquirerStatusCode { get; set; }
        public string AcquirerStatusMessage { get; set; }
        public string AcquirerName { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}