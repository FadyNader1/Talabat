# Talabat E-Commerce Backend

A modular, secure, and scalable e-commerce backend API built with ASP.NET Core (.NET 6).  
This project provides endpoints for user authentication, product management, basket operations, order processing, payment integration, and supports advanced features like pagination and filtering, following clean architecture and best practices.

---

## Features

- **User Authentication & Authorization**
  - JWT-based authentication for secure API access.
  - User registration, login, profile management, and password reset.
- **Product Management**
  - CRUD operations for products, with advanced filtering, searching, and pagination.
- **Basket/Cart Management**
  - Create, update, retrieve, delete, and clear customer baskets.
  - Basket data is cached using Redis for high performance.
- **Order Management**
  - Place new orders, retrieve order details, and list user orders (with pagination support).
- **Payment Integration**
  - Stripe integration for secure payment processing.
- **Email Notifications**
  - SMTP-based email service for password recovery and notifications.
- **Error Handling**
  - Consistent, structured API error responses.
- **Pagination**
  - Built-in pagination for product and order listings to optimize data delivery and user experience.
- **API Documentation**
  - Swagger/OpenAPI for interactive API exploration and testing.

---

## Technologies Used

- .NET 6 / ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- Microsoft Identity (User management & authentication)
- AutoMapper (Object mapping)
- Redis (Basket caching)
- Stripe API (Payments)
- JWT (Authentication)
- SMTP (Email)
- Swagger/OpenAPI (API documentation)
- Postman

---

## Architecture & Design Patterns

- **Repository Pattern:** Abstracts data access logic and provides a clean interface for CRUD operations.
- **Unit of Work Pattern:** Manages transactions and coordinates changes across multiple repositories.
- **Specification Pattern:** Encapsulates query logic, filtering, and pagination for repositories.
- **Dependency Injection:** All services, repositories, and managers are injected via constructors.
- **DTO Pattern:** Separates API models from domain entities for security and flexibility.
- **Service Pattern:** Encapsulates business logic (e.g., token and email services).
- **Mapper Pattern:** Uses AutoMapper to map between entities and DTOs.

---

## Main Controllers

- **Accountcontroller:**  
  User registration, login, profile, password reset.
- **Productcontroller:**  
  Product CRUD, filtering, searching, and pagination.
- **Basketcontroller:**  
  Basket/cart operations.
- **Ordercontroller:**  
  Order placement, retrieval, and paginated listing.
- **Paymentcontroller:**  
  Payment intent creation, status checking, and Stripe webhook handling.

---

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server
- Redis
- Stripe account (for payment integration)

### Configuration

1. **Clone the repository:**
   git clone https://github.com/your-username/talabat-backend.git cd talabat-backend

2. **Update `appsettings.json`:**  
   Set your SQL Server, Redis, JWT, Email, and Stripe credentials.

3. **Apply Migrations:**
   dotnet ef database update

4. **Run the application:**
   dotnet run

## 🧪 Testing the Endpoints

### ✅ Postman Collection
📥 [Download Talabat.postman_collection.json](./Talabat.postman_collection.json)

### ⚙️ How to Use

1. Open **Postman**.
2. Click on **Import**.
3. Select the downloaded `.json` file.
4. Start testing all available API endpoints with ease 🎯

## API Endpoints Overview

### Account

- `POST /api/Account/Register` — Register a new user
- `POST /api/Account/Login` — User login
- `GET /api/Account/CurrentUser` — Get current user info (JWT required)
- `POST /api/Account/UpdateUserProfile` — Update user profile (JWT required)
- `POST /api/Account/ForgetPassword` — Send password reset link
- `POST /api/Account/ResetPassword` — Reset password

### Basket

- `POST /api/Basket/CreateOrUpdateBasket` — Create or update basket
- `GET /api/Basket/GetBasketById?id={basketId}` — Get basket by ID
- `DELETE /api/Basket/DeleteBasket?id={basketId}` — Delete basket
- `GET /api/Basket/GetAllBaskets` — Get all baskets
- `DELETE /api/Basket/ClearBasket` — Clear all baskets

### Product

- `GET /api/Product/GetAllProducts?pageIndex={pageIndex}&pageSize={pageSize}&...` — Get all products (with filtering & pagination)
- `GET /api/Product/GetProductById?id={productId}` — Get product by ID
- `POST /api/Product/AddProduct` — Add new product
- `PUT /api/Product/UpdateProduct?id={productId}` — Update product
- `DELETE /api/Product/DeleteProduct?id={productId}` — Delete product

### Order

- `POST /api/Order/CreateOrder` — Place a new order
- `GET /api/Order/GetOrderById?id={orderId}` — Get order by ID
- `GET /api/Order/GetOrdersForUser?pageIndex={pageIndex}&pageSize={pageSize}` — Get all orders for current user (with pagination)

### Payment

- `POST /api/Payment/CreateOrUpdatePaymentIntent/{basketId}` — Create or update payment intent
- `POST /api/Payment/webhook` — Stripe webhook endpoint

---

## Folder Structure
Talabat/ │ ├── Core/ │   ├── Entities/ │   ├── Interfaces/ │   └── Specifications/ │ ├── Repository/ │   ├── Context/ │   ├── Repositories/ │   └── Specifications/ │ ├── Services/ │   └── Services/ │ ├── Controllers/ │   └── (All API controllers) │ ├── DTO/ │   └── (All Data Transfer Objects) │ ├── Errors/ │   └── (Custom error classes) │ ├── appsettings.json └── Program.cs / Startup.cs

---

## License

This project is licensed under the MIT License.

---

**Note:**  
Replace endpoint URLs and sections as needed to match your actual implementation and folder structure.
