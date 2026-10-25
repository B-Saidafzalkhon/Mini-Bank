using MiniBank.Api.Models;
using MiniBank.Api.Repositories;

namespace MiniBank.Api.Contracts
{
    public class AccountFilter
    {
        public string? Currency {  get; set; } = string.Empty;
        public string? Owner {  get; set; } = string.Empty;
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
        
    }
}
