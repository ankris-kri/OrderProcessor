using OrderProcessor.Models;
using System;
using System.Linq;

namespace OrderProcessor.Rules
{
    public class NewMembershipPaymentRule : PaymentRuleBase
    {
        public override void Execute(Order order)
        {
            //TODO: SMPT to be implemented
            order.SetActionsPerformed("Email Sent");

            order.SetMembershipDetail(new MembershipDetail
            {
                ValidFrom = DateTime.Today
                //TODO ValidUpto gets calculated based on the product code of membership(1 month, 1Year, LifeTime etc). To be moved to another service
            });
            order.SetActionsPerformed("Membership added");
        }

        public override bool IsMatch(Order order)
        {
            return order.BasketItems.Any(item =>
            item.ProductGroupName == ProductGroupName.Membership &&
            item.Action == BasketAction.Add);
        }
    }
}
