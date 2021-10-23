
using NUnit.Framework;
using OrderProcessor.Models;
using OrderProcessor.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcessor.Tests.Rules
{
    [testfixture]
    public class UpgradeMembershipPaymentRuleTests
    {
        UpgradeMemberShipPaymentRule sut;

        [SetUp]
        public void Setup()
        {
            sut = new UpgradeMemberShipPaymentRule();
        }

        [Test]
        public void Should_Upgrade_Membership_and_sendMail()
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
                        Action = BasketAction.Update,
                        ProductCategoryName = ProductCategoryName.VirtualDeliverable,
                        ProductGroupName = ProductGroupName.Membership
                    }
                }
            };
            order.SetMembershipDetail(new MembershipDetail
            {
                ValidUpto = DateTime.Today.AddMonths(1)
            }) ;

            var expectedMembership = new MembershipDetail
            {
                ValidUpto = DateTime.Today.AddMonths(1).AddYears(1)
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.True);

            Assert.That(order.ActionsPerformed.Count, Is.EqualTo(2));
            Assert.That(order.ActionsPerformed.Any(a => a == "Membership updated"), Is.True);
            Assert.That(order.ActionsPerformed.Any(a => a == "Email Sent"), Is.True);

            Assert.That(order.MembershipDetail, Is.Not.Null);
            Assert.That(order.MembershipDetail.ValidUpto, Is.EqualTo(expectedMembership.ValidUpto));
        }

        [Test]
        public void Should_NOT_Upgrade_Membership_and_sendMail()
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

            //Assert
            Assert.That(isMatchRuleResult, Is.False);

            Assert.That(order.ActionsPerformed, Is.Null);
            Assert.That(order.MembershipDetail, Is.Null);
        }
    }
}
