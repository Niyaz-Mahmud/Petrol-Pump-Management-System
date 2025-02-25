# ⛽ Petrol Pump Management System

A modern solution for efficient fuel station operations with real-time tracking and reporting.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Core](https://img.shields.io/badge/.NET-6.0-blue)](https://dotnet.microsoft.com)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019+-red)](https://www.microsoft.com/sql-server)

The Petrol Pump Management System is a feature-rich desktop application built with C# and .NET 6, designed to optimize petrol station operations through digital automation. It provides robust tools for inventory control, financial management, and workforce organization while ensuring data security and operational transparency.

## ✨ Key Features

### 🚦 Core Modules
| Module | Description |
|--------|-------------|
| Fuel Management | <ul><li>Track multiple fuel types (Petrol, Diesel, CNG)</li><li>Automatic price configuration</li><li>Nozzle-level quantity monitoring</li></ul> |
| Smart Inventory | <ul><li>Real-time stock alerts</li><li>Supplier order history</li><li>Tank lorry receipt management</li></ul> |
| Financial Hub | <ul><li>Daily sales reports (PDF/Excel)</li><li>Expense categorization</li><li>Profit/loss statements</li></ul> |
| HR Management | <ul><li>Shift scheduling</li><li>Payroll integration</li><li>Role-based access control</li></ul> |
| Customer Interface | <ul><li>Digital receipts</li><li>Loyalty programs</li><li>Prepaid card support</li></ul> |

### 🔐 Security Features
- AES-256 encrypted database
- Two-factor authentication
- Audit logs for sensitive operations
- Role-based permissions (Admin, Cashier, Manager)

## 🛠️ Technology Stack
- Frontend: Windows Presentation Foundation (WPF)
- Backend: .NET 6 with Entity Framework Core
- Database: Microsoft SQL Server 2022
- Reporting: Crystal Reports
- Dependency Injection: Autofac
- Testing: xUnit + Moq

## 🚀 Installation Guide

### Prerequisites
- Windows 10/11
- .NET 6 Runtime
- SQL Server 2019+ with SSMS
- Visual Studio 2022 (for development)

### Setup Instructions
1. Clone Repository
   ```bash
   git clone https://github.com/Niyaz-Mahmud/Petrol-Pump-Management-System.git
   cd Petrol-Pump-Management-System
   ```

2. Database Configuration
   ```sql
   -- Using SQL Server Management Studio
   CREATE DATABASE PetrolPumpDB;
   USE PetrolPumpDB;
   :r Database/Schema.sql
   :r Database/SeedData.sql
   ```

3. Application Setup
   - Update connection string in `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=PetrolPumpDB;Integrated Security=True;TrustServerCertificate=True;"
     }
     ```
   - Restore NuGet packages:
     ```powershell
     dotnet restore
     ```

4. Launch Application
   ```powershell
   dotnet run --project PetrolPump.UI
   ```

## 📖 User Documentation

### Dashboard Overview
![Dashboard Preview](docs/images/dashboard.png) *(Sample dashboard image)*

### Key Workflows
1. Fuel Sale
   - Select pump number → Choose fuel type → Enter quantity → Process payment (Cash/Card)

2. Inventory Replenishment
   - Navigate to Inventory → Create Purchase Order → Verify delivery → Update stock

3. Employee Management
   - Add new staff → Assign shifts → Track attendance → Generate payroll

4. Financial Reports
   - Select report type → Choose date range → Export/Print

## 🤝 Contributing

We welcome contributions! Please follow these steps:
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Development Guidelines
- Write unit tests for new features
- Maintain 85%+ code coverage
- Follow C# coding conventions
- Update documentation accordingly

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.

## 📞 Support

For technical assistance or feature requests:
- 📧 Email: niyazmahmud213@gmail.com
- 🐛 [Issue Tracker](https://github.com/Niyaz-Mahmud/Petrol-Pump-Management-System/issues)

