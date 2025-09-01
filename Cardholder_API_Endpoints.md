# Cardholder API - Endpoint Documentation

## 📋 Table of Contents
- [Authentication & Profile Management](#authentication--profile-management)
- [Card Management](#card-management)
- [Dashboard & Reporting](#dashboard--reporting)
- [QR Code Operations](#qr-code-operations)
- [Transaction Management](#transaction-management)
- [API Response Format](#api-response-format)
- [Authentication Flow](#authentication-flow)

---

## 🔐 Authentication & Profile Management

### 1. User Registration
**Endpoint:** `POST /api/auth/register`

**Description:** Creates a new cardholder account

**Request Body:**
```json
{
  "firstName": "Ahmet",
  "lastName": "Yılmaz",
  "email": "ahmet@example.com",
  "password": "123456",
  "phoneNumber": "+905551234567",
  "address": "İstanbul, Türkiye"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "firstName": "Ahmet",
      "lastName": "Yılmaz",
      "email": "ahmet@example.com",
      "phoneNumber": "+905551234567",
      "address": "İstanbul, Türkiye"
    }
  },
  "message": "User registered successfully"
}
```

**Features:**
- Email uniqueness validation
- Password hashing with BCrypt
- JWT token generation
- Input validation

---

### 2. User Login
**Endpoint:** `POST /api/auth/login`

**Description:** Authenticates user and returns JWT token

**Request Body:**
```json
{
  "email": "ahmet@example.com",
  "password": "123456"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "firstName": "Ahmet",
      "lastName": "Yılmaz",
      "email": "ahmet@example.com"
    }
  },
  "message": "Login successful"
}
```

---

### 3. Get User Profile
**Endpoint:** `GET /api/auth/profile`

**Description:** Retrieves authenticated user's profile information

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "firstName": "Ahmet",
    "lastName": "Yılmaz",
    "email": "ahmet@example.com",
    "phoneNumber": "+905551234567",
    "address": "İstanbul, Türkiye"
  },
  "message": "Profile retrieved successfully"
}
```

---

### 4. Update User Profile (Partial Update)
**Endpoint:** `PUT /api/auth/profile`

**Description:** Updates user profile with partial data (only provided fields are updated)

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "firstName": "Ahmet Yeni",
  "phoneNumber": "+905559876543"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "firstName": "Ahmet Yeni",
    "lastName": "Yılmaz",
    "email": "ahmet@example.com",
    "phoneNumber": "+905559876543",
    "address": "İstanbul, Türkiye"
  },
  "message": "Profile updated successfully"
}
```

**Features:**
- Partial update support
- Password change validation
- Field-specific updates

---

## 💳 Card Management

### 1. Get User Cards
**Endpoint:** `GET /api/cards`

**Description:** Lists all cards belonging to the authenticated user

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "cardNumber": "1234567890123456",
      "cardType": "Debit",
      "balance": 1000.00,
      "status": "Active",
      "expiryDate": "2025-12-31"
    }
  ],
  "message": "Cards retrieved successfully"
}
```

---

### 2. Get Card Details
**Endpoint:** `GET /api/cards/{cardId}`

**Description:** Retrieves detailed information of a specific card

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "cardNumber": "1234567890123456",
    "cardType": "Debit",
    "balance": 1000.00,
    "status": "Active",
    "expiryDate": "2025-12-31",
    "transactions": [
      {
        "id": 1,
        "amount": 50.00,
        "type": "Payment",
        "status": "Success",
        "date": "2024-01-15T10:30:00Z"
      }
    ]
  },
  "message": "Card details retrieved successfully"
}
```

---

### 3. Create New Card
**Endpoint:** `POST /api/cards`

**Description:** Creates a new card for the authenticated user

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "cardNumber": "1234567890123456",
  "pin": "1234",
  "cardType": "Debit",
  "expiryDate": "2025-12-31"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 2,
    "cardNumber": "1234567890123456",
    "cardType": "Debit",
    "balance": 0.00,
    "status": "Active",
    "expiryDate": "2025-12-31"
  },
  "message": "Card created successfully"
}
```

**Features:**
- Card number uniqueness validation
- PIN encryption
- Card type validation

---

### 4. Update Card Status
**Endpoint:** `PUT /api/cards/{cardId}/status`

**Description:** Updates the status of a specific card (Active/Inactive)

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "status": "Inactive"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "status": "Inactive"
  },
  "message": "Card status updated successfully"
}
```

