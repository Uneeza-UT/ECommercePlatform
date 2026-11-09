# E-Commerce Platform API

An ASP.NET Core Web API for an e-commerce platform, providing product
and category management, customer shopping carts, order processing,
inventory management, coupons and discounts, delivery settings, and
payment handling.

The API is built with C# and Entity Framework Core, uses SQL Server for
data persistence, and implements JWT authentication and role-based
authorization to protect customer and administrative operations.

------------------------------------------------------------------------

## Features

-   User registration and login
-   JWT authentication
-   Role-based authorization
    -   Admin
    -   Customer
-   Guest checkout without requiring customer registration
-   Product management
-   Category management
-   Product search, sort and filtering
-   Shopping cart management
-   Order placement and order history
-   Order item management
-   Inventory and stock management
-   Coupon and discount management
-   Customer coupon assignment and usage tracking
-   Delivery fee configuration
-   Shipping details management
-   Payment records and payment type handling
-   Order status management
-   DTO-based API contracts
-   Repository and service patterns
-   Input validation using FluentValidation
-   Entity Framework Core with SQL Server
-   Swagger / OpenAPI documentation
-   Global error handling

------------------------------------------------------------------------

## User Roles

### Admin

Administrative operations include:

-   Manage categories
-   Manage products
-   Manage orders
-   Manage coupons and discounts
-   Manage delivery settings
-   Access administrative operations protected by role-based
    authorization

### Customer

Customer operations include:

-   Register and log in
-   Browse and search products
-   View product and category information
-   Manage shopping cart items
-   Place orders
-   Provide shipping details
-   Select an available payment type
-   View their orders and order history
-   Use eligible coupons and discounts

Access to customer-specific resources is restricted so that customers
can only access their own data.

------------------------------------------------------------------------

## Technologies

-   ASP.NET Core Web API
-   C#
-   Entity Framework Core
-   SQL Server
-   ASP.NET Core Identity
-   JWT Bearer Authentication
-   Repository and Service Patterns
-   DTOs
-   FluentValidation
-   AutoMapper
-   Swagger / OpenAPI
-   Resend
-   Clean Architecture

------------------------------------------------------------------------

## Architecture

The project follows a layered architecture based on Clean Architecture
principles.

``` text
ECommercePlatform
│
├── API
│   ├── wwwroot
│   ├── Controllers
│   ├── Middleware
│   ├── Models
│   └── Program.cs
│
├── Application
│   ├── Contracts
│   │   ├── Email
│   │   ├── Identity
│   │   ├── Persistence
│   │   └── Services
│   │
│   ├── DTOs
│   ├── Exceptions
│   ├── Mapping Profiles
│   ├── Models
│   └── Validations
│
├── Domain
│   ├── Common
│   ├── Enums
│   └── Entities
│
└── Infrastructure
    ├── Identity
    │   ├── Configurations
    │   ├── DbContext
    │   └── Models
    │   └── Services
    │
    └── Infrastructure       
    │   ├── Email Service
    │   └── Services
    │
    └── Persistence
        └── Configurations
        ├── DbContext
        ├── Repositories

```

The API layer handles HTTP requests, the Application layer contains
application logic and contracts, the Domain layer defines core entities
and enums, and the Infrastructure layer handles persistence and external
implementation details.

------------------------------------------------------------------------

## Authentication

The API uses ASP.NET Core Identity for user management and JWT Bearer
tokens for authentication.

After a successful login, the API returns a JWT token. Authenticated
requests use the following header:

``` http
Authorization: Bearer <token>
```

Most customer and administrative operations require authentication.
Access to protected operations is controlled through role-based
authorization.

------------------------------------------------------------------------

## Using Swagger

1.  Use the **Login** endpoint to log in with an existing account, or
    create a new account using the **Register** endpoint.
2.  After a successful login response (HTTP 200), Swagger automatically
    retrieves the JWT token from the response and applies it to
    subsequent authorized requests.
3.  You can then test protected endpoints without manually copying the
    token into Swagger's Authorize dialog.


------------------------------------------------------------------------

## Authorization

