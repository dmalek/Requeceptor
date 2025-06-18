# 📥 RequestReceptor

**RequestReceptor** is a modular and extensible .NET component for inspecting and logging incoming HTTP requests. It supports multiple request formats (JSON, XML, SOAP) and is designed to be part of a request monitoring system with a future Blazor-based user interface.

---

## ✨ Features

- ✅ Parses and inspects incoming HTTP requests
- ✅ Supports JSON and XML (including SOAP) body formats
- ✅ Detects action name from request body or route
- ✅ Extracts full request metadata (headers, cookies, IP addresses, etc.)
- ✅ Extensible with custom format parsers via the Strategy Pattern
- ✅ Built-in integration with ASP.NET Core (`HttpContext`, `RouteData`, `HttpRequest`)

---

## 🧩 Use Case

This library is a core component of a larger system that:

- Receives HTTP requests
- Logs and stores request details
- Allows users to review requests via a Blazor UI (under development)
- Enables filtering, searching, and auditing of requests

---

## 🛠️ Technologies Used

- .NET 8.0+
- ASP.NET Core
- Dependency Injection
- Strategy Pattern
- Blazor (for future UI)

---

## 📦 Planned Features

- 🔍 Web dashboard (Blazor) for request browsing and filtering
- 🧠 Smart format detection and parsing (SOAP-aware XML)
- 🗂️ Storage support (in-memory, file-based, or EF Core)
- 📤 Export request data to JSON/XML
- 🔐 Optional authentication for secure access to UI