---

## 📊 Dashboard & Reporting

### 1. Get Dashboard
**Endpoint:** `GET /api/dashboard`

**Description:** Retrieves comprehensive dashboard statistics for the user

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "cardholderStats": {
      "totalTransactions": 25,
      "totalAmount": 1250.00,
      "successfulTransactions": 23,
      "failedTransactions": 2
    },
    "recentTransactions": [
      {
        "id": 1,
        "amount": 50.00,
        "type": "Payment",
        "status": "Success",
        "date": "2024-01-15T10:30:00Z",
        "description": "Market purchase"
      }
    ],
    "cardBalances": [
      {
        "cardId": 1,
        "cardNumber": "1234567890123456",
        "balance": 950.00,
        "status": "Active"
      }
    ]
  },
  "message": "Dashboard data retrieved successfully"
}
```

---

### 2. Get Transaction History
**Endpoint:** `GET /api/dashboard/transactions`

**Description:** Retrieves paginated transaction history

**Headers:**
```
Authorization: Bearer {token}
```

**Query Parameters:**
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

**Response:**
```json
{
  "success": true,
  "data": {
    "transactions": [
      {
        "id": 1,
        "amount": 50.00,
        "type": "Payment",
        "status": "Success",
        "date": "2024-01-15T10:30:00Z",
        "description": "Market purchase",
        "merchantName": "ABC Market"
      }
    ],
    "pagination": {
      "currentPage": 1,
      "totalPages": 3,
      "totalItems": 25,
      "pageSize": 10
    }
  },
  "message": "Transaction history retrieved successfully"
}
```

---

## 📱 QR Code Operations

### 1. Generate QR Code
**Endpoint:** `POST /api/qr/generate`

**Description:** Generates a QR code for payment processing

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "merchantId": 1,
  "amount": 50.00,
  "description": "Market purchase"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "qrCodeId": "QR_123456789",
    "merchantId": 1,
    "amount": 50.00,
    "description": "Market purchase",
    "status": "Pending",
    "expiryTime": "2024-01-15T11:30:00Z",
    "qrCodeData": "QR_123456789|1|50.00|Market purchase"
  },
  "message": "QR code generated successfully"
}
```

**Features:**
- Unique QR code ID generation
- Pending transaction creation
- Expiry time setting
- QR code data for frontend display

---

### 2. Process QR Payment
**Endpoint:** `POST /api/qr/process`

**Description:** Processes payment using QR code and card information

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "qrCodeId": "QR_123456789",
  "cardId": 1,
  "pin": "1234"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "amount": 50.00,
    "status": "Pending",
    "cardBalance": 950.00,
    "requiresConfirmation": true
  },
  "message": "Payment processed successfully. Please confirm the transaction."
}
```

**Features:**
- Card and PIN validation
- Balance verification
- Transaction creation
- Confirmation requirement

---

### 3. Get QR Code Status
**Endpoint:** `GET /api/qr/{qrCodeId}/status`

**Description:** Retrieves the current status of a QR code

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "qrCodeId": "QR_123456789",
    "status": "Success",
    "amount": 50.00,
    "description": "Market purchase",
    "transactionId": 456,
    "processedAt": "2024-01-15T10:35:00Z"
  },
  "message": "QR code status retrieved successfully"
}
```

---

## 💰 Transaction Management

### 1. Confirm Transaction
**Endpoint:** `POST /api/transactions/confirm`

**Description:** Confirms or cancels a pending transaction

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "transactionId": 456,
  "action": "Confirm"
}
```

**Response (Confirm):**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "status": "Success",
    "amount": 50.00,
    "cardBalance": 950.00,
    "processedAt": "2024-01-15T10:35:00Z"
  },
  "message": "Transaction confirmed successfully"
}
```

