# FreshInventory

![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)

## Introduction

**FreshInventory** is a modern inventory management system designed to help businesses efficiently track and manage their fresh ingredients. Built with scalability and maintainability in mind, FreshInventory leverages contemporary software development practices to ensure reliable performance and ease of use.

## Technologies Used

- ![.NET](https://img.shields.io/badge/.NET_8.0-blue)
- ![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-green)
- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-purple)
- ![CQRS](https://img.shields.io/badge/CQRS-brightgreen)
- ![MediatR](https://img.shields.io/badge/MediatR-yellow)
- ![AutoMapper](https://img.shields.io/badge/AutoMapper-orange)
- ![Serilog](https://img.shields.io/badge/Serilog-red)
- ![Swagger](https://img.shields.io/badge/Swagger-brightgreen)
- ![SQLite](https://img.shields.io/badge/SQLite-blueviolet)
- ![GIT](https://img.shields.io/badge/GIT-Orange)

---

## Project Objective

The primary objective of **FreshInventory** is to monitor and manage the stock of fresh ingredients in a kitchen, ensuring it is always up-to-date and minimizing waste. By automating and streamlining inventory processes, FreshInventory helps businesses maintain optimal stock levels, reduce spoilage, and enhance overall operational efficiency.

### Key Goals:

- **Accurate Stock Monitoring**: Keep real-time track of ingredient quantities to prevent shortages and overstocking.
- **Waste Reduction**: Implement alerts and analytics to minimize ingredient spoilage and waste.
- **Operational Efficiency**: Streamline inventory management to save time and resources for kitchen staff.

## Features

- **Ingredient Management**: Easily add, update, delete, and retrieve ingredient records with detailed attributes such as quantity, unit, cost, category, supplier, purchase date, and expiry date.
- **Automated Date Tracking**: Automatically records creation and update timestamps for each ingredient, ensuring accurate audit trails without manual intervention.
- **Low Stock Alerts**: Receive automatic notifications when ingredient levels fall below predefined thresholds, enabling timely reordering.
- **Detailed Inventory Reports**: Generate comprehensive reports to analyze stock levels, usage patterns, and identify areas for improvement.
- **Recipe Integration**: Integrate with recipe systems to visualize which dishes can be prepared with the available ingredients, aiding in menu planning and inventory optimization.
- **Robust Logging**: Integrates Serilog for structured and comprehensive logging, facilitating easier monitoring and troubleshooting.
- **Exception Handling**: Implements robust exception handling mechanisms to gracefully manage errors and provide meaningful feedback.
- **API Documentation**: Utilizes Swagger to provide interactive API documentation, making it easier for developers to understand and test the endpoints.
- **Pagination & Filtering**: Efficiently handles large datasets with pagination and filtering capabilities, enhancing performance and user experience.
- **CQRS Pattern**: Implements the Command Query Responsibility Segregation (CQRS) pattern to separate read and write operations, improving scalability and maintainability.
- **Clean Architecture**: Adheres to Clean Architecture principles, ensuring a well-organized, maintainable, and scalable codebase.

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html)
- [Git](https://git-scm.com/downloads)
