using System.ComponentModel.DataAnnotations;

namespace MiniBank.Api.Contracts
{
    public class MoneyOperationRequest
    {
        [Range(typeof(decimal), "0.01", "1000000", 
            ParseLimitsInInvariantCulture = true)]
        public decimal Amount { get; set; }
    }
}
