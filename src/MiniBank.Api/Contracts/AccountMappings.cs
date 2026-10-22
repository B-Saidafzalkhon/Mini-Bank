using MiniBank.Api.Models;

namespace MiniBank.Api.Contracts
{
    public static class AccountMappings
    {
        public static AccountResponse ToResponse(this Account account)
        {
            return new AccountResponse
            {
                Id = account.Id,
                OwnerName = account.OwnerName,
                Currency = account.Currency,
                Balance = account.Balance,
                CreatedAtUtc = account.CreatedAtUtc
            };
        }
    }
}
