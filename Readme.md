# Renta - Premium Rental & Event Management API

## 🚀 What the Project Does
Renta is a high-performance, modular .NET 9.0 Web API designed to handle the complex logistics of luxury rentals (Cars, Yachts) and Event ticketing. It provides a robust backend for:
- **Inventory Management:** Multi-entity support for Cars, Yachts, and Events with rich multimedia (Cloudinary integration).
- **Secure Ticketing:** QR-code-based ticket generation and validation with automated email delivery.
- **Integrated Payments:** Full Stripe integration for secure transactions and automated order processing via webhooks.
- **Clean Architecture:** Built using CQRS, Repository Pattern, and FastEndpoints for maximum scalability and maintainability.

---

## 🛠️ Getting Started (Local Setup)

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Stripe CLI](https://stripe.com/docs/stripe-cli) (for webhook testing)

### Step-by-Step Execution
1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/renta.git
   cd renta
   ```

2. **Configure Environment Variables:**
   Update `appsettings.json` in `Renta.WebApi` or set environment variables (see section below).

3. **Apply Database Migrations:**
   ```bash
   dotnet ef database update --project Renta.Infrastructure --startup-project Renta.WebApi
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project Renta.WebApi
   ```

---

## 🧪 Testing the API

### Swagger UI
Once the app is running, you can explore and test all endpoints via the interactive Swagger documentation:- **URL:** `http://localhost:5037/scalar` 

### Example Request: Uploading a Car Photo
**Endpoint:** `POST /api/file/upload`  
**Content-Type:** `multipart/form-data`

**Request Body:**
- `File`: [Your Image File]
- `CarId`: [Some car Id]
- `Type`: `Gallery`

**Response (200 OK):**
```json
{
  "photoId": "019b60c1-d33d-8bb7-b72f-6e50bf1c0547",
  "url": "http://res.cloudinary.com/dnta7zbad/image/upload/v1/car/...",
  "secureUrl": "https://res.cloudinary.com/dnta7zbad/image/upload/v1/car/...",
  "format": "jpg",
  "size": 54210
}
```

---

## 💳 Stripe Sandbox Testing

To test the payment flow without real money:

1. **Use Test Cards:** Use Stripe's [test card numbers](https://stripe.com/docs/testing#cards) (e.g., `4242 4242 4242 4242`).
2. **Test Webhooks Locally:**
   - Install Stripe CLI and login: `stripe login`
   - Forward webhooks to your local instance:
     ```bash
     stripe listen --forward-to http://localhost:5037/api/stripe/webhook
     ```
   - Copy the `whsec_...` signing secret provided by the CLI into your `StripeSettings__WebhookSecret`.
3. **Trigger Events:** You can trigger a successful payment via the CLI:
   ```bash
   stripe trigger payment_intent.succeeded
   ```

---

## 🔑 Required Environment Variables

For production or local overrides, ensure the following variables are set:

| Category | Variable Name | Description |
| :--- | :--- | :--- |
| **Database** | `ConnectionStrings__WriteDefaultConnection` | PostgreSQL connection string |
| **Auth** | `JwtSettings__Secret` | 32+ character secret key |
| **Stripe** | `StripeSettings__SecretKey` | Your `sk_test_...` key |
| **Stripe** | `StripeSettings__WebhookSecret` | Webhook signing secret |
| **Cloudinary** | `CloudinarySettings__CloudName` | Cloudinary Cloud Name |
| **Cloudinary** | `CloudinarySettings__ApiKey` | Cloudinary API Key |
| **Cloudinary** | `CloudinarySettings__ApiSecret` | Cloudinary API Secret |
| **Email** | `EmailSettings__Password` | App password for SMTP |

---

## 🏗️ Architecture Details
- **CQRS:** Commands (Write) and Queries (Read) are separated to optimize performance.
- **Validation:** FluentValidation is used to ensure data integrity before processing.
- **Security:** Role-based authorization (Admin/Client) and JWT-based authentication.