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
        public AccountsController(IAccountRepository repository )
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IReadOnlyList<Account>> GetAll()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Account> GetAccount(Guid id)
        {
            var account = _repository.GetById(id);
            if(account is null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public ActionResult<Account> Create(CreateAccountRequest request)
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

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
    }
}
