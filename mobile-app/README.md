# SpeakEasy Mobile App

Cross-platform React Native mobile application for secure messaging.

## Features

- iOS and Android support
- End-to-end encryption
- Native performance
- Secure key storage

## Setup

1. Install dependencies:
```bash
npm install
```

2. Install pods (iOS only):
```bash
cd ios && pod install && cd ..
```

3. Run the app:

For iOS:
```bash
npm run ios
```

For Android:
```bash
npm run android
```

## Configuration

Edit `src/services/api.ts` to configure the API endpoint:

```typescript
const API_BASE_URL = 'http://your-api-url/api';
```

For Android emulator, use `http://10.0.2.2:5000/api` to connect to localhost.

## Project Structure

```
mobile-app/
├── src/
│   ├── components/     # Reusable components
│   ├── context/        # React context
│   ├── screens/        # Screen components
│   ├── services/       # API and crypto services
│   └── types/          # TypeScript types
├── App.tsx             # Root component
└── index.js            # Entry point
```

## Building for Production

### Android
```bash
cd android
./gradlew assembleRelease
```

### iOS
```bash
cd ios
xcodebuild -workspace SpeakEasy.xcworkspace -scheme SpeakEasy -configuration Release
```

## Note on Encryption

The current implementation uses placeholder encryption for demonstration.
For production use, implement proper RSA encryption using libraries like:
- `react-native-rsa-native`
- `react-native-crypto`
