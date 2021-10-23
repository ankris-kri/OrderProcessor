using System;

namespace OrderProcessor.Models
{
    public class MembershipDetail
    {
        public string Id { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUpto { get; set; }
    }
}
