# Payment Service

Welcome to the Payment Service documentation! This service provides merchants with a secure and efficient way to process payments from shoppers using their card information. 

## Table of Contents
- [Interacting with Payment Service](#interacting-with-payment-service)
- [Transaction Endpoints](#transaction-endpoints)
- [Technical Details](#technical-details)
  - [System Requirements](#system-requirements)
  - [Code Structure](#code-structure)
- [Assumptions Made](#assumptions-made)
- [Areas for Improvement](#areas-for-improvement)
- [Cloud Technologies](#cloud-technologies)
- [Extra Mile](#extra-mile)

## Interacting with Payment Service

The Payment Service requires merchants to register and obtain an authorization token for making any requests. Below are the authentication endpoints:

1. Register as a merchant:
    ```bash
    curl -X 'POST' \
      'http://localhost:5045/api/Auth/Register' \
      -H 'accept: text/plain' \
      -H 'Content-Type: application/json' \
      -d '{
      "merchantName": "Apple",
      "password": "password123"
    }'
    ```

2. Login and generate an authorization token:
    ```bash
    curl -X 'POST' \
      'http://localhost:5045/api/Auth/Login' \
      -H 'accept: text/plain' \
      -H 'Content-Type: application/json' \
      -d '{
      "merchantName": "Apple",
      "password": "password123"
    }'
    ```

## Transaction Endpoints

Once authorized, merchants can initiate payments and retrieve transaction history. Below are the transaction endpoints:

1. Initiate a payment:
    ```bash
    curl -X 'POST' \
      'http://localhost:5045/api/Transaction/Pay' \
      -H 'accept: text/plain' \
      -H 'Authorization: [AuthToken]' \
      -H 'Content-Type: application/json' \
      -d '{
      "cardHolderName": "Peter Parker",
      "cardNo": "4123456789123456",
      "expiryDate": "0225",
      "cvv": "123",
      "amount": 90.20,
      "currency": "GBP"
    }'
    ```

2. Retrieve all transactions by the merchant:
    ```bash
    curl -X 'GET' \
      'http://localhost:5045/api/Transaction/Pay' \
      -H 'accept: text/plain' \
      -H 'Authorization: [AuthToken]'
    ```

## Technical Details

### System Requirements

To run the Payment Service, you need the following:

- .NET 7 Framework
- Visual Studio 2022
- SQL Server Express 2022
- Any modern browser


### Code Structure

- `CKOBankSimulator`: This folder contains classes simulating a bank with three dummy customers. The `CKOBankSimulator.cs` implements basic validations and money detection from user accounts.
- `Controllers`: Contains the following endpoints:
  - `Auth`: For Merchant Authorization
  - `Transaction`: For handling payment and transaction retrieval
- `Service`: Utilizes a Repository pattern to inject services into controllers.
  - `AuthService`: Implements `IAuthService` and handles merchant registration and login operations.
  - `TransactionService`: Implements `ITransactionService` and handles payment and transaction retrieval logic.
- `Data`: Defines the `DataContext` and `DataSet` for mapping SQL Database objects using Entity Framework.
- `Model`: Contains `Transaction` and `Merchant` models to hold properties stored in the DB.
- `Dto`: Contains data transfer objects used to encapsulate data and pass it between different layers of the application (Automapper used to map models and DTOs).

## Assumptions Made

- All payment requests are assumed to be in GBP (British Pound Sterling).

## Areas for Improvement

- Implement a Global Exception Handler to handle exceptions in the Payment API.
- Log every request and response JSON with sensitive information masked.
- Add unit test cases to ensure robustness and reliability.

## Cloud Technologies

For cloud deployment, consider using the following technologies:

- **Amazon ELB**: To implement load balancing and distribute incoming requests efficiently.
- **Amazon CloudWatch**: For monitoring and logging the services, providing valuable insights for debugging and performance optimization.

## Extra Mile

- JWT token authentication is implemented to authorize merchants and prevent unauthorized access to the payment service.
- The merchant ID is extracted from the token whenever a request is made, ensuring proper identification and authorization.
- Dtos ,Repository Pattern and Automapper(for Object - Object mapping) has been used.
- 
