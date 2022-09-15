namespace NB.WebAPI.DTO.CustomerDTO.FreePay
{
    public class Freepay_DTO_Send
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string OrderNumber { get; set; }
        public bool SaveCard { get; set; }
        public string CustomerAcceptUrl { get; set; }
        public string CustomerDeclineUrl { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public string EnforceLanguage { get; set; }
        public Options Options { get; set; }
    }

    public class BillingAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }

    public class Options
    {
        public bool TestMode { get; set; }
    }
}