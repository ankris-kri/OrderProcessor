using Autofac;
using OrderProcessor.RuleEngines;
using OrderProcessor.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderProcessor.Registry
{
    public class PaymentRulesRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //RuleEngine
            builder.RegisterType<PaymentRuleEngine>().As<IPaymentRuleEngine>();

            //Autodetect and Register all rules containing keyword 'PaymentRule'
            var assembly = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.BaseType == typeof(PaymentRuleBase)
            && t.Name.IndexOf("PaymentRule", 0, StringComparison.CurrentCultureIgnoreCase) != -1).As<PaymentRuleBase>();
        }
    }
}