Authorization is enforced using roles and resource ownership.

Examples include:

-   Administrative endpoints are restricted to users with the Admin
    role.
-   Customer operations require the Customer role where applicable.
-   Customers can only access their own carts and orders.
-   Customers cannot access another customer's order or other
    customer-specific resources.

------------------------------------------------------------------------

## Product and Category Management

The API provides operations for managing products and categories.

Product information includes details such as:

-   Product name and description
-   Price
-   Stock quantity
-   Category
-   Product image
-   Discount information, where applicable
-   Sales information

Products can be organized by category and retrieved using product search,
sort and filtering functionality.

------------------------------------------------------------------------

## Shopping Cart

Customers can manage their shopping cart before placing an order.

Cart functionality includes:

-   Adding products to the cart
-   Updating item quantities
-   Removing items
-   Retrieving cart contents

Cart and cart-item data are persisted in the database.

------------------------------------------------------------------------

## Order Management

Registered Customers can place orders using the products in their shopping cart.

Orders contain information about the customer, order items, payment,
shipping details, applicable coupon usage, and order totals.

Order items preserve the relevant product and pricing information
associated with an order.

The supported order statuses are:

``` text
Pending
Confirmed
Shipped
Delivered
Cancelled
```

Customers can retrieve their own orders and order history.
Administrative users can manage orders according to their authorization
permissions.

------------------------------------------------------------------------

## Guest Checkout

The platform supports guest checkout, allowing customers to place orders without creating an account or logging in.

Guest checkout functionality includes:
- Placing orders without user registration or authentication.
- Providing shipping details during checkout.
- Creating orders without associating them with a registered customer account.
- Supporting both registered and guest customers through the same order processing workflow.


The CustomerId property in the Order entity is nullable:
- For registered customers, the order is associated with their customer account.
- For guest customers, CustomerId is null.


Shipping details are stored separately and associated with the order, ensuring that guest orders contain the necessary delivery information.

Note: Shopping cart functionality is available to authenticated customers. Guest customers can proceed with checkout without maintaining a persistent customer cart.

------------------------------------------------------------------------

## Inventory Management

The API maintains product stock quantities.

Inventory is considered during order processing to help prevent orders
from being placed for unavailable quantities. Product sales information
is also maintained.

------------------------------------------------------------------------

## Coupons and Discounts

The platform supports coupon-based discounts.

Coupon configuration can include:

-   Coupon code
-   Discount type
-   Discount value
-   Minimum order amount
-   Maximum discount amount
-   Usage limit
-   Usage count
-   Validity period
-   Active or inactive status

Customer coupon assignments and coupon usage are tracked separately.
This supports customer-specific coupon availability and prevents a
customer's coupon usage from being treated as unrestricted.

------------------------------------------------------------------------

## Delivery and Shipping

Delivery settings are maintained centrally and include delivery fee
configuration for different payment methods, such as:

-   Cash on Delivery
-   Online payment

Shipping details are associated with orders and contain the information
required for order delivery.

------------------------------------------------------------------------

## Payments

The API maintains payment information associated with orders.

The payment types defined for the platform include:

-   Cash on Delivery
-   Credit/Debit Card
-   Bank Account

Payment records are associated with their respective orders.

------------------------------------------------------------------------


## Email Notifications

Email notifications are implemented using Resend.

Emails are sent for events such as:

- Account registration
- Password change
- Password reset requests
- Order Placed, Confirmed, Shipped, Delivered, Cancelled.
- Coupon Assignment
------------------------------------------------------------------------


## Validation and Error Handling

Request validation is implemented using FluentValidation.

Validation is used to check incoming data before application operations
are performed.

The API also uses centralized error handling to return consistent HTTP
error responses.

Common HTTP status codes include:

-   `200 OK`
-   `201 Created`
-   `204 No Content`
-   `400 Bad Request`
-   `401 Unauthorized`
-   `403 Forbidden`
-   `404 Not Found`
-   `409 Conflict`
-   `500 Internal Server Error`

------------------------------------------------------------------------

## Prerequisites

