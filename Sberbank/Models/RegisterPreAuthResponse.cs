namespace RoCMS.SberbankPaymentSystem.Models
{
    public class RegisterPreAuthResponse
    {
        public string orderId { get; set; }
        public string formUrl { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
