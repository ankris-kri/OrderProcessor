using OrderProcessor.Models;
using System;
using System.Linq;

namespace OrderProcessor.Rules
{
    public class PhysicalProductPaymentRule : PaymentRuleBase
    {
        public override void Execute(Order order)
        {
            order.SetPackagingSlips(new PackagingSlip { Department = Department.Shipping });
            order.SetActionsPerformed("Packaging slip for shipping generated");

            //TODO: Here its hardcoded. But in reality, Commission generation logic to be moved to another system. 
            order.SetBasketItem(new BasketItem
            {
                Id = Guid.NewGuid(),
                ProductName = "Commision_agent_10%",
                ProductCode = "S02",
                Action = BasketAction.Add,
                ProductCategoryName = ProductCategoryName.Internal,
                ProductGroupName = ProductGroupName.Commission
            }); ;

            order.SetActionsPerformed("Commission payment for Agent generated");
        }

        public override bool IsMatch(Order order)
        {
            return order.BasketItems.Any(item => item.ProductCategoryName == ProductCategoryName.PhysicalDeliverable);
        }
    }
}
