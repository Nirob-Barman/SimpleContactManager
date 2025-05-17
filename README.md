# ✅ SimpleContactManager

**SimpleContactManager** is a lightweight ASP.NET Core Web API designed to manage contact information efficiently. It provides a foundational structure for CRUD (Create, Read, Update, Delete) operations on contact data, making it suitable for learning purposes or as a starting point for more complex applications.

---

## 📸 Screenshots

> Coming soon... *(screenshots to showcase API responses or Swagger UI)*

---

## 🔧 Features

- **RESTful API Endpoints:** Offers standard endpoints to create, retrieve, update, and delete contacts.
- **Entity Framework Core Integration:** Utilizes EF Core for seamless database interactions.
- **Modular Architecture:** Organized into clear folders like Controllers, Models, Data, and Middleware for maintainability.
- **Sample Configuration:** Includes a sample.appsettings.json to guide initial setup

---

## 🧑‍💻 Tech Stack

- **ASP.NET Core Web API** (.NET 6+)
- **Entity Framework Core**
- **SQL Server / LocalDb**

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [SQL Server Express or LocalDb](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)



### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Nirob-Barman/SimpleContactManager.git
   cd SimpleContactManager
	```

2. **Restore dependencies:**
   ```bash
   dotnet restore
	```

3. **Apply migrations and create the database:**
   ```bash
   dotnet ef database update
	```

4. **Run the project:**
   ```bash
   dotnet run
	```

5. **Open your browser and visit:**
   ```bash
   https://localhost:5001
	```

## 🔐 Configuration
Before running the project, make sure to set up your own `appsettings.json` file with the correct database connection string. For security, sensitive information like usernames and passwords should not be committed to version control.

A `sample.appsettings.json` file is included for reference. Create your actual `appsettings.json` in the project root like this:

<details>
<summary>Click to view sample.appsettings.json</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "AllowedHosts": "*"
}
```

</details>
📝 Replace YOUR_SERVER_NAME, YOUR_DATABASE_NAME, YOUR_USERNAME, and YOUR_PASSWORD with your actual SQL Server values.


## 🗃 Project Structure
	```
	SimpleContactManager/
	├── Controllers/           # MVC Controllers
	├── Models/                # Data models
	├── Data/                  # DbContext
	├── Middleware/            # Custom middleware
	├── Migrations/            # EF Core migrations
	├── Properties/            # Project properties
	├── Shared/                # Shared resources
	├── SimpleContactManager.csproj
	├── SimpleContactManager.sln
	└── sample.appsettings.json
	```

## ✍️ Author

- 👤 **Nirob Barman**  
- [![Medium](https://img.shields.io/badge/Medium-Blog-black?logo=medium)](https://nirob-barman.medium.com/)
- [![LinkedIn](https://img.shields.io/badge/LinkedIn-Connect-blue?logo=linkedin)](https://www.linkedin.com/in/nirob-barman/)
- [![Portfolio](https://img.shields.io/badge/Portfolio-Visit-brightgreen?logo=firefox-browser)](https://nirob-barman-19.web.app/)
- [![Email](https://img.shields.io/badge/Email-Contact-orange?logo=gmail)](mailto:nirob.barman.19@gmail.com)

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

