# Laza WebAPI

Laza WebAPI is a backend API built with ASP.NET Core for a mobile e-commerce application. It provides all the necessary endpoints to manage categories, products, carts, orders, user authentication/authorization, and more. The application follows **Clean Architecture** principles for better maintainability and scalability.

## 🌟 Features

- **User Authentication & Authorization**:
  - User registration and login.
  - Social login via Google, Facebook, and Twitter.
  - Password recovery using verification codes.
  - Role-based authorization.

- **Product Management**:
  - View categories and products.
  - Search and filter products.
  - Product reviews for user feedback.

- **Shopping Cart**:
  - Add, update, and remove items from the cart.
  - Retrieve cart details for the logged-in user.

- **Order Management**:
  - Place orders and view order history.
  - Cards processing.

- **Address Management**:
  - Add addresses.
  - Set default shipping addresses.

- **Card Management**:
  - Save payment card details securely.
  - Retrieve, update, and delete stored cards.

- **Email Services**:
  - Send user Reset Password Verification Code mail.
  - Send order confirmation emails upon order placement.

- **Admin Features**:
  - Manage categories, products, and orders.
  - Assign roles to users.

- **Wishlist**:
  - Add and remove products from the wishlist.
  - Retrieve the wishlist for the logged-in user.

- **Product Reviews**:
  - Submit reviews for products.
  - View average ratings and feedback from other users.

## 🚀 Getting Started

### Prerequisites
Make sure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- SQL Server or any compatible database.

### Installation Steps

1. **Clone the repository:**
    ```bash
    git clone https://github.com/amaalselim/Laza-WebAPI.git
    ```

2. **Navigate to the project directory:**
    ```bash
    cd Laza-WebAPI
    ```

3. **Restore NuGet packages:**
    ```bash
    dotnet restore
    ```

4. **Update the database connection string in `appsettings.json`.**

5. **Apply migrations:**
    ```bash
    dotnet ef database update
    ```

6. **Run the application:**
    ```bash
    dotnet run
    ```
    The API will be available at https://localhost:5001 or http://localhost:5000.

## 📖 Documentation
Explore the API documentation and test endpoints using Swagger:
[API Documentation](https://laza.runasp.net/swagger/index.html)

## 🛠️ Technologies Used
- **ASP.NET Core Web API:** Framework for building APIs.
- **Entity Framework Core:** ORM for database operations.
- **SQL Server:** Database for storing user, product, and order data.
- **JWT Authentication:** For secure authentication and token-based authorization.
- **Clean Architecture:** A design pattern for structuring applications with clear separation of concerns.
- **Swagger:** API documentation tool for easy testing and exploration.
- **SMTP/Email Service:** For sending emails, including registration and order confirmations.

## 🌐 Live Demo
Check out the published API documentation here: 
[Swagger UI](https://laza.runasp.net/swagger/index.html)
