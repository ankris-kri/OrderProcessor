using OrderProcessor.Models;
using System;
using System.Linq;

namespace OrderProcessor.Rules
{
    public class UpgradeMemberShipPaymentRule : PaymentRuleBase
    {
        public override bool IsMatch(Order order)
        {
            return order.BasketItems.Any(item =>
            item.ProductGroupName == ProductGroupName.Membership &&
            item.Action == BasketAction.Update);
        }

        public override void Execute(Order order)
        {
            //TODO: SMPT to be implemented
            order.SetActionsPerformed("Email Sent");

            //TODO: if null, fetch from our DB
            if(order.MembershipDetail != null)
            {
                var membershipItems = order.BasketItems.Where(item => item.ProductGroupName == ProductGroupName.Membership).ToList();
                foreach (var item in membershipItems)
                {
                    //TODO: Remove switch case. Based on membership productcode, a service should provide us the membership extension days
                    switch (item.ProductCode)
                    {
                        case "S06":
                            order.SetMembershipDetail(new MembershipDetail
                            {
                                ValidUpto = order.MembershipDetail.ValidUpto.AddYears(1)
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
            order.SetActionsPerformed("Membership updated");
        }
    }
}
