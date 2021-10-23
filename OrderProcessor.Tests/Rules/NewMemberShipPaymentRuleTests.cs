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
    public class NewMemberShipPaymentRuleTests
    {
        NewMembershipPaymentRule sut;

        [SetUp]
        public void Setup()
        {
            sut = new NewMembershipPaymentRule();
        }

        [Test]
        public void Should_Activate_Membership_and_sendMail()
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
                        ProductName = "MemberShip_1Y",
                        ProductCode = "S06",
                        Action = BasketAction.Add,
                        ProductCategoryName = ProductCategoryName.VirtualDeliverable,
                        ProductGroupName = ProductGroupName.Membership
                    }
                }
            };

            var expectedMembership = new MembershipDetail
            {
                ValidFrom = DateTime.Today
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);
            if (isMatchRuleResult) sut.Execute(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.True);

            Assert.That(order.ActionsPerformed.Count, Is.EqualTo(2));
            Assert.That(order.ActionsPerformed.Any(a => a == "Membership added"), Is.True);
            Assert.That(order.ActionsPerformed.Any(a => a == "Email Sent"), Is.True);

            Assert.That(order.MembershipDetail, Is.Not.Null);
            Assert.That(order.MembershipDetail.ValidFrom, Is.EqualTo(expectedMembership.ValidFrom));
        }

        [Test]
        public void Should_NOT_Activate_Membership_and_sendMail()
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
            Assert.That(order.MembershipDetail, Is.Null);
        }
    }
}
