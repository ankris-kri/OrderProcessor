using OrderProcessor.Models;
using System;
using System.Linq;

namespace OrderProcessor.Rules
{
    public class BookPaymentRule : PaymentRuleBase
    {
        public override void Execute(Order order)
        {
            order.SetPackagingSlips(new PackagingSlip { Department = Department.Royality });
            order.SetActionsPerformed("Packaging slip for royalty generated");
        }

        public override bool IsMatch(Order order)
        {
            return order.BasketItems.Any(item => item.ProductGroupName == ProductGroupName.Book);
        }
    }
}
