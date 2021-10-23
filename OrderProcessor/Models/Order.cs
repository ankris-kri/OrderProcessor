using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessor.Models
{
    public class Order
    {
        [Required]
        public string OrderId { get; set; }
        [Required, MinLength(1)]
        public List<BasketItem> BasketItems { get; set; }
        public MembershipDetail MembershipDetail { get; private set; }
        public List<PackagingSlip> PackagingSlips { get; private set; }

        /// <summary>
        /// ActionsPerformed is just for this demo purpose to show the list of actions been performed by the Rule
        /// </summary>
        public List<string> ActionsPerformed { get; private set; }

        public void SetBasketItem(BasketItem newBasketItem)
        {
            BasketItems ??= new List<BasketItem>();
            BasketItems.Add(newBasketItem);
        }

        public void SetMembershipDetail(MembershipDetail membershipDetail)
        {
            MembershipDetail = membershipDetail;
        }

        public void SetPackagingSlips(PackagingSlip newPackagingSlip)
        {
            PackagingSlips ??= new List<PackagingSlip>();
            PackagingSlips.Add(newPackagingSlip);
        }

        public void SetActionsPerformed(string action)
        {
            ActionsPerformed ??= new List<string>();
            ActionsPerformed.Add(action);
        }
    }
}
