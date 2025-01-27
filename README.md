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

### **1. AtendeLogo.Common**
This project serves as a DRY (Don't Repeat Yourself) library containing common utilities:

- **Domain** – Primitive base classes for domain logic (e.g., `EntityBase`)
- **Enums** – Common enums shared across domains
- **Extensions** – Utility extension methods
- **Helpers** – Complex utilities that depend on other dependencies (e.g., `PasswordHelper` using SHA256)
- **Utils** – Pure functions without dependencies

### **2. Core Layer**

#### AtendeLogo.Domain
- **Constants** – Domain-specific constants
- **Entities** – Core business entities
- **Enums** – Enumerations related to the domain
- **Extensions** – Domain extension methods
- **Interfaces** – Interfaces ensuring integrity, such as Automapper
- **ValueObjects** – Immutable domain value objects

#### AtendeLogo.Application
- **Contracts**
  - Notifications
  - Persistence
  - Security
  - Services
- **Exceptions** – Custom exceptions for error handling
- **Validations** – Input and business rule validations

### **3. Use Cases Layer**

#### AtendeLogo.UseCases
- **Identities** – Handles identity-related operations
- **Activities** – Business logic for activities
- **Messages** – Message handling and processing

### **4. Persistence Layer**

#### Application.Persistence.Identities
- **Repositories** – Data access layer for identities
- **Migrations** – Database migrations
- **Configurations** – Database configurations
- **DbContext** – Entity Framework DbContext

#### AtendeLogo.Persistence.Activities
- **/documents** – Activity-related document management

#### Application.Persistence.Messages
- **/documents** – Message-related document storage

### **5. Infrastructure Layer**

#### AtendeLogo.Infrastructure
- **CacheServices** – Services responsible for caching operations

## Future Enhancements
Potential improvements include:
- Support for multi-tenancy
- Integration with third-party services
- Enhanced analytics and reporting

## Contribution
This project is primarily for portfolio purposes, but contributions and suggestions are welcome. Feel free to open issues or submit pull requests.
