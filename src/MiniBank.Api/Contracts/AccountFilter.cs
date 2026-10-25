using MiniBank.Api.Models;
using MiniBank.Api.Repositories;

namespace MiniBank.Api.Contracts
{
    public class AccountFilter
    {
        public string? Currency {  get; set; } 
        public string? Owner {  get; set; } 
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
        
    }
}
