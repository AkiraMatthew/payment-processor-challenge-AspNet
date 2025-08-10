### Note
This project is about a backend challenge, available in this repository: https://github.com/zanfranceschi/rinha-de-backend-2025. 

# About

This repository implements a Payment Processor with limited resources, handling a large number of requests even with limited infrastructure, focusing on creating a resilient + performatic backend.
<br>
This project uses the .NET AOT (Ahead Of Time) compilation, which is a technique used to compile code before runtime, resulting in 30% of performance improvement of the application runtime.

# Api Architecture
This API uses the Vertical Slice architecture to improve focus, scalability, and flexibility of the codebase.

## Structure

![Payment Processor System design](./assets/mermaid-paymentProcessor.svg)

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
