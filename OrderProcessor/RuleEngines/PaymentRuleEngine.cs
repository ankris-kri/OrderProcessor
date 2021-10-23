using OrderProcessor.Models;
using OrderProcessor.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderProcessor.RuleEngines
{
    public interface IPaymentRuleEngine
    {
        void ExecuteRules(Order order);
    }

    public class PaymentRuleEngine : IPaymentRuleEngine
    {
        private readonly IEnumerable<PaymentRuleBase> _rules;

        public PaymentRuleEngine(IEnumerable<PaymentRuleBase> rules)
        {
            _rules = rules;
        }

        public void ExecuteRules(Order order)
        {
            foreach(var rule in _rules)
            {
                if (rule.IsMatch(order))
                {
                    rule.Execute(order);
                }
            }
        }
    }
}