**Response (Cancel):**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "status": "Cancelled"
  },
  "message": "Transaction cancelled successfully"
}
```

---

### 2. Get Transaction Details
**Endpoint:** `GET /api/transactions/{transactionId}`

**Description:** Retrieves detailed information of a specific transaction

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 456,
    "amount": 50.00,
    "type": "Payment",
    "status": "Success",
    "description": "Market purchase",
    "merchantName": "ABC Market",
    "cardNumber": "1234567890123456",
    "date": "2024-01-15T10:35:00Z"
  },
  "message": "Transaction details retrieved successfully"
}
```

---

### 3. Update Transaction Status
**Endpoint:** `PUT /api/transactions/{transactionId}/status`

**Description:** Manually updates the status of a transaction

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "status": "Success"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "status": "Success"
  },
  "message": "Transaction status updated successfully"
}
```

---

### 4. Process Successful Payment
**Endpoint:** `POST /api/transactions/success`

**Description:** Marks a transaction as successful and updates card balance

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "transactionId": 456
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "status": "Success",
    "cardBalance": 950.00
  },
  "message": "Payment processed successfully"
}
```

---

### 5. Process Failed Payment
**Endpoint:** `POST /api/transactions/failed`

**Description:** Marks a transaction as failed with reason

**Headers:**
```
Authorization: Bearer {token}
```

**Request Body:**
```json
{
  "transactionId": 456,
  "reason": "Insufficient balance"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "transactionId": 456,
    "status": "Failed",
    "reason": "Insufficient balance"
  },
  "message": "Payment marked as failed"
}
```

---

## 📋 API Response Format

All API endpoints return responses in a standardized format:

### Success Response
```json
{
  "success": true,
  "data": {
    // Response data here
  },
  "message": "Operation completed successfully"
}
```

### Error Response
```json
{
  "success": false,
  "data": null,
  "message": "Error description",
  "errors": [
    {
      "field": "email",
      "message": "Email is required"
    }
  ]
}
```

---

## 🔄 Authentication Flow

### 1. Registration Flow
1. User submits registration data
2. System validates email uniqueness
3. Password is hashed with BCrypt
4. User account is created
5. JWT token is generated and returned

### 2. Login Flow
1. User submits email and password
2. System validates credentials
3. JWT token is generated and returned
4. Token is used for subsequent API calls

### 3. Protected Endpoints
- All endpoints except `/api/auth/register` and `/api/auth/login` require authentication
- Include `Authorization: Bearer {token}` header
- Token contains user ID for authorization

---

## 🚀 Complete User Journey Example

### Step 1: Registration
```http
POST /api/auth/register
{
  "firstName": "Ahmet",
  "lastName": "Yılmaz",
  "email": "ahmet@example.com",
  "password": "123456",
  "phoneNumber": "+905551234567",
  "address": "İstanbul, Türkiye"
}
```

### Step 2: Login
```http
POST /api/auth/login
{
  "email": "ahmet@example.com",
  "password": "123456"
}
```

### Step 3: Create Card
```http
POST /api/cards
Authorization: Bearer {token}
{
  "cardNumber": "1234567890123456",
  "pin": "1234",
  "cardType": "Debit",
  "expiryDate": "2025-12-31"
}
```

### Step 4: Generate QR Code
```http
POST /api/qr/generate
Authorization: Bearer {token}
{
  "merchantId": 1,
  "amount": 50.00,
  "description": "Market purchase"
}
```

### Step 5: Process QR Payment
```http
POST /api/qr/process
Authorization: Bearer {token}
{
  "qrCodeId": "QR_123456789",
  "cardId": 1,
  "pin": "1234"
}
```

### Step 6: Confirm Transaction
```http
POST /api/transactions/confirm
Authorization: Bearer {token}
{
  "transactionId": 456,
  "action": "Confirm"
}
```

### Step 7: View Dashboard
```http
GET /api/dashboard
Authorization: Bearer {token}
```

---

## 📝 Notes

- All timestamps are in ISO 8601 format (UTC)
- Amounts are in decimal format with 2 decimal places
- Card numbers are masked for security in responses
- PINs are never returned in responses
- JWT tokens expire after 24 hours by default
- All endpoints support CORS for frontend integration

---

*This documentation covers all endpoints available in the Cardholder API. For technical implementation details, refer to the source code and Swagger documentation.* 