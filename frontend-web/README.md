# SpeakEasy Web Frontend

React web application with end-to-end encrypted messaging.

## Features

- Modern React with TypeScript
- shadcn/ui component library
- TailwindCSS for styling
- End-to-end encryption
- Real-time messaging interface

## Setup

1. Install dependencies:
```bash
npm install
```

2. Create `.env` file:
```
VITE_API_URL=http://localhost:5000/api
```

3. Run development server:
```bash
npm run dev
```

## Building

```bash
npm run build
```

The build output will be in the `dist` directory.

## Project Structure

```
frontend-web/
├── src/
│   ├── components/      # UI components
│   │   └── ui/         # shadcn components
│   ├── context/        # React context
│   ├── lib/            # Utilities
│   │   ├── api.ts      # API client
│   │   ├── crypto.ts   # Encryption
│   │   └── utils.ts    # Helpers
│   ├── pages/          # Page components
│   ├── App.tsx         # Main app component
│   └── main.tsx        # Entry point
└── index.html
```

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint
