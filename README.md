# AtendeLogo

AtendeLogo is a personal project that I am developing during my free time and weekends.
Its goal is to streamline the initial customer service process via WhatsApp for civil registry offices in Brazil.

This project primarily focuses on facilitating pre-service interactions, with the potential for future expansion to include multi-tenant capabilities.
 
## Technologies and Architecture

It will follow the Clean Architecture pattern using C# .NET 9.0 and microservices, ensuring a robust and scalable solution.

I am creating this project to gain hands-on experience and explore new technologies while solving a real-world problem.

### **Backend Architecture Overview:**
- **Identity API** -> PostgreSQL Database
- **Activities API** -> MongoDB
- **Messages Service** -> MongoDB
- **Caching Service** -> Redis
- **WebSockets Communication** -> SignalR for real-time updates

### **Frontend Overview:**
- **Admin Panel (TBD):** Blazor (tentative)
- **WhatsApp Agent:** React UI

## Project Structure

The project is organized into multiple layers and services following Clean Architecture principles:

### 1. Shared Layers

### AtendeLogo.Common
This project serves as a DRY (Don't Repeat Yourself) library containing common utilities:

- **Extensions** Utility extension methods
- **Helpers** Complex utilities that depend on other dependencies (e.g., `PasswordHelper` using SHA256)
- **Utils** Pure functions without dependencies
#### AtendeLogo.SharedKernel

- **Purpose:**  
  A dedicated layer for **cross-cutting concerns** shared across the entire solution. It is independent of any specific layer and ensures compliance with the **Dependency Rule**.

- **Key Features:**
  - **Validation Constants:** Centralized constants, such as maximum string lengths and business rule constraints.
  - **Enums:** Shared enumerations used across multiple layers.
  - **Interfaces:** Shared contracts that multiple layers may implement.

  - **Structure:**
  
  - This project contains only **structs**, records, enums, and interfaces.
  - No methods or utility classes are included in this layer.
  - This layer should not reference any other project in the solution.

- **Why SharedKernel?**
  - Keeps shared logic reusable and decoupled.
  - Prevents duplication of constants and enums across layers.
  - Maintains clean separation of concerns.

### **2. Core Layer**

#### AtendeLogo.Domain
- **Constants** Domain-specific constants
- **Entities** Core business entities
- **Enums** Enumerations related to the domain
- **Extensions** Domain extension methods
- **ValueObjects** Immutable domain value objects

#### AtendeLogo.Application
- **Purpose:**  
  This layer contains application-specific, contracts and mediator for commands, queries, and domain events. It serves as a bridge between the Domain and Infrastructure layers.
- **Contracts**
  - Notifications
  - Persistence
  - Security
  - Services
- **Exceptions** Custom exceptions for error handling
- **Validations** Input and business rule validations

#### AtendeLogo.UseCases
- **Purpose:**  
  This layer contains application-specific use cases, including backend implementations for commands, queries, and domain events.

- - **Identities** Handles identity-related operations
- **Activities** Business logic for activities
- **Messages** Message handling and processing

#### AtendeLogo.UseCases.Shared

- **Purpose:**  
  This layer contains shared use cases, including DTOs for requests and responses, as well as validators. These components are designed for use in both the frontend and backend.

- **Key Features:**
  - **Decoupling:** Enables consistent data handling and validation across Blazor and React UIs without exposing the Domain layer.
  - **DTOs:** Data Transfer Objects used for API communication.
  - **Validation Logic:** Centralized validation rules for input data using FluentValidation or similar libraries.
  - **Interfaces** Interfaces ensuring integrity, such as Automapper
  - **Contracts** Services that will be used by the UI to communicate with the backend
 
- **Structure:**
  - This project contains **DTOs**, **validation classes**, **interfaces**, and **contracts**.
  - This project should reference AtendeLogo.SharedKernel and AtendeLogo.Common, but no other project in the solution.
  

### **3. Infrastructure Layer**

#### AtendeLogo.Infrastructure
- **CacheServices** Services responsible for caching operations
- **EmailServices** Services for sending emails
- **NotificationServices** Services for sending notifications

#### Application.Persistence
- **Database** PostgreSQL database for entities management
- **Repositories** Data access layer for entities
- **Migrations** Database migrations
- **Configurations** Database configurations
- **DbContext** Entity Framework DbContext

#### AtendeLogo.Persistence.Activities
- **Database** MongoDB database for activity storage
- **/documents** Activity-related document management

#### Application.Persistence.Messages
- **DataBase** MongoDB database for message storage
- **/documents** Message-related document storage

### **4. Presentation Layer**
- In progress

## Future Enhancements
Potential improvements include:
- Support for multi-tenancy
- Integration with third-party services
- Enhanced analytics and reporting

## Contribution
This project is primarily for portfolio purposes, but contributions and suggestions are welcome.