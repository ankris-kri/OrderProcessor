
namespace OrderProcessor.Models
{
    public class PackagingSlip
    {
        public Department Department { get; set; }
    }

    public enum Department
    {
        Shipping,
        Royality
    }
}
