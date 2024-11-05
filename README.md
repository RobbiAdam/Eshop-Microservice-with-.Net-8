# Eshop-Microservices with .Net 8

This is a personal project to practice creating a microservice for E-commerce

there is a couple microservices that are used in this project, those are :
- Catalog: To handle stored product
- Basket: To add product to basket and checkout
- Discount: To handle discount on product
- Ordering: To handle order history after purchasing or checkout from basket
- YarpApiGateway

More Detailed on each service

## Catalog Service
- Using .Net 8 Asp.net core minimal api
- Implemented with Vertical Slice Architecture with features folder
- CQRS implementation with MediatR and CQRS pipeline behavior with MediatR and Fluent Validation
- Use Marten .NET Transactional Document DB for PostgreSQL
- Logging, Global exception handling and health checks
- Implement Dockerfile and docker-compose file for running service and PostgreSQL database in docker environtment

## Basket Service
- All above Catalog service
- Using Redis as distributed cache
- Consume Discount.grpc service for communication to calculate product final price
- Publish BasketCheckout Queue using MassTransit and RabbitMQ
- Contenarize service with Redis and PostgreSQL database

## Discount Service
- ASP.NET gRPC application
- gRPC communication with Basket service
- Use SQLite database
- N-Layer Architecture implementation
- Using Entity Framework Core, Migrations for data access
- Containerize Discount service with SQLite database

## Ordering Service
- ASP.Net Core Web Api
- Implementing Domain Driven Desiqn, CQRS and Clean Architecture while applying best practice
- Raise & handle Domain events & integrations events
- Using EFCore, Migrations
- Using SQL server as database
- Consuming RabbitMQ BasketCheckout event queue with using MassTransit-RabbitMQ configuration
- Containerize Ordering service with SQL server Database

## YarpApiGateway
- Develop Api Gateways with Yarp Reverse Proxy applying Gateway routing pattern
- Yarp Reverse Proxy Configuration; Route, Cluster, Path, Transform, Destinations
- Rate Limiting with FixedWindowLimiter
- Contenarize YarpApiGateway