using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudPayments.Cash.Contracts
{
    [DataContract]
    public class CustomerReceipt
    {
        [DataMember(Name = "Items")]
        public List<CustomerReceiptItem> Items { get; set; }
        
        [DataMember(Name="taxationSystem")]
        public TaxationSystem TaxationSystem { get; set; } = TaxationSystem.Common;

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }
    }
}