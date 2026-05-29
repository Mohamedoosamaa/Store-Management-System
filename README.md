# Store Management System

A desktop Store Management System built using WPF, MVVM architecture, and SQL Server.

## Features

### Authentication & Authorization
- Secure login system
- Role-based access control
- Admin Dashboard
- Cashier Dashboard

### Dashboard
- Total Products
- Low Stock Products
- Today's Sales
- Monthly Revenue
- Weekly Sales Chart
- Top Selling Products Chart
- Inventory Distribution Chart

### Product Management
- Add Products
- Update Products
- Delete Products (Soft Delete)
- Search Products
- Category Management
- Stock Tracking

### Inventory Management
- Monitor Inventory Levels
- Low Stock Alerts
- Inventory Value Calculation

### POS (Point of Sale)
- Product Selection
- Shopping Cart
- Quantity Management
- Checkout Process
- Automatic Stock Updates

### Transaction Management
- Transaction History
- Transaction Details
- Sales Tracking
- Revenue Reporting

## Technologies Used

- C#
- WPF
- MVVM Pattern
- SQL Server
- ADO.NET
- LiveCharts
- Git & GitHub

## Project Structure

```text
StoreManagementSystem
│
├── Models
├── ViewModels
├── Views
├── Repositories
├── Services
├── Commands
├── Helpers
├── Database
│
└── StoreManagementSystem.sln
```

## Database

The database scripts are located in:

```text
Database/
├── StoreManagement.sql
└── SeedData.sql
```

## Installation

### 1. Clone Repository

```bash
git clone https://github.com/Mohamedoosamaa/Store-Management-System.git
```

### 2. Open Project

Open:

```text
StoreManagementSystem.sln
```

using Visual Studio.

### 3. Configure SQL Server

Update the connection string in:

```csharp
Helpers/DbConnection.cs
```

### 4. Create Database

Run:

```sql
Database/StoreManagement.sql
```

Then:

```sql
Database/SeedData.sql
```

### 5. Run Project

Press:

```text
F5
```


### Login System
- Secure user authentication

### Dashboard
- Sales analytics
- Inventory monitoring
- Top selling products

### POS System
- Product checkout
- Cart management

### Inventory Management
- Product stock tracking

## Team Members

- Abdelrahman Helmy
- Mohamed Osama
- Andrew Bassem
- Hesham Shahat

## License

This project is developed for educational and academic purposes.
