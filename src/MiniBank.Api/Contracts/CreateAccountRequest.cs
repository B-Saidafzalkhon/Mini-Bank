using System.ComponentModel.DataAnnotations;

namespace MiniBank.Api.Contracts
{
    public class CreateAccountRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string OwnerName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z]{3}$", ErrorMessage = "The currency must consist of exactly 3 Latin letters.")]
        public string Currency { get; set; } = string.Empty;
    }
}
    