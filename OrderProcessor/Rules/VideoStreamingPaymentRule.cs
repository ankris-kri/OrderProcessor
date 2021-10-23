using OrderProcessor.Models;
using System;
using System.Linq;

namespace OrderProcessor.Rules
{
    public class VideoStreamingPaymentRule : PaymentRuleBase
    {
        public override void Execute(Order order)
        {
            var videoItems = order.BasketItems.Where(item => item.ProductGroupName == ProductGroupName.Video).ToList();
            foreach(var item in videoItems)
            {
                //TODO: Hardcoded here. Ideally mapping between the product and its related free products will be moved to a different service
                switch (item.ProductCode)
                {
                    case "S03":
                        order.BasketItems.Add(new BasketItem
                        {
                            Id = Guid.NewGuid(),
                            ProductName = "Vid_FirstAid",
                            ProductCode = "S05",
                            Action = BasketAction.Add,
                            ProductCategoryName = ProductCategoryName.VirtualDeliverable,
                            ProductGroupName = ProductGroupName.Video
                        });
                        break;
                    default:
                        break;
                }
            }

            order.SetActionsPerformed("First Aid video added");
        }

        public override bool IsMatch(Order order)
        {
            return order.BasketItems.Any(item => item.ProductGroupName == ProductGroupName.Video);
        }
    }
}
