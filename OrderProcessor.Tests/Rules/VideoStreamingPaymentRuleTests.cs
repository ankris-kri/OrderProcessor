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
    public class VideoStreamingPaymentRuleTests
    {
        VideoStreamingPaymentRule sut;

        [SetUp]
        public void Setup()
        {
            sut = new VideoStreamingPaymentRule();
        }

        [Test]
        public void Should_Add_FirstAidVideo()
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

            var ExpectedFirstAidBasketItem = new BasketItem
            {
                ProductName = "Vid_FirstAid",
                ProductCode = "S05",
                Action = BasketAction.Add,
                ProductCategoryName = ProductCategoryName.VirtualDeliverable,
                ProductGroupName = ProductGroupName.Video
            };

            //Act
            var isMatchRuleResult = sut.IsMatch(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.True);

            Assert.That(order.ActionsPerformed.Count, Is.EqualTo(1));
            Assert.That(order.ActionsPerformed.Any(a => a == "First Aid video added"), Is.True);

            Assert.That(order.BasketItems.Count, Is.EqualTo(2));
            Assert.That(JsonConvert.SerializeObject(order.BasketItems[1]), Is.EqualTo(JsonConvert.SerializeObject(ExpectedFirstAidBasketItem)));
        }

        [Test]
        public void Should_NOT_Add_FirstAidVideo()
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

            //Act
            var isMatchRuleResult = sut.IsMatch(order);

            //Assert
            Assert.That(isMatchRuleResult, Is.False);
            Assert.That(order.ActionsPerformed, Is.Null);
            Assert.That(order.BasketItems.Count, Is.EqualTo(1));
        }
    }
}
