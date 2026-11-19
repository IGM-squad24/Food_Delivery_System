# 🚚 Food Delivery Management System

![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-blueviolet?style=for-the-badge&logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-8.0.6-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?style=for-the-badge&logo=microsoft-sql-server)
![.NET 9.0](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)

A comprehensive, full-stack Food Delivery Management System built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**. This application allows for managing restaurants, menus, customers, delivery drivers, and the complete order lifecycle.

---

## 📸 Screenshots

### 🏠 Dashboard & Orders
| Dashboard | Order Form |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/65c5e402-2692-40fe-8afc-ba5c6a43be2b" width="100%" alt="Dashboard" /> | <img src="https://github.com/user-attachments/assets/a835dea4-176c-4443-8894-5110b0b57bbf" width="100%" alt="Order Form" /> |

| Managing Orders | Order Status |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/94b66e86-5921-476f-8e39-9b7646bbc5b7" width="100%" alt="Managing Orders" /> | <img src="https://github.com/user-attachments/assets/0a533294-2f7f-47b8-bf4b-68341b2f42b1" width="100%" alt="Order Details" /> |

### 🍔 Menus, Customers & Drivers
| Menu Items | Menu Management |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/9e000360-0f44-46cb-a578-20ab619221f1" width="100%" alt="Menu Items" /> | <img src="https://github.com/user-attachments/assets/22549afd-c1b1-4f93-a300-b10a04f8e212" width="100%" alt="Menu Management" /> |

| Customers | Delivery Drivers |
| :---: | :---: |
| <img src="https://github.com/user-attachments/assets/2add8c94-98d3-40ec-b451-7aca7da81dab" width="100%" alt="Customers" /> | <img src="https://github.com/user-attachments/assets/d2a38755-1dd8-434c-b0e4-1d0f1114398d" width="100%" alt="Delivery Drivers" /> |

---

## 🗄️ Entity Relationship (ER) Diagram

<div align="center">
  <img src="https://github.com/user-attachments/assets/c825eed4-d1a7-46cc-9d4a-7d02db93a1e4" width="80%" alt="ER Diagram" />
</div>

---

## 🚀 Key Features

| Feature | Description |
| :--- | :--- |
| **Dashboard** | Live stats (total orders, active orders, revenue, restaurants, customers, drivers) + 5 most recent orders. |
| **Order Workflow** | Status progression: `Pending` → `Confirmed` → `Preparing` → `Out for Delivery` → `Delivered` (with `Cancel` at any step). |
| **Search & Filter** | Every listing page has search; MenuItems filter by category and restaurant; Orders filter by status. |
| **Seed Data** | 3 restaurants, 9 menu items, 3 customers, 3 drivers, 3 orders with 8 order items pre-loaded. |
| **Responsive UI** | Bootstrap 5 + Bootstrap Icons, mobile-friendly tables, card-based layout. |
| **Toast Notifications** | Success messages via `TempData` on all CRUD operations. |

---

## 🏗️ Architecture & Project Structure

```text
FoodDeliverySystem/
├── Controllers/
│   ├── HomeController.cs           # Dashboard with stats & recent orders
│   ├── RestaurantsController.cs    # Full CRUD + search
│   ├── MenuItemsController.cs      # Full CRUD + filter by category/restaurant
│   ├── CustomersController.cs      # Full CRUD + search
│   ├── DeliveryDriversController.cs# Full CRUD + search
│   └── OrdersController.cs         # Full CRUD + status workflow + search/filter
├── Models/
│   ├── Restaurant.cs
│   ├── MenuItem.cs
│   ├── Customer.cs
│   ├── DeliveryDriver.cs
│   ├── Order.cs                    # Includes OrderStatus enum
│   ├── OrderItem.cs
│   └── ErrorViewModel.cs
├── Data/
│   └── ApplicationDbContext.cs     # EF Core context + Fluent API + seed data
├── Views/
│   ├── Home/Index.cshtml           # Dashboard with 6 stat cards + recent orders
│   ├── Restaurants/                # Index, Details, Create, Edit, Delete
│   ├── MenuItems/                  # Index, Details, Create, Edit, Delete
│   ├── Customers/                  # Index, Details, Create, Edit, Delete
│   ├── DeliveryDrivers/            # Index, Details, Create, Edit, Delete
│   ├── Orders/                     # Index, Details, Create, Edit, Delete
│   └── Shared/_Layout.cshtml       # Navigation bar + toast notifications
├── Program.cs                      # DI + EF Core + auto DB creation
├── appsettings.json                # SQL Server connection string
└── FoodDeliverySystem.csproj
