namespace NB.WebAPI.DTO
{
    public class Error_DTO
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public Error_DTO(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}