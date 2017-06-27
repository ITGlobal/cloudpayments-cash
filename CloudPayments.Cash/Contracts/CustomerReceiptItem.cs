using System.Runtime.Serialization;

namespace CloudPayments.Cash.Contracts
{
    [DataContract]
    public class CustomerReceiptItem
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "price")]
        public decimal Price { get; set; }

        [DataMember(Name = "quantity")]
        public decimal Quantity { get; set; }

        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }

        [DataMember(Name = "vat")]
        public VatValues? Vat { get; set; }

        [DataMember(Name = "ean13")]
        public string Ean13 { get; set; }
    }
}