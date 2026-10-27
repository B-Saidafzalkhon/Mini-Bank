# MiniBank.Api

MiniBank.Api is an educational REST API for managing bank accounts. It supports creating accounts, retrieving and filtering them, depositing money, and withdrawing funds while enforcing basic validation and business rules.

## Features

- Create bank accounts with an owner name and currency.
- Retrieve an account by its `id`.
- List accounts with optional filters for currency, owner name, and balance range.
- Deposit money into an account.
- Withdraw money from an account.
- Validate incoming request data.
- Return errors using the `ProblemDetails` format.
- Log key application operations.
- Add the `MiniBank-Correlation-Id` and `X-Server-Name` headers to responses.
- Check application health through `/health`.

## Tech stack

- **.NET 10** (`net10.0`)
- **ASP.NET Core Web API**
- **C#**
- **In-memory storage** — account data is stored in application memory and is lost when the application restarts.
- **OpenAPI / Swagger UI** — API documentation and manual testing.

## Getting started

### Requirements

Install the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) and Git.

### Clone the repository

```bash
git clone <repository-url>
cd MiniBank/src/MiniBank.Api
```

Replace `<repository-url>` with the URL of your Git repository.

### Run the application

```bash
dotnet run
```

The application starts at the address shown in the terminal. The included `.http` file uses `http://localhost:5293`; the actual address may differ depending on your launch configuration.

### Swagger / OpenAPI

In the Development environment, open Swagger UI at:

```text
http://localhost:5293/swagger
```

If the application uses a different address or port, replace it with the actual one. The OpenAPI JSON document is available at `/openapi/v1.json`.

### HTTP request file

The `MiniBank.Api.http` file contains sample requests for creating an account, listing accounts, retrieving an account by `id`, depositing and withdrawing money, and checking error responses. You can run these requests from an editor that supports `.http` files, such as Visual Studio or VS Code with a suitable extension.

## API endpoints

| Method | Route | Description | Responses |
|---|---|---|---|
| `GET` | `/api/accounts` | List accounts with optional filters | `200`, `400` |
| `GET` | `/api/accounts/{id}` | Get an account by `id` | `200`, `404` |
| `POST` | `/api/accounts` | Create a new account | `201`, `400` |
| `POST` | `/api/accounts/{id}/deposit` | Deposit money into an account | `200`, `400`, `404` |
| `POST` | `/api/accounts/{id}/withdraw` | Withdraw money from an account | `200`, `400`, `404`, `409` |
| `GET` | `/health` | Check application health | `200`, `503` |

### List filters

The `GET /api/accounts` endpoint supports these optional query parameters:

| Parameter | Description | Example |
|---|---|---|
| `currency` | Filter by account currency | `?currency=EUR` |
| `owner` | Filter by part of the owner's name; case-insensitive | `?owner=ivan` |
| `minBalance` | Minimum balance, inclusive | `?minBalance=100` |
| `maxBalance` | Maximum balance, inclusive | `?maxBalance=1000` |

Example:

```http
GET /api/accounts?currency=EUR&minBalance=100&maxBalance=1000
```

### Request examples

Create an account:

```http
POST /api/accounts
Content-Type: application/json

{
  "ownerName": "Ivan",
  "currency": "EUR"
}
```

Deposit money:

```http
POST /api/accounts/{id}/deposit
Content-Type: application/json

{
  "amount": 100
}
```

Withdraw money:

```http
POST /api/accounts/{id}/withdraw
Content-Type: application/json

{
  "amount": 30
}
```

## Validation and business rules

### Input validation — `400 Bad Request`

ASP.NET Core validates incoming request models. If a request fails validation, the API returns `400 Bad Request`.

- `ownerName` is required and must contain between 2 and 100 characters.
- `currency` is required and must contain exactly three Latin letters. The currency code is converted to uppercase before it is stored.
- `amount` must be between `0.01` and `1,000,000`.
- For account list filters, `minBalance` must not be greater than `maxBalance`.

### Business rules and resource errors

- If an account with the specified `id` does not exist, the API returns `404 Not Found`.
- If an account has insufficient funds for a withdrawal, the API returns `409 Conflict` with an `Insufficient funds` error.
- A newly created account starts with a balance of `0`.
- Accounts are stored in memory, so all account data is lost when the application restarts.

Application errors are handled through exception-handling middleware and the `ProblemDetails` mechanism.

## Project structure

```text
MiniBank/
└── src/
    └── MiniBank.Api/
        ├── Contracts/
        │   ├── AccountFilter.cs
        │   ├── AccountMappings.cs
        │   ├── AccountResponse.cs
        │   ├── CreateAccountRequest.cs
        │   └── MoneyOperationRequest.cs
        ├── Controllers/
        │   └── AccountsController.cs
        ├── Middleware/
        │   ├── CorrelationIdMiddleware.cs
        │   └── CorrelationIdMiddlewareExtensions.cs
        ├── Models/
        │   └── Account.cs
        ├── Repositories/
        │   ├── IAccountRepository.cs
        │   └── InMemoryAccountRepository.cs
        ├── MiniBank.Api.http
        ├── MiniBank.Api.csproj
        └── Program.cs
```

- **`Contracts/`** — request and response models, account filters, and mapping from the account model to DTOs.
- **`Controllers/`** — HTTP endpoints and request handling for account operations.
- **`Middleware/`** — middleware for response headers and request correlation context.
- **`Models/`** — the internal bank account model.
- **`Repositories/`** — the repository interface and in-memory account storage implementation.
- **`MiniBank.Api.http`** — sample HTTP requests for manual testing.
- **`Program.cs`** — service registration and HTTP pipeline configuration.
- **`MiniBank.Api.csproj`** — target .NET version and project dependencies.

## Roadmap

- [ ] Add persistent storage using Entity Framework Core and a database.
- [ ] Implement account transaction history.
- [ ] Add transfers between accounts, including currency checks and atomic operations.
- [ ] Implement joint accounts with multiple owners.
