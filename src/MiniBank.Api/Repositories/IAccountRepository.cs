using MiniBank.Api.Models;

namespace MiniBank.Api.Repositories
{
    public interface IAccountRepository
    {
        IReadOnlyList<Account> GetAll();
        Account? GetById(Guid id);
        void Add(Account account);
    }
}
