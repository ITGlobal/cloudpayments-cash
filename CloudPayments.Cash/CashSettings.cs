namespace CloudPayments.Cash
{
    public class CashSettings
    {
        public string PublicId { get; set; }
        
        public string ApiSecret { get; set; }

        /// <summary>
        /// Optional INN, can be set in ReceiptContract
        /// </summary>
        public string Inn { get; set; }
        
        public string Endpoint { get; set; } = "https://api.cloudpayments.ru";
        
        public bool Test { get; set; } = false;
    }
}