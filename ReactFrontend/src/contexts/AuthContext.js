import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../services/authService';

const AuthContext = createContext();

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem('token');
    const email = localStorage.getItem('email');
    const displayName = localStorage.getItem('displayName');
    if (token) {
      setIsAuthenticated(true);
      setUser(email ? { email, displayName } : null);
    }
    setLoading(false);
  }, []);

  const login = async (email, password) => {
    try {
      const response = await authService.login(email, password);
      if (response.success) {
        // API may return ResponseDto<LoginResponseDto>
        const payload = response.data?.data ?? response.data;
        const accessToken = payload?.accessToken || payload?.AccessToken;
        const respEmail = payload?.email || payload?.Email || email;
        const displayName = payload?.displayName || payload?.DisplayName || '';
        if (accessToken) localStorage.setItem('token', accessToken);
        if (respEmail) localStorage.setItem('email', respEmail);
        if (displayName) localStorage.setItem('displayName', displayName);
        setUser({ email: respEmail, displayName });
        setIsAuthenticated(true);
        return { success: true };
      } else {
        return { success: false, message: response.message };
      }
    } catch (error) {
      return { success: false, message: 'An error occurred during login' };
    }
  };

  const register = async (userData) => {
    try {
      const response = await authService.register(userData);
      if (response.success) {
        return { success: true, message: 'Registration successful! Please login.' };
      } else {
        const msg = response.data?.message || response.message || 'Registration failed. Please try again.';
        return { success: false, message: msg };
      }
    } catch (error) {
      return { success: false, message: 'An error occurred during registration' };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('displayName');
    setUser(null);
    setIsAuthenticated(false);
  };

  const value = { user, isAuthenticated, loading, login, register, logout };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};
