using MiniBank.Api.Models;
using MiniBank.Api.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MiniBank.Api.Contracts
{
    public class AccountFilter : IValidatableObject
    {
        public string? Currency {  get; set; } 
        public string? Owner {  get; set; } 
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(MinBalance is not null && MaxBalance is not null && MinBalance > MaxBalance)
            {
                yield return new ValidationResult(
                    "Max balance must be higher than min balance!",
                    new[] { nameof(MaxBalance) }
                    );
            }
        }
    }
}
