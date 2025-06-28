# Docker Hosting Guide

This guide covers hosting your .NET 9 MyNorthwind API using Docker with different deployment scenarios.

## 🐳 Docker Setup Overview

### **Files Created:**
- `Dockerfile` - Production Docker image
- `Dockerfile.dev` - Development Docker image with hot reload
- `docker-compose.yml` - Production multi-container setup
- `docker-compose.dev.yml` - Development environment
- `database/init/01-init-database.sql` - Database initialization

## 🚀 Quick Start

### **Option 1: Production Deployment**
```bash
# Build and run the complete stack
docker-compose up -d

# Access your API
curl http://localhost:8080/api/customers
```

### **Option 2: Development with Hot Reload**
```bash
# Start development environment
docker-compose -f docker-compose.dev.yml up -d

# Access your API (with hot reload)
curl http://localhost:8080/api/customers
```

## 📋 Prerequisites

- Docker Desktop installed
- Docker Compose installed
- At least 4GB RAM available for containers

## 🔧 Configuration

### **Environment Variables**

Create a `.env` file in your project root:
```bash
# Database Configuration
DB_PASSWORD=YourStrong@Passw0rd
DB_NAME=Northwind

# API Configuration
ASPNETCORE_ENVIRONMENT=Development
CONNECTION_SQL=Server=sqlserver;Database=Northwind;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;

# Firebase Configuration (if using)
SERVICE_ACCOUNT_PATH=/app/firebase/serviceAccountKey.json
```

### **Firebase Setup (Optional)**

If you're using Firebase push notifications:

1. Create a `firebase` directory in your project root
2. Place your Firebase service account JSON file there:
   ```bash
   mkdir firebase
   cp /path/to/your/serviceAccountKey.json firebase/
   ```

## 🏗️ Architecture

### **Production Stack (`docker-compose.yml`)**
```
┌─────────────────┐    ┌─────────────────┐
│   .NET 9 API    │    │  SQL Server     │
│   (Port 8080)   │◄──►│   (Port 1433)   │
└─────────────────┘    └─────────────────┘
```

### **Development Stack (`docker-compose.dev.yml`)**
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   .NET 9 API    │    │  SQL Server     │    │   Adminer       │
│ (Hot Reload)    │◄──►│   (Port 1433)   │    │  (Port 8081)    │
│ (Port 8080)     │    └─────────────────┘    └─────────────────┘
└─────────────────┘
```

## 🚀 Deployment Scenarios

### **1. Local Development**

```bash
# Start development environment
docker-compose -f docker-compose.dev.yml up -d

# View logs
docker-compose -f docker-compose.dev.yml logs -f api

# Stop services
docker-compose -f docker-compose.dev.yml down
```

**Features:**
- Hot reload on code changes
- Database management with Adminer
- Sample data pre-loaded
- Debug-friendly configuration

### **2. Production Deployment**

```bash
# Build and start production stack
docker-compose up -d --build

# Check service status
docker-compose ps

# View logs
docker-compose logs -f api

# Stop services
docker-compose down
```

**Features:**
- Optimized production images
- Persistent database storage
- Health checks
- Production-ready configuration

### **3. Database Management**

```bash
# Access database with Adminer (development)
# Open http://localhost:8081 in browser
# Server: sqlserver
# Username: sa
# Password: YourStrong@Passw0rd
# Database: Northwind

# Or connect directly with SQL Server tools
# Server: localhost,1433
# Username: sa
# Password: YourStrong@Passw0rd
```

### **4. With Additional Tools**

```bash
# Start with Redis cache
docker-compose -f docker-compose.dev.yml --profile cache up -d

# Start with database management tools
docker-compose -f docker-compose.dev.yml --profile tools up -d

# Start with both
docker-compose -f docker-compose.dev.yml --profile cache --profile tools up -d
```

## 🔍 Monitoring and Debugging

### **Container Status**
```bash
# Check running containers
docker ps

# Check container logs
docker logs mynorthwind-api
docker logs mynorthwind-db

