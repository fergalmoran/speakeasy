# SpeakEasy - Secure End-to-End Encrypted Messaging

A complete secure messaging application with end-to-end encryption consisting of:

1. **Web Frontend** - React + Vite + shadcn/ui
2. **Backend API** - ASP.NET Core + Entity Framework
3. **Mobile App** - React Native (Cross-platform)

## Features

- 🔐 End-to-end encryption using RSA
- 💬 Real-time messaging
- 🔑 Secure key exchange
- 👥 User management
- 📱 Cross-platform support (Web, iOS, Android)

## Project Structure

```
speakeasy/
├── frontend-web/          # React web application
├── backend-api/           # ASP.NET Core API
├── mobile-app/            # React Native mobile app
└── REQUIREMENTS.md        # Project requirements
```

## Getting Started

### Prerequisites

- Node.js 18+ 
- .NET 8 SDK
- PostgreSQL or SQLite
- React Native development environment (for mobile)

### Backend API

```bash
cd backend-api
dotnet restore
dotnet ef database update
dotnet run
```

The API will be available at `http://localhost:5000`

### Web Frontend

```bash
cd frontend-web
npm install
npm run dev
```

The web app will be available at `http://localhost:3000`

### Mobile App

```bash
cd mobile-app
npm install

# For iOS
npm run ios

# For Android
npm run android
```

## Configuration

### Backend API

Edit `backend-api/appsettings.json`:

```json
{
  "DatabaseProvider": "Sqlite",
  "ConnectionStrings": {
    "Sqlite": "Data Source=speakeasy.db",
    "PostgreSQL": "Host=localhost;Database=speakeasy;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!"
  }
}
```

### Web Frontend

Create `frontend-web/.env`:

```
VITE_API_URL=http://localhost:5000/api
```

### Mobile App

Edit `mobile-app/src/services/api.ts` to set your API URL.

## Security

This application implements end-to-end encryption:

- Messages are encrypted on the sender's device
- Messages are decrypted on the receiver's device
- The server only stores encrypted messages
- RSA-2048 key pairs for encryption
- JWT tokens for authentication

## Technology Stack

### Backend
- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL / SQLite
- JWT Authentication
- BCrypt for password hashing

### Web Frontend
- React 18
- Vite
- TypeScript
- shadcn/ui components
- TailwindCSS
- React Router

### Mobile
- React Native
- TypeScript
- React Navigation
- AsyncStorage

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login user
- `GET /api/auth/me` - Get current user
- `GET /api/auth/users` - Get all users

### Messages
- `POST /api/messages` - Send message
- `GET /api/messages/conversations` - Get conversations
- `GET /api/messages/conversation/:userId` - Get conversation with user

### Key Exchange
- `POST /api/keyexchange/initiate` - Initiate key exchange
- `POST /api/keyexchange/accept` - Accept key exchange
- `GET /api/keyexchange/pending` - Get pending exchanges

## Development

### Running Tests

```bash
# Backend
cd backend-api
dotnet test

# Frontend
cd frontend-web
npm test

# Mobile
cd mobile-app
npm test
```

### Building for Production

```bash
# Backend
cd backend-api
dotnet publish -c Release

# Frontend
cd frontend-web
npm run build

# Mobile
cd mobile-app
npm run android:release  # or ios:release
```

## License

MIT

## Contributors

Built with ❤️ for secure communication