-   .NET SDK compatible with the project target framework
-   SQL Server
-   Entity Framework Core tools
-   Visual Studio or another .NET-compatible IDE

------------------------------------------------------------------------

## Configuration

Before running the application, configure the required application
settings, including:

-   SQL Server connection string
-   JWT settings
-   Resend API key

Example configuration:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  },
  "JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "DurationInMinutes": 30
  },
  "EmailSettings": {
    "ApiKey": "your-secret-key",
    "FromAddress": "your-email-provider-email-address",
    "FromName": "your-email-provider-name"
  }
}
```

Use the configuration section names and properties defined in the
project.

Sensitive values, including database credentials and the JWT signing
key, should be configured through .NET User Secrets, environment
variables, or deployment platform settings rather than committed to
source control.

------------------------------------------------------------------------

## Database

The application uses SQL Server with Entity Framework Core.

The database includes entities for:

-   Identity Users and Roles
-   Products
-   Categories
-   Images
-   Cart
-   Cart items
-   Orders
-   Order items
-   Payments
-   Shipping details
-   Coupons
-   Customer coupons
-   Coupon usages
-   Delivery settings

Entity Framework Core migrations are used to manage database schema
changes.

------------------------------------------------------------------------

## Database Setup

1.  Create a SQL Server database or configure an existing SQL Server
    instance.
2.  Configure the connection string.
3.  Open the solution in Visual Studio.
4.  Migrations are committed to GitHub.
5.  After configuring the SQL Server connection string, run:

Select Persistence project as default project:
update-database -Context ApplicationDbContext

Select Identity project as default project:
update-database -Context ApplicationIdentityDbContext

6. Start the application.

------------------------------------------------------------------------

## Running the Application

Clone the repository:

``` bash
git clone <repository-url>
```

Navigate to the API project:

``` bash
cd <api-project-directory>
```

Restore dependencies:

``` bash
dotnet restore
```

Apply database migrations:

``` bash
dotnet ef database update
```

Run the application:

``` bash
dotnet run
```

------------------------------------------------------------------------

## Swagger / API Documentation

The API is documented using Swagger / OpenAPI.

Once the application is running, open:

``` text
https://localhost:<port>/swagger
```

Swagger can be used to explore and test the REST API endpoints.


------------------------------------------------------------------------

## Testing the API

A typical testing flow is:

1.  Configure the SQL Server connection string.
2.  Configure JWT settings.
3.  Apply the database migrations.
4.  Start the API.
5.  Open Swagger.
6.  Register a customer account or log in with an existing account.
7.  Use the product and category endpoints to browse the catalog.
8.  Add products to the customer's cart.
9.  Update or remove cart items as needed.
10. Configure shipping details and place an order.
11. Test the available payment types.
12. Test order retrieval and order history.
13. Test coupon validation and discount application.
14. Test administrative product, category, coupon, delivery, and order
    operations using an Admin account.
15. Test authorization by attempting to access resources belonging to
    another customer.
16. Test guest checkout by placing an order without logging in.
17. Verify that the guest order is created with a null CustomerId and its shipping details are saved correctly.

------------------------------------------------------------------------

## Project Status

The project is an ASP.NET Core e-commerce backend API.

It provides the core functionality for product and category management,
shopping carts, order processing, inventory, coupons, delivery settings,
payment records, authentication, and authorization.

The API is designed to be consumed by a frontend application.

------------------------------------------------------------------------

## Implementation Highlights

-   Clean Architecture principles
-   Repository and service patterns
-   ASP.NET Core Identity
-   JWT authentication
-   Role-based authorization
-   Customer resource ownership checks
-   Entity Framework Core with SQL Server
-   DTO-based request and response models
-   FluentValidation
-   AutoMapper
-   Product and category management
-   File attachment support for Product Images
-   Shopping cart and order processing
-   Inventory management
-   Coupon and discount handling
-   Customer coupon and usage tracking
-   Delivery and shipping management
-   Payment records
-   Email Notifications
-   Centralized error handling
-   Swagger/OpenAPI documentation
-   Automatic JWT handling in Swagger
