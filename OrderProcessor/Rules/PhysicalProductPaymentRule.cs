﻿using OrderProcessor.Models;
using System;

namespace OrderProcessor.Rules
{
    public class PhysicalProductPaymentRule : PaymentRuleBase
    {
        public override void Execute(Order order)
        {
            throw new NotImplementedException();
        }

        public override bool IsMatch(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
