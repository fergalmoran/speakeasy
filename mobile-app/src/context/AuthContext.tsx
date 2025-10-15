import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { api, User } from '../services/api';
import { generateKeyPair } from '../services/crypto';

interface AuthContextType {
  user: User | null;
  token: string | null;
  privateKey: string | null;
  login: (username: string, password: string) => Promise<void>;
  register: (username: string, email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [privateKey, setPrivateKey] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadStoredAuth();
  }, []);

  const loadStoredAuth = async () => {
    try {
      const storedToken = await AsyncStorage.getItem('token');
      const storedPrivateKey = await AsyncStorage.getItem('privateKey');
      
      if (storedToken) {
        setToken(storedToken);
        setPrivateKey(storedPrivateKey);
        const currentUser = await api.auth.getCurrentUser(storedToken);
        setUser(currentUser);
      }
    } catch (error) {
      console.error('Failed to load auth', error);
    } finally {
      setIsLoading(false);
    }
  };

  const login = async (username: string, password: string) => {
    const response = await api.auth.login(username, password);
    await AsyncStorage.setItem('token', response.token);
    setToken(response.token);
    setUser({
      id: response.userId,
      username: response.username,
      email: response.email,
      publicKey: response.publicKey,
    });
  };

  const register = async (username: string, email: string, password: string) => {
    const keys = await generateKeyPair();
    const response = await api.auth.register(username, email, password, keys.publicKey);
    await AsyncStorage.setItem('token', response.token);
    await AsyncStorage.setItem('privateKey', keys.privateKey);
    setToken(response.token);
    setPrivateKey(keys.privateKey);
    setUser({
      id: response.userId,
      username: response.username,
      email: response.email,
      publicKey: response.publicKey,
    });
  };

  const logout = async () => {
    await AsyncStorage.removeItem('token');
    await AsyncStorage.removeItem('privateKey');
    setToken(null);
    setPrivateKey(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, token, privateKey, login, register, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
