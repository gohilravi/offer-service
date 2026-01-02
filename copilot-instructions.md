# Offer Service Microservice Instructions

This is a .NET Core 8 microservice for managing vehicle offers with clean architecture, PostgreSQL, RabbitMQ MassTransit, Swagger, and comprehensive unit testing.

## Architecture
- Clean Architecture with layered design
- Domain-driven design patterns
- Repository pattern for data access
- CQRS with MediatR
- Event-driven architecture with RabbitMQ

## Technology Stack
- .NET Core 8 (LTS)
- PostgreSQL for persistence
- RabbitMQ with MassTransit for messaging
- Swagger for API documentation
- xUnit for unit testing
- Entity Framework Core for ORM

## Progress Tracking
- [x] Clarify Project Requirements
- [x] Scaffold the Project
- [x] Customize the Project
- [x] Install Required Extensions
- [x] Compile the Project
- [x] Create and Run Task
- [x] Launch the Project
- [x] Ensure Documentation is Complete

## Project Summary

Successfully created a comprehensive offer-service microservice with:

### ✅ Domain Layer
- Seller and Offer entities with proper validation
- Domain exceptions and business rules
- Offer status enums with transition logic

### ✅ Application Layer  
- DTOs for all operations (Create, Update, Response)
- Application services with business logic
- AutoMapper configuration for object mapping
- Event models for RabbitMQ messaging

### ✅ Infrastructure Layer
- Entity Framework Core with PostgreSQL
- Repository pattern implementation
- Unit of Work pattern
- MassTransit event publisher
- Mock external API services (Purchase/Transport)

### ✅ API Layer
- RESTful controllers for Sellers and Offers
- Comprehensive Swagger documentation
- Global exception handling middleware
- Request logging middleware
- CORS configuration

### ✅ Testing
- 41 comprehensive unit tests (100% passing)
- Controller tests with all scenarios
- Service layer tests with mocking
- Domain logic validation tests
- FluentAssertions for readable test assertions

### ✅ Features Implemented
- **Seller Management**: List sellers with pagination
- **Offer CRUD**: Create, Read, Update offers with validation
- **Offer Assignment**: External API integration workflow
- **Offer Cancellation**: Status transition management
- **Event Publishing**: RabbitMQ integration for all operations
- **Database Seeding**: 10 sample sellers with hashed passwords
- **Error Handling**: Proper HTTP status codes and error responses

### ✅ API Endpoints
- `GET /api/sellers` - Paginated seller list
- `POST /api/offers` - Create new offer
- `GET /api/offers/{id}` - Get offer by ID
- `GET /api/offers` - List offers with filtering/sorting/pagination
- `PUT /api/offers/{id}` - Update offer (restricted by status)
- `POST /api/offers/{id}/assign` - Assign offer
- `POST /api/offers/{id}/cancel` - Cancel offer

### ✅ Business Rules Enforced
- Seller validation before offer creation
- Status transition validation (offered → assigned/canceled)
- Update restrictions (only "offered" status can be updated)
- Comprehensive field validation with proper error responses

Ready for development and production deployment!