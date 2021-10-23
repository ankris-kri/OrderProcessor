using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessor.Models;
using OrderProcessor.RuleEngines;

namespace OrderProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRuleEngine _ruleEngine;
        public PaymentController(IPaymentRuleEngine ruleEngine)
        {
            _ruleEngine = ruleEngine;
        }

        [HttpPost]
        public IActionResult Process(Order order)
        {
            try
            {
                _ruleEngine.ExecuteRules(order);
                return Ok(order);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
