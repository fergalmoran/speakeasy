import axios from 'axios';

const API_BASE_URL = 'http://10.0.2.2:5000/api'; // Android emulator localhost

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

export const api = {
  auth: {
    register: async (username: string, email: string, password: string, publicKey?: string) => {
      const response = await axios.post<AuthResponse>(`${API_BASE_URL}/auth/register`, {
        username,
        email,
        password,
        publicKey,
      });
      return response.data;
    },
    
    login: async (username: string, password: string) => {
      const response = await axios.post<AuthResponse>(`${API_BASE_URL}/auth/login`, {
        username,
        password,
      });
      return response.data;
    },
    
    getCurrentUser: async (token: string) => {
      const response = await axios.get<User>(`${API_BASE_URL}/auth/me`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    },
    
    getAllUsers: async (token: string) => {
      const response = await axios.get<User[]>(`${API_BASE_URL}/auth/users`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    },
  },
  
  messages: {
    sendMessage: async (token: string, receiverId: number, encryptedContent: string) => {
      const response = await axios.post<Message>(
        `${API_BASE_URL}/messages`,
        { receiverId, encryptedContent },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      return response.data;
    },
    
    getConversation: async (token: string, otherUserId: number) => {
      const response = await axios.get<Message[]>(
        `${API_BASE_URL}/messages/conversation/${otherUserId}`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      return response.data;
    },
  },
};
