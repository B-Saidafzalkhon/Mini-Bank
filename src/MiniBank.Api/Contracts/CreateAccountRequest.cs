namespace MiniBank.Api.Contracts
{
    public class CreateAccountRequest
    {
        public string OwnerName { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
    }
}
    