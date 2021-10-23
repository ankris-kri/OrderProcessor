using Newtonsoft.Json;
using NUnit.Framework;
using OrderProcessor.Models;
using OrderProcessor.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcessor.Tests.Rules
{
    [TestFixture]
    public class BookPaymentRuleTests
    {
        BookPaymentRule sut;

        [SetUp]
        public void Setup()
        {
            sut = new BookPaymentRule();
        }

        [Test]
        public void Should_generate_RoyaltyPackagingSlip()
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
                        ProductName = "BK_LifeOfPie",
                        ProductCode = "S04",
                        Action = BasketAction.Add,
                        ProductCategoryName = ProductCategoryName.PhysicalDeliverable,
                        ProductGroupName = ProductGroupName.Book
                    }
                }
            };

            var ExpectedPackagingSlipBasketItem = new PackagingSlip
            {
                Department = Department.Royality
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.True);

            Assert.That(order.ActionsPerformed.Count, Is.EqualTo(1));
            Assert.That(order.ActionsPerformed.Any(a => a == "Packaging slip for royalty generated"), Is.True);

            Assert.That(order.PackagingSlips.Count, Is.EqualTo(1));
            Assert.That(JsonConvert.SerializeObject(order.PackagingSlips[0]), Is.EqualTo(JsonConvert.SerializeObject(ExpectedPackagingSlipBasketItem)));
        }

        [Test]
        public void Should_NOT_generate_RoyaltyPackagingSlip()
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

            //Act
            var isMatchRuleResult = sut.IsMatch(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.False);
            Assert.That(order.ActionsPerformed, Is.Null);
            Assert.That(order.PackagingSlips, Is.Null);
        }
    }
}
