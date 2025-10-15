const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

export interface User {
  id: number;
  username: string;
  email: string;
  publicKey?: string;
  lastSeen?: string;
}

export interface AuthResponse {
  userId: number;
  username: string;
  email: string;
  token: string;
  publicKey?: string;
}

export interface Message {
  id: number;
  senderId: number;
  senderUsername: string;
  receiverId: number;
  receiverUsername: string;
  encryptedContent: string;
  sentAt: string;
  readAt?: string;
}

export interface Conversation {
  user: User;
  lastMessage?: Message;
  unreadCount: number;
}

export interface KeyExchange {
  id: number;
  initiatorId: number;
  initiatorUsername: string;
  receiverId: number;
  receiverUsername: string;
  initiatorPublicKey: string;
  receiverPublicKey?: string;
  status: 'Pending' | 'Accepted' | 'Rejected' | 'Completed';
  createdAt: string;
  completedAt?: string;
}

const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    ...(token && { Authorization: `Bearer ${token}` }),
  };
};

export const api = {
  auth: {
    register: async (username: string, email: string, password: string, publicKey?: string) => {
      const response = await fetch(`${API_BASE_URL}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, email, password, publicKey }),
      });
      if (!response.ok) throw new Error('Registration failed');
      return response.json() as Promise<AuthResponse>;
    },
    
    login: async (username: string, password: string) => {
      const response = await fetch(`${API_BASE_URL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password }),
      });
      if (!response.ok) throw new Error('Login failed');
      return response.json() as Promise<AuthResponse>;
    },
    
    getCurrentUser: async () => {
      const response = await fetch(`${API_BASE_URL}/auth/me`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) throw new Error('Failed to get current user');
      return response.json() as Promise<User>;
    },
    
    getAllUsers: async () => {
      const response = await fetch(`${API_BASE_URL}/auth/users`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) throw new Error('Failed to get users');
      return response.json() as Promise<User[]>;
    },
  },
  
  messages: {
    sendMessage: async (receiverId: number, encryptedContent: string) => {
      const response = await fetch(`${API_BASE_URL}/messages`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ receiverId, encryptedContent }),
      });
      if (!response.ok) throw new Error('Failed to send message');
      return response.json() as Promise<Message>;
    },
    
    getConversations: async () => {
      const response = await fetch(`${API_BASE_URL}/messages/conversations`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) throw new Error('Failed to get conversations');
      return response.json() as Promise<Conversation[]>;
    },
    
    getConversation: async (otherUserId: number) => {
      const response = await fetch(`${API_BASE_URL}/messages/conversation/${otherUserId}`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) throw new Error('Failed to get conversation');
      return response.json() as Promise<Message[]>;
    },
    
    markAsRead: async (messageId: number) => {
      const response = await fetch(`${API_BASE_URL}/messages/${messageId}/read`, {
        method: 'POST',
        headers: getAuthHeaders(),
      });
      return response.ok;
    },
  },
  
  keyExchange: {
    initiate: async (receiverId: number, publicKey: string) => {
      const response = await fetch(`${API_BASE_URL}/keyexchange/initiate`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ receiverId, publicKey }),
      });
      if (!response.ok) throw new Error('Failed to initiate key exchange');
      return response.json() as Promise<KeyExchange>;
    },
    
    accept: async (keyExchangeId: number, publicKey: string) => {
      const response = await fetch(`${API_BASE_URL}/keyexchange/accept`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify({ keyExchangeId, publicKey }),
      });
      if (!response.ok) throw new Error('Failed to accept key exchange');
      return response.json() as Promise<KeyExchange>;
    },
    
    getPending: async () => {
      const response = await fetch(`${API_BASE_URL}/keyexchange/pending`, {
        headers: getAuthHeaders(),
      });
      if (!response.ok) throw new Error('Failed to get pending key exchanges');
      return response.json() as Promise<KeyExchange[]>;
    },
    
    getForUser: async (otherUserId: number) => {
      const response = await fetch(`${API_BASE_URL}/keyexchange/user/${otherUserId}`, {
        headers: getAuthHeaders(),
      });
      if (response.status === 404) return null;
      if (!response.ok) throw new Error('Failed to get key exchange');
      return response.json() as Promise<KeyExchange>;
    },
  },
};
