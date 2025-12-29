# Renta Project Documentation

## Overview

Renta is a modular .NET 9.0 application for managing rentals, ticket sales, payments, and file uploads. It uses FastEndpoints for API structure, Entity Framework Core with PostgreSQL for data, and integrates with Stripe for payments and Cloudinary for file storage.

---

## Architecture

- **CQRS Pattern:** Commands and queries are separated for clean, maintainable code.
- **Repository Pattern:** Abstracts data access for testability and flexibility.
- **Dependency Injection:** All services and repositories are injected for loose coupling.
- **Entity Framework Core:** Used for ORM and migrations.
- **FastEndpoints:** Minimal API endpoints with strong typing and validation.

---

## Key Features

### 1. Ticketing & Events
- List, buy, and manage tickets for events.
- QR code generation for ticket validation.
- Email notifications on ticket purchase.

### 2. Payments
- Stripe integration for payment processing.
- Webhook endpoint for payment status updates.
- Idempotency and metadata validation for safe payment handling.

### 3. File Uploads
- Upload images to Cloudinary for Cars, Yachts, and Events.
- Each entity has a one-to-many relationship with Photos.
- File size and type validation.

### 4. Database Design
- PostgreSQL with proper foreign key relationships.
- Photo entity links to Car, Yacht, or Event via nullable FKs.
- Cascade delete for related photos.

---

## Main Entities

- **Car, Yacht, Event:** Each has a `Photos` collection.
- **Photo:** Has nullable `CarId`, `YachtId`, `EventId` FKs and navigation properties.
- **Ticket:** Linked to Event and User.
- **User:** Identity and role management.

---

## Example: File Upload Flow

1. **Endpoint:** `POST /file/upload`
2. **Command:** `UploadFileCommand` with one of `CarId`, `YachtId`, or `EventId`.
3. **Handler:** Validates input, uploads to Cloudinary, creates a Photo, and links it to the correct entity.
4. **Database:** Only the relevant FK is set; others are null.

---

## Running Migrations

```bash
dotnet ef database update --project Renta.Infrastructure --startup-project Renta.WebApi
```

---

## Stripe Webhook

- **Endpoint:** `/api/stripe/webhook`
- **Validates** Stripe signature and processes payment events.

---

## Cloudinary Integration

- **Settings:** Configured in `appsettings.json` and registered in DI.
- **Service:** `IFileService` abstraction for uploads.

---

## Error Handling

- All endpoints return clear error messages and HTTP status codes.

---
