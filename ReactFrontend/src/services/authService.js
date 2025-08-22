import axios from 'axios';

const API_BASE_URL = 'http://localhost:5001/api';

// Create axios instance with base configuration
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests if available
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Handle token expiration
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authService = {
  async login(email, password) {
    try {
      const response = await api.post('/auth/login', { email, password });
      // API returns { displayName, email, accessToken, refreshToken, refreshTokenExpiryTime }
      return { success: true, data: response.data };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Login failed. Please try again.'
      };
    }
  },

  async register(userData) {
    try {
      // Expecting { firstName, lastName, dateOfBirth, email, password }
      const response = await api.post('/auth/register', userData);
      return { success: true, data: response.data };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Registration failed. Please try again.'
      };
    }
  },

  async refreshToken(refreshToken) {
    try {
      const response = await api.post('/auth/refreshtoken', { refreshToken });
      return { success: true, data: response.data };
    } catch (error) {
      return { success: false, message: 'Token refresh failed' };
    }
  }
};
