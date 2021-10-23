using OrderProcessor.Models;

namespace OrderProcessor.Rules
{
    public abstract class RuleBase
    {
        public abstract bool IsMatch(Order order);
        public abstract void Execute(Order order);
    }
}
