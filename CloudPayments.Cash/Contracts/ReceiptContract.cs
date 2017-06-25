namespace CloudPayments.Cash.Contracts
{
    public class ReceiptContract
    {
        public string Inn { get; set; }
        public string InvoiceId { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; }
        public CustomerReceipt CustomerReceipt { get; set; }
    }
}