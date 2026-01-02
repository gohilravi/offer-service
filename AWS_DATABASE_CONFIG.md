# AWS Database Configuration Guide

This guide explains how to configure the Offer Service to use a custom AWS RDS PostgreSQL database.

## Configuration Methods

The application supports three methods for configuring AWS database connections (in order of precedence):

### 1. Environment Variable (Recommended for Production)
Set the complete connection string as an environment variable:

```bash
export AWS_CONNECTION_STRING="Host=your-rds-endpoint.amazonaws.com;Port=5432;Database=OfferServiceDb;Username=your-username;Password=your-password;SSL Mode=Require;Trust Server Certificate=true;"
```

### 2. Connection String in appsettings
Update the `AWSConnection` in your configuration file:

```json
{
  "ConnectionStrings": {
    "AWSConnection": "Host=your-rds-endpoint.amazonaws.com;Port=5432;Database=OfferServiceDb;Username=your-username;Password=your-password;SSL Mode=Require;Trust Server Certificate=true;"
  },
  "AWS": {
    "UseAWSConnection": true
  }
}
```

### 3. Individual AWS RDS Configuration
Configure individual connection parameters:

```json
{
  "AWS": {
    "UseAWSConnection": true,
    "Region": "us-east-1",
    "RDS": {
      "Host": "your-rds-endpoint.amazonaws.com",
      "Port": "5432",
      "Database": "OfferServiceDb",
      "Username": "your-username",
      "Password": "your-password",
      "SSLMode": "Require"
    }
  }
}
```

## Environment-Specific Configuration

### Development (Local PostgreSQL)
```json
{
  "AWS": {
    "UseAWSConnection": false
  }
}
```

### Production (AWS RDS)
```json
{
  "AWS": {
    "UseAWSConnection": true
  }
}
```

## Security Best Practices

1. **Never commit sensitive credentials to source control**
2. **Use environment variables for production credentials**
3. **Consider using AWS Secrets Manager for credential management**
4. **Enable SSL/TLS for database connections**
5. **Use IAM database authentication when possible**

## AWS RDS Setup

1. Create an RDS PostgreSQL instance in AWS
2. Configure security groups to allow connections from your application
3. Note the endpoint, port, database name, username, and password
4. Enable SSL if required by your security policy

## Docker Configuration

For Docker deployments, pass the connection string as an environment variable:

```bash
docker run -e AWS_CONNECTION_STRING="Host=..." your-image
```

## Kubernetes Configuration

Use Kubernetes secrets for credential management:

```yaml
apiVersion: v1
kind: Secret
metadata:
  name: db-secret
type: Opaque
stringData:
  AWS_CONNECTION_STRING: "Host=your-rds-endpoint.amazonaws.com;Port=5432;Database=OfferServiceDb;Username=your-username;Password=your-password;SSL Mode=Require;Trust Server Certificate=true;"
```

## Testing Connection

The application will validate the connection on startup and throw an exception if the configuration is invalid.

## Troubleshooting

1. **Connection timeout**: Check security group settings and network connectivity
2. **SSL errors**: Verify SSL mode configuration and certificate settings
3. **Authentication errors**: Verify username/password and IAM permissions
4. **Configuration not found**: Ensure `UseAWSConnection` is set to `true`