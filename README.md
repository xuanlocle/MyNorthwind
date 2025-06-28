# MyNorthwind

A modern .NET 9 Web API application that provides a RESTful interface for managing Northwind database entities with Firebase Cloud Messaging (FCM) push notification capabilities.

## 🚀 Features

- **RESTful API**: Full CRUD operations for Customers and Orders
- **Pagination Support**: Efficient data retrieval with pagination for large datasets
- **Firebase Integration**: Push notification service using Firebase Cloud Messaging
- **Device Token Management**: Register and manage device tokens for push notifications
- **Entity Framework Core**: Modern ORM with SQL Server support
- **Swagger Documentation**: Interactive API documentation
- **JSON Serialization**: Optimized with circular reference handling

## 🏗️ Architecture

- **Framework**: .NET 9
- **Database**: SQL Server with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Push Notifications**: Firebase Cloud Messaging
- **Architecture Pattern**: Controller-based REST API

## 📋 Prerequisites

- .NET 9 SDK
- SQL Server (local or cloud)
- Firebase project with service account credentials

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MyNorthwind
```

### 2. Environment Configuration

Create environment variables or use a configuration file:

```bash
# Database connection string
CONNECTION_SQL="Server=your-server;Database=Northwind;Trusted_Connection=true;TrustServerCertificate=true;"

# Firebase service account path
SERVICE_ACCOUNT_PATH="/path/to/your/firebase-service-account.json"
```

### 3. Database Setup

Ensure your SQL Server instance is running and the Northwind database is available. The application expects the following tables:
- `Customers`
- `Orders`
- `OrderDetails`
- `DeviceTokens`

### 4. Firebase Setup

1. Create a Firebase project at [Firebase Console](https://console.firebase.google.com/)
2. Generate a service account key:
   - Go to Project Settings > Service Accounts
   - Click "Generate new private key"
   - Save the JSON file securely
   - Set the `SERVICE_ACCOUNT_PATH` environment variable to point to this file

### 5. Run the Application

```bash
cd MyNorthwind
dotnet restore
dotnet run
```

The API will be available at:
- **API**: https://localhost:7000
- **Swagger UI**: https://localhost:7000/swagger

## 📚 API Documentation

### Customers API

#### Get All Customers (Paginated)
```http
GET /api/customers?pageNumber=1&pageSize=10
```

#### Get Customer by ID
```http
GET /api/customers/{id}
```

#### Create Customer
```http
POST /api/customers
Content-Type: application/json

{
  "customerId": "CUST01",
  "companyName": "Example Corp",
  "contactName": "John Doe",
  "contactTitle": "Manager",
  "address": "123 Main St",
  "city": "New York",
  "region": "NY",
  "postalCode": "10001",
  "country": "USA",
  "phone": "(555) 123-4567",
  "fax": "(555) 123-4568"
}
```

#### Update Customer
```http
PUT /api/customers/{id}
Content-Type: application/json

{
  "customerId": "CUST01",
  "companyName": "Updated Corp",
  // ... other fields
}
```

#### Delete Customer
```http
DELETE /api/customers/{id}
```

### Orders API

#### Get All Orders (Paginated)
```http
GET /api/orders?pageNumber=1&pageSize=10
```

#### Get Order by ID
```http
GET /api/orders/{id}
```

#### Create Order
```http
POST /api/orders
Content-Type: application/json

{
  "customerId": "CUST01",
  "orderDate": "2024-01-15T10:00:00Z",
  "requiredDate": "2024-01-22T10:00:00Z",
  "shippedDate": null,
  "shipVia": 1,
  "freight": 25.50,
  "shipName": "John Doe",
  "shipAddress": "123 Main St",
  "shipCity": "New York",
  "shipRegion": "NY",
  "shipPostalCode": "10001",
  "shipCountry": "USA"
}
```

**Note**: Creating an order automatically sends push notifications to all registered devices.

#### Update Order
```http
PUT /api/orders/{id}
Content-Type: application/json

{
  "orderId": 1,
  "customerId": "CUST01",
  // ... other fields
}
```

#### Delete Order
```http
DELETE /api/orders/{id}
```

### Device Management API

#### Register Device Token
```http
POST /api/device/register
Content-Type: application/json

{
  "token": "your-fcm-device-token-here"
}
```

#### Check Device Registration
```http
GET /api/device/check/{token}
```

## 🔔 Push Notifications

The application automatically sends push notifications when:
- A new order is created

Notifications include:
- **Title**: "New Order Created"
- **Body**: Order creation confirmation message
- **Data**: Customer ID for additional context

## 📊 Response Format

### Paginated Responses
Paginated endpoints include pagination metadata in the `X-Pagination` header:

```json
{
  "TotalCount": 100,
  "PageSize": 10,
  "CurrentPage": 1,
  "TotalPages": 10
}
```

### Error Responses
```json
{
  "message": "Error description"
}
```

## 🧪 Testing

Use the provided HTTP file for testing:

```bash
# Test customers endpoint
GET http://localhost:5100/api/customers/
Accept: application/json
```

Or use the Swagger UI at `/swagger` for interactive testing.

## 📦 Dependencies

- **Microsoft.AspNetCore.OpenApi**: OpenAPI support
- **Microsoft.EntityFrameworkCore**: ORM framework
- **Microsoft.EntityFrameworkCore.SqlServer**: SQL Server provider
- **FirebaseAdmin**: Firebase Cloud Messaging
- **Swashbuckle.AspNetCore**: Swagger documentation

## 🔧 Configuration

### Database Connection
Configure your SQL Server connection string in the `CONNECTION_SQL` environment variable.

### Firebase Configuration
Set the `SERVICE_ACCOUNT_PATH` environment variable to point to your Firebase service account JSON file.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Check the Swagger documentation at `/swagger`
- Review the API examples in `MyNorthwind.http` 