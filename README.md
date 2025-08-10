### Note
This project is about a backend challenge, available in this repository: https://github.com/zanfranceschi/rinha-de-backend-2025. 

# About

This repository implements a Payment Processor with limited resources, handling a large number of requests even with limited infrastructure, focusing on creating a resilient + performatic backend.
<br>
This project uses the .NET AOT (Ahead Of Time) compilation, which is a technique used to compile code before runtime, resulting in 30% of performance improvement of the application runtime.

# Api Architecture
This API uses the Vertical Slice architecture to improve focus, scalability, and flexibility of the codebase.

## Structure

### Project Root
```
PaymentProcessor-Challenge/
├── PaymentProcessor_VerticalSlice.sln
├── docker-compose.yaml
├── README.md
├── LICENSE
├── .dockerignore
├── .gitignore
└── assets/
    └── payment-processor-design.png
```

### PaymentProcessor.Api/
```
PaymentProcessor.Api/
├── Program.cs                          # Main entry point
├── Dockerfile                          # Docker configuration
├── docker-compose.yml                  # Local development setup
├── appsettings.json                    # Configuration
├── appsettings.Development.json        # Development settings
├── PaymentProcessor.Api.csproj         # Project file (.NET 9.0 + AOT)
├── Properties/
│   └── launchSettings.json            # Launch profiles
├── Domain/
│   ├── DTOs/
│   │   ├── GET/
│   │   │   ├── PaymentPurgeResponse.cs
│   │   │   └── PaymentSummaryResponse.cs
│   │   ├── POST/
│   │   │   ├── PaymentProcessorRequest.cs
│   │   │   └── PaymentRequest.cs
│   │   ├── PaymentProcessorDTO.cs
│   │   └── PaymentSummaryDTO.cs
│   ├── Entities/
│   │   └── Payments.cs
│   └── Interfaces/
├── Features/
│   └── PaymentProcessor/
├── Infrastructure/
│   ├── Config/
│   │   └── DbConnectionFactory.cs
│   ├── Contexts/
│   ├── Database/
│   │   ├── DatabaseHealthCheck.cs
│   │   └── Scripts/
│   │       └── 080520250142_Payments.sql
│   ├── Enum/
│   │   └── PaymentGateway.cs
│   ├── MessageBroker/
│   └── Redis/
│       ├── IRedisCacheService.cs
│       └── RedisCacheService.cs
```

### PaymentProcessor.Tests/
```
PaymentProcessor.Tests/
├── PaymentProcessor.Tests.csproj
└── UnitTest1.cs
```

### .github/
```
.github/
└── workflows/
    └── build.yml
```

# System Design
A schema about how the system works. The resources limitations are in the docker-compose.yaml file.
![Payment Processor System design](./assets/payment-processor-design.png)


# Technical Specifications
- .NET Core
- ADO.NET + AOT
- PostgreSQL
- Redis
- RabbitMQ
- Nginx
- Docker
- Docker Compose
