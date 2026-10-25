using Microsoft.AspNetCore.Mvc;
using MiniBank.Api.Contracts;
using MiniBank.Api.Models;
using MiniBank.Api.Repositories;

namespace MiniBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        public AccountsController(IAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id:guid}")]
        public ActionResult<AccountResponse> GetAccount(Guid id)
        {
            var account = _repository.GetById(id);
            if (account is null)
            {
                return NotFound();
            }
            return Ok(account.ToResponse());
        }

        [HttpPost]
        public ActionResult<AccountResponse> Create(CreateAccountRequest request)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                OwnerName = request.OwnerName,
                Currency = request.Currency.ToUpperInvariant(),
                Balance = 0.0m,
                CreatedAtUtc = DateTime.UtcNow
            };

            _repository.Add(account);

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account.ToResponse());
        }

        [HttpPost("{id:guid}/deposit")]
        public ActionResult<AccountResponse> Deposit(Guid id, MoneyOperationRequest request)
        {
            var account = _repository.GetById(id);
            if (account is null)
                return NotFound();

            account.Balance += request.Amount;

            return Ok(account.ToResponse());
        }

        [HttpPost("{id:guid}/withdraw")]
        public ActionResult<AccountResponse> Withdraw(Guid id, MoneyOperationRequest request)
        {
            var account = _repository.GetById(id);

            if (account is null)
                return NotFound();

            if (account.Balance < request.Amount)
                return Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Insufficient funds"
                    );

            account.Balance -= request.Amount;
            return Ok(account.ToResponse());
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<AccountResponse>> GetAll([FromQuery] AccountFilter filter)
        {
            if (filter.MinBalance > filter.MaxBalance)
                return Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Invalid balance range",
                    detail: "Min balance cannot be higher than max balance");

            IEnumerable<Account> accounts = _repository.GetAll();

            if (filter.Currency is not null)
            {
                accounts = accounts.Where(a => a.Currency.Equals(filter.Currency, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.Owner is not null)
            {
                accounts = accounts.Where(a => a.OwnerName.Contains(filter.Owner, StringComparison.OrdinalIgnoreCase));
            }
            if (filter.MinBalance is not null)
            {
                accounts = accounts.Where(a => a.Balance >= filter.MinBalance);
            }
            if (filter.MaxBalance is not null)
            {
                accounts = accounts.Where(a => a.Balance <= filter.MaxBalance);
            }
            return Ok(accounts.Select(a => a.ToResponse()).ToList());
        }
    }
}
