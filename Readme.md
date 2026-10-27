# MiniBank.Api

MiniBank.Api is an educational REST API for managing bank accounts. It supports account creation, filtering, deposits, and withdrawals while enforcing input validation and basic business rules.

## Features

- Create bank accounts with an owner name and currency.
- Retrieve an account by its ID.
- List accounts with optional filters for currency, owner name, and balance range.
- Deposit money into an account.
- Withdraw money with insufficient-funds protection.
- Validate incoming requests using data annotations.
- Return structured error responses using `ProblemDetails`.
- Log application events and errors.
- Assign a correlation ID to each request to help trace requests and diagnose errors.
- Provide a health check endpoint.

## Tech stack

- **.NET 10** (`net10.0`)
- **ASP.NET Core Web API**
- **C#**
- **In-memory storage** — account data is stored in memory and is lost when the application restarts.
- **OpenAPI / Swagger UI** — API documentation and testing.

## Getting started

### Requirements

Install the following tools:

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Git](https://git-scm.com/)

### Clone the repository

```bash
git clone https://github.com/B-Saidafzalkhon/Mini-Bank.git
cd Mini-Bank/src/MiniBank.Api
```

### Run the application

```bash
dotnet run
```

The application will start at the address shown in the terminal. The exact address and port depend on the launch configuration.

### Swagger / OpenAPI

In the Development environment, open Swagger UI at:

```text
http://localhost:5293/swagger
```

If the application uses a different port, replace `5293` with the actual port.

The OpenAPI JSON document is available at:

```text
/openapi/v1.json
```

### HTTP request file

The `MiniBank.Api.http` file contains sample HTTP requests for testing the API, including account creation, retrieving accounts, deposits, withdrawals, and error scenarios.

You can run these requests using an editor that supports `.http` files, such as Visual Studio or Visual Studio Code with a suitable extension.

## API endpoints

| Method | Route | Description | Responses |
|---|---|---|---|
| `GET` | `/api/accounts` | List accounts with optional filters | `200`, `400` |
| `GET` | `/api/accounts/{id}` | Retrieve an account by ID | `200`, `404` |
| `POST` | `/api/accounts` | Create a new account | `201`, `400` |
| `POST` | `/api/accounts/{id}/deposit` | Deposit money into an account | `200`, `400`, `404` |
| `POST` | `/api/accounts/{id}/withdraw` | Withdraw money from an account | `200`, `400`, `404`, `409` |
| `GET` | `/health` | Check application health | `200`, `503` |

### Account list filters

The `GET /api/accounts` endpoint supports the following optional query parameters:

| Parameter | Description | Example |
|---|---|---|
| `currency` | Filter by account currency | `?currency=EUR` |
| `owner` | Filter by part of the owner's name (case-insensitive) | `?owner=ivan` |
| `minBalance` | Minimum balance, inclusive | `?minBalance=100` |
| `maxBalance` | Maximum balance, inclusive | `?maxBalance=1000` |

Example:

```http
GET /api/accounts?currency=EUR&minBalance=100&maxBalance=1000
```

### Request examples

**Create an account**

```http
POST /api/accounts
Content-Type: application/json

{
  "ownerName": "Ivan",
  "currency": "EUR"
}
```

**Deposit money**

```http
POST /api/accounts/{id}/deposit
Content-Type: application/json

{
  "amount": 100
}
```

**Withdraw money**

```http
POST /api/accounts/{id}/withdraw
Content-Type: application/json

{
  "amount": 30
}
```

## Validation and business rules

### Input validation — `400 Bad Request`

Incoming requests are validated before processing. Invalid input results in a `400 Bad Request` response.

- `ownerName` is required and must contain between 2 and 100 characters.
- `currency` is required and must contain exactly three Latin letters. The currency code is converted to uppercase before storage.
- `amount` must be between `0.01` and `1,000,000`.
- When filtering accounts, `minBalance` must not be greater than `maxBalance`.

### Business rules and resource errors

- **`404 Not Found`** — the requested account does not exist.
- **`409 Conflict`** — the account has insufficient funds for a withdrawal.
- A newly created account starts with a balance of `0`.
- Account data is stored in memory and is lost when the application restarts.

Application errors are handled through exception-handling middleware and returned using the `ProblemDetails` format.

## Project structure

```text
Mini-Bank/
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

- **`Contracts/`** — request and response models, account filters, and mapping between internal models and API responses.
- **`Controllers/`** — HTTP endpoints and request handling for account operations.
- **`Middleware/`** — request middleware and extension methods. A correlation ID helps associate log entries with the same request, making debugging and troubleshooting easier.
- **`Models/`** — internal bank account models.
- **`Repositories/`** — repository interfaces and the in-memory storage implementation.
- **`MiniBank.Api.http`** — sample HTTP requests for manual API testing.
- **`Program.cs`** — application configuration, dependency injection, and HTTP pipeline setup.
- **`MiniBank.Api.csproj`** — target .NET version and project dependencies.

## Roadmap

- [ ] Add persistent storage using Entity Framework Core and a database.
- [ ] Implement account transaction history.
- [ ] Add transfers between accounts with validation and atomic operations.
- [ ] Implement joint accounts with multiple owners.