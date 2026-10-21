using MiniBank.Api.Models;
using System.Collections.Concurrent;

namespace MiniBank.Api.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<Guid, Account> _accounts = new();
        public IReadOnlyList<Account> GetAll()
        {
            return _accounts.Values.OrderBy(x => x.CreatedAtUtc).ToList();
        }
        public Account? GetById(Guid accountId)
        {
            _accounts.TryGetValue(accountId, out var account);
            return account;
        }
        public void Add(Account account)
        {

            if (!_accounts.TryAdd(account.Id, account))
                throw new InvalidOperationException("Cannot add a new account");
        }
    }
}
