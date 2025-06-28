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
- **Docker Support**: Complete containerization with development and production setups
- **Multiple Deployment Options**: Docker, Firebase Hosting, Google Cloud Run

## 🏗️ Architecture

- **Framework**: .NET 9
- **Database**: SQL Server with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Push Notifications**: Firebase Cloud Messaging
- **Architecture Pattern**: Controller-based REST API
- **Containerization**: Docker with multi-stage builds
- **Deployment**: Multiple cloud platform support

## 📋 Prerequisites

- .NET 9 SDK
- SQL Server (local or cloud)
- Firebase project with service account credentials
- Docker Desktop (for containerized deployment)

## 🛠️ Quick Start

### Option 1: Docker (Recommended)

#### Development with Hot Reload
```bash
# Clone the repository
git clone <repository-url>
cd MyNorthwind

# Start development environment
docker-compose -f docker-compose.dev.yml up -d

# Access your API
curl http://localhost:8080/api/customers
```

#### Production Deployment
```bash
# Start production stack
docker-compose up -d --build

# Access your API
curl http://localhost:8080/api/customers
```

### Option 2: Local Development

#### 1. Clone the Repository
```bash
git clone <repository-url>
cd MyNorthwind
```

#### 2. Environment Configuration
Create environment variables or use a configuration file:

```bash
# Database connection string
CONNECTION_SQL="Server=your-server;Database=Northwind;Trusted_Connection=true;TrustServerCertificate=true;"

# Firebase service account path
SERVICE_ACCOUNT_PATH="/path/to/your/firebase-service-account.json"
```

#### 3. Database Setup
Ensure your SQL Server instance is running and the Northwind database is available. The application expects the following tables:
- `Customers`
- `Orders`
- `OrderDetails`
- `DeviceTokens`

#### 4. Firebase Setup
1. Create a Firebase project at [Firebase Console](https://console.firebase.google.com/)
2. Generate a service account key:
   - Go to Project Settings > Service Accounts
   - Click "Generate new private key"
   - Save the JSON file securely
   - Set the `SERVICE_ACCOUNT_PATH` environment variable to point to this file

#### 5. Run the Application
```bash
cd MyNorthwind
dotnet restore
dotnet run
```

The API will be available at:
- **API**: https://localhost:7000
- **Swagger UI**: https://localhost:7000/swagger

## 🐳 Docker Hosting

### Development Environment
```bash
# Start with hot reload
docker-compose -f docker-compose.dev.yml up -d

# Access points:
# API: http://localhost:8080
# Swagger: http://localhost:8080/swagger
# Database Admin: http://localhost:8081 (Adminer)
```

### Production Environment
```bash
# Start production stack
docker-compose up -d --build

# Access points:
# API: http://localhost:8080
# Swagger: http://localhost:8080/swagger
```

### Database Management
```bash
# Access Adminer (development)
# Open http://localhost:8081
# Server: sqlserver
# Username: sa
# Password: YourStrong@Passw0rd
# Database: Northwind
```

### Docker Features
- **Hot Reload**: Code changes auto-reload in development
- **Database**: SQL Server 2022 with sample data
- **Adminer**: Web-based database management
- **Health Checks**: Container monitoring
- **Persistent Storage**: Database data survives restarts

For detailed Docker setup, see [DOCKER_HOSTING.md](DOCKER_HOSTING.md).

## ☁️ Cloud Deployment

### Google Cloud Run (Recommended for APIs)
```bash
# Deploy to Google Cloud Run
# See .github/workflows/deploy-to-cloud-run.yml
```

### Firebase Hosting (Static Documentation)
```bash
# Deploy static documentation
# See .github/workflows/deploy-to-firebase.yml
```

For detailed deployment guides, see [DEPLOYMENT.md](DEPLOYMENT.md) and [FIREBASE_SETUP.md](FIREBASE_SETUP.md).

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

### Docker Environment
```bash
# Test API endpoints
curl http://localhost:8080/api/customers
curl http://localhost:8080/api/orders
curl http://localhost:8080/swagger

# Test with pagination
curl "http://localhost:8080/api/customers?pageNumber=1&pageSize=5"
```

### Local Environment
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

### Docker Configuration
- **Development**: Uses `docker-compose.dev.yml` with hot reload
- **Production**: Uses `docker-compose.yml` with optimized images
- **Database**: SQL Server 2022 with persistent storage

## 🚀 Deployment Options

### 1. Docker (Local/On-Premises)
- **Best for**: Development, testing, on-premises deployment
- **Features**: Complete stack with database, hot reload, admin tools
- **Setup**: `docker-compose up -d`

### 2. Google Cloud Run (Cloud)
- **Best for**: Production APIs, auto-scaling, serverless
- **Features**: Pay-per-use, auto-scaling, managed infrastructure
- **Setup**: GitHub Actions deployment

### 3. Firebase Hosting (Static)
- **Best for**: Documentation, static sites
- **Features**: Fast CDN, global distribution
- **Setup**: GitHub Actions deployment

## 🔍 Monitoring and Debugging

### Docker Environment
```bash
# Check container status
docker ps

# View logs
docker logs mynorthwind-api
docker logs mynorthwind-db

# Execute commands in containers
docker exec -it mynorthwind-api bash
```

### Database Operations
```bash
# Connect to database (Docker)
docker exec -it mynorthwind-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d Northwind

# Run SQL commands
docker exec -it mynorthwind-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d Northwind -Q "SELECT * FROM Customers"
```

## 🚨 Troubleshooting

### Common Issues

#### Docker Issues
- **Port conflicts**: Change ports in docker-compose files
- **Memory issues**: Increase Docker Desktop memory limit
- **Permission issues**: Check file permissions for volumes

#### Database Issues
- **Connection failed**: Verify connection string and SQL Server status
- **Missing tables**: Run database initialization script

#### Firebase Issues
- **Authentication failed**: Check service account key path
- **Push notifications not working**: Verify FCM configuration

### Debug Commands
```bash
# Check container health
docker-compose ps

# View detailed logs
docker-compose logs -f api

# Reset everything
docker-compose down -v
docker-compose up -d --build
```

## 📈 Performance Optimization

### Docker Optimizations
- **Multi-stage builds**: Smaller production images
- **Volume mounts**: Faster development builds
- **Health checks**: Automatic container monitoring

### API Optimizations
- **Pagination**: Efficient data retrieval
- **Lazy loading**: Optimized Entity Framework queries
- **Caching**: Redis support in development

## 🔐 Security Best Practices

1. **Environment Variables**: Use secrets for sensitive data
2. **Database Security**: Change default passwords
3. **Firebase Security**: Secure service account keys
4. **Container Security**: Non-root users, minimal images
5. **Network Security**: Restrict container communication

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
- Review the deployment guides:
  - [DOCKER_HOSTING.md](DOCKER_HOSTING.md)
  - [DEPLOYMENT.md](DEPLOYMENT.md)
  - [FIREBASE_SETUP.md](FIREBASE_SETUP.md)

## 📚 Additional Resources

- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Firebase Documentation](https://firebase.google.com/docs)
- [Docker Documentation](https://docs.docker.com/)
- [Google Cloud Run](https://cloud.google.com/run/docs) 