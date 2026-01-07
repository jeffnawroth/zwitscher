# Zwitscher

Zwitscher is a social microblogging platform with a Vue 3 frontend, ASP.NET Core 7 backend API, SQL Server database, and an experimental Xamarin.Forms mobile client.

## Tech Stack

### Frontend
- **Framework**: Vue 3 with TypeScript
- **Build Tool**: Vite
- **UI Library**: Vuetify 3
- **State Management**: Pinia
- **HTTP Client**: Axios
- **Production**: NGINX container

### Backend
- **Framework**: ASP.NET Core 7 Web API
- **ORM**: Entity Framework Core
- **Authentication**: ASP.NET Identity + JWT Bearer
- **Database**: SQL Server 2022
- **API Documentation**: Swagger/OpenAPI

### Mobile App
- **Framework**: Xamarin.Forms
- **Target**: Android (in `app/App3`)

## Project Structure

```
zwitscher/
├── frontend/          # Vue 3 SPA
├── backend/           # ASP.NET Core API
│   └── iva-grp7-backend/
├── app/               # Xamarin.Forms mobile client
├── compose.dev.yaml   # Development Docker Compose
├── compose.yaml       # Production Docker Compose
└── .env.example       # Environment variables template
```

## Prerequisites

- **Docker** and **Docker Compose**
- **Node.js 18+** (if running frontend locally without Docker)
- **.NET 7 SDK** (if running backend locally without Docker)
- **pnpm** (recommended) or npm/yarn

## Getting Started

### 1. Environment Configuration

Copy the example environment file and configure your settings:

```bash
cp .env.example .env
```

Key environment variables:
- `VITE_API_BASE_URL` - Frontend API endpoint (e.g., `http://localhost:5176`)
- `DB_CONN` - SQL Server connection string
- `JWT_SECRET` - Secret key for JWT token signing
- `SA_PASSWORD` - SQL Server SA password

### 2. Running with Docker (Recommended)

#### Development Mode
Includes hot-reload for both frontend and backend:

```bash
docker compose -f compose.dev.yaml up --build
```

**Access points:**
- Frontend: http://localhost:3000
- Backend API: http://localhost:5176
- Swagger UI: http://localhost:5176/swagger
- SQL Server: `localhost:1433`

#### Production Mode
Optimized builds with NGINX serving the frontend:

```bash
docker compose -f compose.yaml up --build
```

**Access points:**
- Frontend: http://localhost:8081
- Backend API: http://localhost:5176

### 3. Running Manually (Optional)

#### Backend
```bash
cd backend/iva-grp7-backend
dotnet restore
dotnet run
```

Ensure SQL Server is accessible and environment variables are set.

#### Frontend
```bash
cd frontend
pnpm install
pnpm dev
```

Set `VITE_API_BASE_URL` in your `.env` file.

## Key Features

### Authentication & Authorization
- User registration and login
- JWT access tokens with refresh token support
- Role-based access (User, Moderator, Admin)
- Password requirements and validation

### User Management
- User profiles with avatars, bio, and interests
- Follow/unfollow functionality
- Gender and birth date fields
- Account locking capability (admin)

### Posts & Interactions
- Create posts with text and file attachments
- Nested comments (multiple levels)
- Upvote/downvote system
- Edit and delete capabilities
- Media file support (images, etc.)

### Social Features
- View posts from followed users
- User search by username
- Profile viewing
- Follower/following lists

### Admin Features
- User management dashboard
- Lock/unlock accounts
- Role assignment
- User statistics

## API Documentation

Once the backend is running, access the interactive Swagger documentation at:
```
http://localhost:5176/swagger
```

Main API endpoints:
- `/api/Authentication` - Register, login, token refresh
- `/api/User` - User management and profiles
- `/api/Post` - Post creation, retrieval, voting
- `/api/Dashboard` - Admin dashboard data

## Database

The application uses **SQL Server 2022** with Entity Framework Core. Migrations are automatically applied on application startup.

### Manual Migration Commands
```bash
cd backend/iva-grp7-backend
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Development

### Frontend Development
```bash
cd frontend
pnpm dev          # Start dev server
pnpm build        # Build for production
pnpm lint         # Lint code
pnpm lint:fix     # Fix linting issues
```

### Backend Development
The backend includes:
- Automatic Swagger documentation generation
- CORS configuration for cross-origin requests
- JWT bearer authentication middleware
- Entity Framework Core with SQL Server provider
