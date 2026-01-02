# Offer Service Microservice

A comprehensive .NET 8 microservice for managing vehicle offers with clean architecture, PostgreSQL, RabbitMQ messaging, and comprehensive testing.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
├── src/
│   ├── OfferService.Domain/          # Domain entities, interfaces, and business rules
│   ├── OfferService.Application/     # Application services, DTOs, and use cases
│   ├── OfferService.Infrastructure/  # Data access, external services, messaging
│   └── OfferService.Api/            # Web API controllers and configuration
├── tests/
│   └── OfferService.UnitTests/      # Comprehensive unit tests
```

## 🛠️ Technology Stack

- **.NET 8** (LTS) - Latest long-term support framework
- **PostgreSQL** - Robust relational database for persistence
- **Entity Framework Core** - ORM for data access
- **RabbitMQ + MassTransit** - Message broker for event-driven architecture
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation and testing
- **xUnit + Moq + FluentAssertions** - Comprehensive testing framework

## 📋 Features

### Domain Models

#### **Sellers**
- Unique seller identification with email validation
- Secure password hashing with BCrypt
- Timestamped records for auditing

#### **Offers**
- Complete vehicle information (make, model, year, trim, etc.)
- Detailed condition assessment fields
- Location and ownership data
- Status-based workflow (offered → assigned → canceled)

### API Endpoints

#### **Seller Management**
- `GET /api/sellers` - List all sellers with pagination

#### **Offer Management**
- `POST /api/offers` - Create new offer
- `GET /api/offers/{id}` - Get offer by ID
- `GET /api/offers` - List offers with filtering, sorting, and pagination
- `PUT /api/offers/{id}` - Update offer (only when status = "offered")
- `POST /api/offers/{id}/assign` - Assign offer (creates purchase + transport)
- `POST /api/offers/{id}/cancel` - Cancel offer

### Business Rules

✅ **Validation Rules:**
- All required fields must be provided
- Seller must exist before creating offers
- Vehicle descriptive attributes must be strings
- Proper HTTP status codes (201, 200, 400, 404, 409, 500)

✅ **State Management:**
- `offered` → `assigned` ✓
- `offered` → `canceled` ✓
- `assigned` → `canceled` ✓
- `canceled` → any status ❌
- `assigned` → `offered` ❌

✅ **Update Restrictions:**
- Only offers with status "offered" can be updated
- Only vehicle and condition fields are updateable

### Event-Driven Architecture

The service publishes events to RabbitMQ for all major operations:

- **OfferCreated** - When a new offer is created
- **OfferAssigned** - When offer is assigned (includes PurchaseId + TransportId)
- **OfferUpdated** - When offer details are modified
- **OfferCanceled** - When offer is canceled

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 13+](https://www.postgresql.org/download/)
- [RabbitMQ](https://www.rabbitmq.com/download.html)

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd offer-service
   ```

2. **Configure PostgreSQL**
   - Create database: `OfferServiceDb`
   - Update connection string in `src/OfferService.Api/appsettings.json`
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=OfferServiceDb;Username=your_user;Password=your_password"
     }
   }
   ```

3. **Configure RabbitMQ**
   - Ensure RabbitMQ is running on `localhost:5672`
   - Update connection string if needed in `appsettings.json`

4. **Build and run**
   ```bash
   dotnet build
   dotnet run --project src/OfferService.Api
   ```

5. **Database initialization**
   - The application will automatically create the database schema
   - Seeds 10 sample sellers with static credentials

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

**Test Coverage:** 41 comprehensive unit tests covering:
- ✅ All controller endpoints with various scenarios
- ✅ Service layer business logic
- ✅ Domain model validation
- ✅ Error handling and edge cases
- ✅ Event publishing verification

## 📖 API Documentation

Once running, access the interactive Swagger documentation at:
- **Development:** `http://localhost:5000`

The API documentation includes:
- Complete endpoint descriptions
- Request/response examples
- Data model schemas
- Error response formats

## 🧪 Sample Usage

### Creating a New Offer

```bash
curl -X POST "http://localhost:5000/api/offers" \\
  -H "Content-Type: application/json" \\
  -d '{
    "sellerId": "11111111-1111-1111-1111-111111111111",
    "sellerNetworkId": "NETWORK001",
    "sellerName": "John Doe",
    "vehicleYear": "2020",
    "vehicleMake": "Toyota",
    "vehicleModel": "Camry",
    "vehicleTrim": "LE",
    "vehicleZipCode": "12345",
    "mileage": 45000
  }'
```

### Assigning an Offer

```bash
curl -X POST "http://localhost:5000/api/offers/{offer-id}/assign"
```

### Filtering Offers

```bash
# Get all assigned offers, sorted by creation date
curl "http://localhost:5000/api/offers?status=assigned&sortBy=createdAt&sortDescending=true"
```

## 🏛️ Database Schema

### Sellers Table
| Column | Type | Description |
|--------|------|-------------|
| SellerId | UUID (PK) | Unique seller identifier |
| Name | VARCHAR(100) | Seller name |
| Email | VARCHAR(255) | Unique email address |
| PasswordHash | VARCHAR(255) | BCrypt hashed password |
| CreatedAt | TIMESTAMP | Record creation time |
| LastModifiedAt | TIMESTAMP | Last update time |

### Offers Table
| Column | Type | Description |
|--------|------|-------------|
| OfferId | UUID (PK) | Unique offer identifier |
| SellerId | UUID (FK) | Reference to seller |
| Status | VARCHAR(50) | offered/assigned/canceled |
| VehicleYear/Make/Model | VARCHAR | Vehicle identification |
| Mileage | INT | Vehicle mileage |
| ... | ... | Additional vehicle/condition fields |
| PurchaseId | UUID | External purchase system ID |
| TransportId | UUID | External transport system ID |
| CreatedAt/LastModifiedAt | TIMESTAMP | Audit timestamps |

## 🔧 Development

### Project Structure

- **Domain Layer** - Core business entities and rules
- **Application Layer** - Use cases, services, DTOs, and interfaces
- **Infrastructure Layer** - Data persistence, external APIs, messaging
- **API Layer** - HTTP endpoints, middleware, configuration

### Adding New Features

1. Define domain entities in `OfferService.Domain`
2. Create DTOs and services in `OfferService.Application`
3. Implement repositories in `OfferService.Infrastructure`
4. Add controllers in `OfferService.Api`
5. Write comprehensive unit tests

### Code Quality

- **Clean Architecture** - Clear separation of concerns
- **SOLID Principles** - Maintainable and extensible code
- **Repository Pattern** - Abstracted data access
- **Event-Driven Design** - Loose coupling via messaging
- **Comprehensive Testing** - High test coverage with realistic scenarios

## 📝 Logging & Monitoring

The application includes:
- **Structured logging** with timestamps and context
- **Request/response logging** middleware
- **Global exception handling** with proper HTTP status codes
- **Event publishing logs** for debugging message flows

## 🔒 Security Considerations

- Password hashing with BCrypt
- Input validation with data annotations
- SQL injection prevention via Entity Framework
- Global exception handling (no sensitive data leakage)

## 🚢 Production Deployment

For production deployment:

1. **Database migrations** - Use `dotnet ef migrations add` and `dotnet ef database update`
2. **Environment configuration** - Use Azure Key Vault or similar for secrets
3. **Health checks** - Add `/health` endpoints
4. **Monitoring** - Integrate with Application Insights or similar
5. **Load balancing** - Multiple API instances behind load balancer
6. **Message broker** - Clustered RabbitMQ or Azure Service Bus

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Write tests for new functionality
4. Ensure all tests pass
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.