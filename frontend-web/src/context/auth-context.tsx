import { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import { api, User, AuthResponse } from '@/lib/api';
import { generateKeyPair } from '@/lib/crypto';

interface AuthContextType {
  user: User | null;
  token: string | null;
  privateKey: string | null;
  login: (username: string, password: string) => Promise<void>;
  register: (username: string, email: string, password: string) => Promise<void>;
  logout: () => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [privateKey, setPrivateKey] = useState<string | null>(localStorage.getItem('privateKey'));
  const [isLoading, setIsLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const initAuth = async () => {
      if (token) {
        try {
          const currentUser = await api.auth.getCurrentUser();
          setUser(currentUser);
        } catch (error) {
          localStorage.removeItem('token');
          localStorage.removeItem('privateKey');
          setToken(null);
          setPrivateKey(null);
        }
      }
      setIsLoading(false);
    };

    initAuth();
  }, [token]);

  const login = async (username: string, password: string) => {
    const response: AuthResponse = await api.auth.login(username, password);
    localStorage.setItem('token', response.token);
    setToken(response.token);
    setUser({
      id: response.userId,
      username: response.username,
      email: response.email,
      publicKey: response.publicKey,
    });
    navigate('/');
  };

  const register = async (username: string, email: string, password: string) => {
    const keys = await generateKeyPair();
    const response: AuthResponse = await api.auth.register(username, email, password, keys.publicKey);
    localStorage.setItem('token', response.token);
    localStorage.setItem('privateKey', keys.privateKey);
    setToken(response.token);
    setPrivateKey(keys.privateKey);
    setUser({
      id: response.userId,
      username: response.username,
      email: response.email,
      publicKey: response.publicKey,
    });
    navigate('/');
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('privateKey');
    setToken(null);
    setPrivateKey(null);
    setUser(null);
    navigate('/login');
  };

  return (
    <AuthContext.Provider value={{ user, token, privateKey, login, register, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
