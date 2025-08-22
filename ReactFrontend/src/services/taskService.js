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

export const taskService = {
  async getAllTasks() {
    try {
      const response = await api.get('/task');
      return {
        success: true,
        data: response.data
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to fetch tasks'
      };
    }
  },

  async getTaskById(id) {
    try {
      const response = await api.get(`/task/${id}`);
      return {
        success: true,
        data: response.data
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to fetch task'
      };
    }
  },

  async createTask(taskData) {
    try {
      const response = await api.post('/task', taskData);
      return {
        success: true,
        data: response.data
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to create task'
      };
    }
  },

  async updateTask(id, taskData) {
    try {
      const response = await api.put(`/task/${id}`, taskData);
      return {
        success: true,
        data: response.data
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to update task'
      };
    }
  },

  async deleteTask(id) {
    try {
      await api.delete(`/task/${id}`);
      return {
        success: true
      };
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Failed to delete task'
      };
    }
  },

  async markTaskAsCompleted(id, completedBy) {
    try {
      // API expects a raw string body; ensure it's quoted JSON string
      const response = await api.post(`/task/${id}/complete`, JSON.stringify(completedBy), {
        headers: { 'Content-Type': 'application/json' }
      });
      return { success: true, data: response.data };
    } catch (error) {
      return { success: false, message: error.response?.data?.message || 'Failed to mark task as completed' };
    }
  }
};
