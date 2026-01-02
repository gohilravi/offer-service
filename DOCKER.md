# Offer Service - Docker Deployment Guide

## Overview
The Offer Service is now fully containerized and ready for deployment using Docker and Docker Compose.

## Prerequisites
- Docker Desktop installed
- Docker Compose v3.8+
- 4GB+ available RAM
- Port 8080, 5432, 5672, 15672 available

## Quick Start

### 1. Build and Run with Docker Compose
```bash
# Navigate to the project root
cd "d:\AI Code Challenge\offer-service"

# Build and start all services
docker-compose up --build

# Or run in detached mode
docker-compose up --build -d
```

### 2. Verify Services
- **API Service**: http://localhost:8080
- **Swagger UI**: http://localhost:8080/swagger
- **Health Check**: http://localhost:8080/health
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **PostgreSQL**: localhost:5432 (postgres/postgres123)

## Service Architecture

The Docker Compose setup includes:

### 1. PostgreSQL Database (`postgres`)
- **Image**: postgres:15-alpine
- **Port**: 5432
- **Database**: OfferServiceDb
- **Credentials**: postgres/postgres123
- **Initialization**: Runs `database/init.sql` on first start
- **Health Check**: pg_isready monitoring

### 2. RabbitMQ Message Broker (`rabbitmq`)
- **Image**: rabbitmq:3.12-management-alpine
- **AMQP Port**: 5672
- **Management UI**: 15672
- **Credentials**: guest/guest
- **Health Check**: rabbitmq-diagnostics ping

### 3. Offer Service API (`offer-service`)
- **Build**: Multi-stage Dockerfile
- **Port**: 8080
- **Environment**: Production
- **Health Check**: /health endpoint
- **Dependencies**: Waits for PostgreSQL and RabbitMQ to be healthy

## Database Initialization

On first startup, PostgreSQL will automatically:
1. Create the OfferServiceDb database
2. Execute the init.sql script to create tables
3. Seed 10 sample sellers with hashed passwords

## Environment Variables

### Offer Service API
```yaml
ASPNETCORE_ENVIRONMENT: Production
ConnectionStrings__DefaultConnection: "Host=postgres;Database=OfferServiceDb;Username=postgres;Password=postgres123"
ConnectionStrings__RabbitMQ: "rabbitmq://guest:guest@rabbitmq:5672/"
ASPNETCORE_URLS: "http://+:8080"
```

## Management Commands

### Start Services
```bash
docker-compose up -d
```

### Stop Services
```bash
docker-compose down
```

### Rebuild and Start
```bash
docker-compose up --build
```

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f offer-service
docker-compose logs -f postgres
docker-compose logs -f rabbitmq
```

### Scale Services
```bash
# Scale API service to 3 instances
docker-compose up -d --scale offer-service=3
```

## Health Monitoring

All services include health checks:
- **PostgreSQL**: pg_isready every 10s
- **RabbitMQ**: diagnostics ping every 30s  
- **API Service**: /health endpoint every 30s

Check health status:
```bash
docker-compose ps
```

## Data Persistence

Persistent volumes are configured for:
- **postgres_data**: PostgreSQL database files
- **rabbitmq_data**: RabbitMQ message storage

Data survives container restarts and rebuilds.

## Network Configuration

All services run on a custom network (`offer-service-network`) enabling:
- Service-to-service communication by name
- Isolated networking from other Docker projects
- Automatic DNS resolution

## Production Considerations

### Security
- Change default passwords before production deployment
- Use environment files (.env) for sensitive data
- Enable PostgreSQL SSL connections
- Configure RabbitMQ authentication

### Resource Limits
Add resource constraints for production:
```yaml
deploy:
  resources:
    limits:
      cpus: '0.5'
      memory: 512M
```

### Load Balancing
Use a reverse proxy (nginx/traefik) for:
- SSL termination
- Load balancing across multiple API instances
- Static asset serving

### Monitoring
Consider adding:
- Prometheus metrics collection
- Grafana dashboards
- Centralized logging (ELK stack)
- APM tools (Application Insights, New Relic)

## Troubleshooting

### Service Won't Start
1. Check port availability: `netstat -an | findstr "8080"`
2. View logs: `docker-compose logs offer-service`
3. Verify dependencies: `docker-compose ps`

### Database Connection Issues
1. Check PostgreSQL health: `docker-compose logs postgres`
2. Verify connection string in docker-compose.yml
3. Test direct connection: `docker exec -it offer-service-postgres psql -U postgres -d OfferServiceDb`

### RabbitMQ Issues
1. Check management UI: http://localhost:15672
2. View RabbitMQ logs: `docker-compose logs rabbitmq`
3. Test connection: `docker exec -it offer-service-rabbitmq rabbitmqctl status`

### API Service Issues
1. Check health endpoint: http://localhost:8080/health
2. View application logs: `docker-compose logs -f offer-service`
3. Verify Swagger UI: http://localhost:8080/swagger

## Development vs Production

### Development Setup
```bash
# Use development environment
export ASPNETCORE_ENVIRONMENT=Development
docker-compose up --build
```

### Production Setup
```bash
# Use production environment (default)
docker-compose up --build -d
```

The containerized offer-service is now ready for any deployment environment!