# Execute commands in containers
docker exec -it mynorthwind-api bash
docker exec -it mynorthwind-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd
```

### **Database Operations**
```bash
# Connect to database
docker exec -it mynorthwind-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d Northwind

# Run SQL commands
docker exec -it mynorthwind-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d Northwind -Q "SELECT * FROM Customers"
```

### **API Testing**
```bash
# Test API endpoints
curl http://localhost:8080/api/customers
curl http://localhost:8080/api/orders
curl http://localhost:8080/swagger

# Test with pagination
curl "http://localhost:8080/api/customers?pageNumber=1&pageSize=5"
```

## 🔧 Customization

### **Modify Database Password**
```bash
# Edit docker-compose.yml or docker-compose.dev.yml
# Change SA_PASSWORD environment variable
```

### **Add Environment Variables**
```bash
# Add to environment section in docker-compose files
environment:
  - NEW_VARIABLE=value
```

### **Change Ports**
```bash
# Modify ports section in docker-compose files
ports:
  - "8080:8080"  # Change 8080 to your preferred port
```

### **Add Volume Mounts**
```bash
# Add to volumes section
volumes:
  - ./your-local-path:/app/container-path
```

## 🚨 Troubleshooting

### **Common Issues**

#### **1. Port Already in Use**
```bash
# Check what's using the port
lsof -i :8080
lsof -i :1433

# Stop conflicting services or change ports
```

#### **2. Database Connection Issues**
```bash
# Check if SQL Server is running
docker logs mynorthwind-db

# Check connection string
docker exec -it mynorthwind-api env | grep CONNECTION_SQL
```

#### **3. Permission Issues**
```bash
# Fix file permissions
chmod -R 755 ./database
chmod -R 755 ./firebase
```

#### **4. Memory Issues**
```bash
# Check container resource usage
docker stats

# Increase Docker Desktop memory limit
# Docker Desktop → Settings → Resources → Memory
```

### **Reset Everything**
```bash
# Stop and remove all containers
docker-compose down -v

# Remove all images
docker rmi $(docker images -q)

# Start fresh
docker-compose up -d --build
```

## 📊 Performance Optimization

### **Production Optimizations**
```bash
# Use production Dockerfile
docker build -f Dockerfile -t mynorthwind:prod .

# Run with resource limits
docker run -d \
  --name mynorthwind-api \
  --memory=512m \
  --cpus=1 \
  -p 8080:8080 \
  mynorthwind:prod
```

### **Development Optimizations**
```bash
# Use volume mounts for faster builds
volumes:
  - ./MyNorthwind:/app/src:ro
  - /app/src/bin
  - /app/src/obj
```

## 🔐 Security Considerations

### **Production Security**
1. **Change default passwords**
2. **Use secrets management**
3. **Enable SSL/TLS**
4. **Restrict network access**
5. **Regular security updates**

### **Environment Variables**
```bash
# Use Docker secrets in production
echo "YourStrong@Passw0rd" | docker secret create db_password -

# Reference in docker-compose.yml
secrets:
  - db_password
```

## 📈 Scaling

### **Horizontal Scaling**
```bash
# Scale API instances
docker-compose up -d --scale api=3

# Use load balancer
docker run -d \
  --name nginx \
  -p 80:80 \
  -v /path/to/nginx.conf:/etc/nginx/nginx.conf \
  nginx:alpine
```

### **Database Scaling**
```bash
# Use external database service
# Azure SQL Database, AWS RDS, Google Cloud SQL
```

## 🚀 Deployment to Cloud

### **Docker Hub**
```bash
# Build and push to Docker Hub
docker build -t yourusername/mynorthwind:latest .
docker push yourusername/mynorthwind:latest
```

### **Azure Container Instances**
```bash
# Deploy to Azure
az container create \
  --resource-group myResourceGroup \
  --name mynorthwind \
  --image yourusername/mynorthwind:latest \
  --ports 8080
```

### **AWS ECS**
```bash
# Create ECS task definition
aws ecs register-task-definition \
  --family mynorthwind \
  --container-definitions file://task-definition.json
```

## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [SQL Server on Docker](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet) 