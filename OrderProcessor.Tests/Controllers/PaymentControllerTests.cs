using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OrderProcessor.Controllers;
using OrderProcessor.Models;
using OrderProcessor.RuleEngines;
using System;

namespace OrderProcessor.Tests.Controllers
{
    [TestFixture]
    class PaymentControllerTests
    {
        Mock<IPaymentRuleEngine> _ruleEngine;
        Mock<PaymentController> _controller;

        [SetUp]
        public void Setup()
        {
            _ruleEngine = new Mock<IPaymentRuleEngine>(MockBehavior.Strict);
            _controller = new Mock<PaymentController>(_ruleEngine.Object) { CallBase = true };
        }

        [TearDown]
        public void Teardown()
        {
            _ruleEngine.VerifyAll();
        }

        [Test]
        public void Validate_Process_returns_200_responsecode()
        {
            //Arrange
            var order = new Order();
            _ruleEngine.Setup(e => e.ExecuteRules(order));

            //Act
            var result = _controller.Object.Process(order);
            var okResult = result as OkObjectResult;

            //Assert
            _ruleEngine.Verify(e => e.ExecuteRules(order), Times.Once);
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void Validate_Process_returns_500_responseCode()
        {
            //Arrange
            var order = new Order();
            _ruleEngine.Setup(e => e.ExecuteRules(order)).Throws(new Exception());

            //Act
            var result = _controller.Object.Process(order);

            //Assert
            Assert.That((result as StatusCodeResult).StatusCode, Is.EqualTo(500));
        }
    }
}
