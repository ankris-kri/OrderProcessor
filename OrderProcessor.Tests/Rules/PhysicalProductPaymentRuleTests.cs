using Newtonsoft.Json;
using NUnit.Framework;
using OrderProcessor.Models;
using OrderProcessor.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderProcessor.Tests.Rules
{
    [TestFixture]
    public class PhysicalProductPaymentRuleTests
    {
        PhysicalProductPaymentRule sut;

        [SetUp]
        public void Setup()
        {
            sut = new PhysicalProductPaymentRule();
        }

        [Test]
        public void Should_generate_ShippingPackagingSlip_and_commissionPayment()
        {
            //Arrange
            var order = new Order
            {
                OrderId = "AT123",
                BasketItems = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = new Guid("8bf7bc31-38ea-4790-8729-a53673c2eed9"),
                        ProductName = "LG_60L_FrontLoad",
                        ProductCode = "S01",
                        Action = BasketAction.Add,
                        ProductCategoryName = ProductCategoryName.PhysicalDeliverable,
                        ProductGroupName = ProductGroupName.WashingMachine
                    }
                }
            };

            var ExpectedCommissionBasketItem = new BasketItem
            {
                ProductName = "Commision_agent_10%",
                ProductCode = "S02",
                Action = BasketAction.Add,
                ProductCategoryName = ProductCategoryName.Internal,
                ProductGroupName = ProductGroupName.Commission
            };

            var ExpectedPackagingSlipBasketItem = new PackagingSlip
            {
                Department = Department.Shipping
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);
            if(isMatchRuleResult) sut.Execute(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.True);

            Assert.That(order.ActionsPerformed.Count, Is.EqualTo(2));
            Assert.That(order.ActionsPerformed.Any(a => a == "Packaging slip for shipping generated"), Is.True);
            Assert.That(order.ActionsPerformed.Any(a => a == "Commission payment for Agent generated"), Is.True);

            Assert.That(order.PackagingSlips.Count, Is.EqualTo(1));
            Assert.That(JsonConvert.SerializeObject(order.PackagingSlips[0]), Is.EqualTo(JsonConvert.SerializeObject(ExpectedPackagingSlipBasketItem)));

            //Commission payment is added as another basketItem in the order
            Assert.That(order.BasketItems.Count, Is.EqualTo(2));
            Assert.That(JsonConvert.SerializeObject(order.BasketItems[1]), Is.EqualTo(JsonConvert.SerializeObject(ExpectedCommissionBasketItem)));
        }

        [Test]
        public void Should_NOT_generate_ShippingPackagingSlip_and_commissionPayment()
        {
            //Arrange
            var order = new Order
            {
                OrderId = "AT123",
                BasketItems = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = new Guid("8bf7bc31-38ea-4790-8729-a53673c2eed9"),
                        ProductName = "Vid_LearningToSki",
                        ProductCode = "S03",
                        Action = BasketAction.Add,
                        ProductCategoryName = ProductCategoryName.VirtualDeliverable,
                        ProductGroupName = ProductGroupName.Video
                    }
                }
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);
            if (isMatchRuleResult) sut.Execute(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.False);

            Assert.That(order.ActionsPerformed, Is.Null);

            Assert.That(order.PackagingSlips, Is.Null);

            //Commission payment is added as another basketItem in the order
            Assert.That(order.BasketItems.Count, Is.EqualTo(1));
        }
    }
}
