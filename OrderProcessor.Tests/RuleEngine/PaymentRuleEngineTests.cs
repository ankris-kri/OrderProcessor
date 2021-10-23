using Moq;
using NUnit.Framework;
using OrderProcessor.Models;
using OrderProcessor.RuleEngines;
using OrderProcessor.Rules;
using System.Collections.Generic;

namespace OrderProcessor.Tests.RuleEngine
{
    [TestFixture]
    public class PaymentRuleEngineTests
    {
        Mock<PaymentRuleEngine> _ruleEngine;
        Mock<PaymentRuleBase> _rule;
        List<PaymentRuleBase> _rules;

        [SetUp]
        public void Setup()
        {
            _rule = new Mock<PaymentRuleBase>();
            _rules = new List<PaymentRuleBase>();
            _rules.Add(_rule.Object);

            _ruleEngine = new Mock<PaymentRuleEngine>(_rules) { CallBase = true };
        }

        [Test]
        public void Validate_ExecuteRules_should_invoke_Rule_Execute_method()
        {
            //Arrange
            var order = new Order();
            _rule.Setup(r => r.IsMatch(order)).Returns(true);
            _rule.Setup(r => r.Execute(order));

            //Act
            _ruleEngine.Object.ExecuteRules(order);

            //Assert
            _rule.Verify(r => r.Execute(order), Times.Once);
        }

        [Test]
        public void Validate_ExecuteRules_should_NOT_invoke_Rule_Execute_method()
        {
            //Arrange
            var order = new Order();
            _rule.Setup(r => r.IsMatch(order)).Returns(false);
            _rule.Setup(r => r.Execute(order));

            //Act
            _ruleEngine.Object.ExecuteRules(order);

            //Assert
            _rule.Verify(r => r.Execute(order), Times.Never);
        }
    }
}
