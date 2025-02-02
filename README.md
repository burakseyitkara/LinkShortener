# LinkShortener

LinkShortener is a modern web application developed for shortening and managing URLs. Built with .NET 8 and following Clean Architecture principles, it provides a robust and scalable solution for URL management.

## 🚀 Features

- URL shortening and management
- User account management
- Link click statistics
- Link expiration settings
- Geographic location-based click analytics
- Device and browser-based analytics

## 🛠️ Technologies

- **.NET 8**
- **Entity Framework Core**
- **SQL Server**
- **Redis** (for caching)
- **BCrypt.NET** (for encryption)
- **AutoMapper**
- **Swagger/OpenAPI**

## 📦 Project Structure

The project follows Clean Architecture principles with a layered structure:

```
LinkShortener/
├── src/
│   ├── LinkShortener.API/           # API layer
│   ├── LinkShortener.Application/   # Application layer
│   ├── LinkShortener.Domain/        # Domain layer
│   └── LinkShortener.Infrastructure/# Infrastructure layer
└── tests/                           # Test projects
```

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full version)
- Redis Server (optional)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/LinkShortener.git
cd LinkShortener
```

2. Create the database:
```bash
cd src/LinkShortener.API
dotnet ef database update
```

3. Run the application:
```bash
dotnet run
```

The application will be available at `http://localhost:5149` by default.

## 🔑 API Endpoints

### Authentication

- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login

### Link Operations

- `POST /api/links` - Create new link
- `GET /api/links` - List user's links
- `GET /api/links/{id}` - View link details
- `PUT /api/links/{id}` - Update link
- `DELETE /api/links/{id}` - Delete link

### Statistics

- `GET /api/links/{id}/stats` - View link statistics

## 📊 Database Schema

The project includes the following main tables:

- `Users` - User information
- `Links` - Shortened links
- `LinkClicks` - Link click records

## 🔒 Security

- JWT-based authentication
- Password hashing (BCrypt)
- Input validation
- XSS and CSRF protection

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'feat: Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

Project Owner - [@burakseyitkara](https://github.com/burakseyitkara)

Project Link: [https://github.com/burakseyitkara/LinkShortener](https://github.com/burakseyitkara/LinkShortener) 