# CSVEditor
## ASP.NET Core 8.0 MVC + Razor Pages App

This project is a **web application** built with **ASP.NET Core 8.0 MVC** using **Razor Pages**.  
It uses **Entity Framework Core with Microsoft SQL Server** as the database provider.

---

## 📦 Requirements

Make sure you have these installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB)  
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) (optional but recommended)

---

## ⚙️ Setup & Run

1. Open a terminal and navigate to the project folder (where your `.csproj` is):

   ```bash
   cd CSVEditor
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Install EF Core tools (if not installed):

   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. Apply database migrations (make sure SQL Server is running and your `appsettings.json` connection string is correct):

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

By default, the app will run on: **https://localhost:7150**

---

## 🔑 Notes

- Ensure SQL Server is running before applying migrations.  
- To add a new migration:

  ```bash
  dotnet ef migrations add MigrationName
  dotnet ef database update
  ```

- The app uses **Razor Pages** for rendering UI.
- [Open CSV example](example.csv)
---

## 🛠 Example Connection String (`appsettings.json`)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=csvAppDb;Trusted_Connection=True;Encrypt=False;"
}
```

Change `Server` and `Database` if you are using SQL Server Express or a remote SQL Server.
