import React, { useState, useEffect } from 'react';
import { taskService } from '../services/taskService';
import { useAuth } from '../contexts/AuthContext';
import TaskForm from '../components/TaskForm';
import TaskList from '../components/TaskList';

const Dashboard = () => {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState('');
  const [showTaskForm, setShowTaskForm] = useState(false);
  const [editingTask, setEditingTask] = useState(null);

  const { user } = useAuth();

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    setLoading(true);
    const result = await taskService.getAllTasks();
    
    if (result.success) {
      setTasks(result.data);
    } else {
      setMessageType('danger');
      setMessage(result.message);
    }
    
    setLoading(false);
  };

  const handleCreateTask = async (taskData) => {
    const result = await taskService.createTask(taskData);
    
    if (result.success) {
      setMessageType('success');
      setMessage('Task created successfully!');
      setShowTaskForm(false);
      fetchTasks();
      setTimeout(() => setMessage(''), 3000);
    } else {
      setMessageType('danger');
      setMessage(result.message);
    }
  };

  const handleUpdateTask = async (id, taskData) => {
    const result = await taskService.updateTask(id, taskData);
    
    if (result.success) {
      setMessageType('success');
      setMessage('Task updated successfully!');
      setEditingTask(null);
      fetchTasks();
      setTimeout(() => setMessage(''), 3000);
    } else {
      setMessageType('danger');
      setMessage(result.message);
    }
  };

  const handleDeleteTask = async (id) => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      const result = await taskService.deleteTask(id);
      
      if (result.success) {
        setMessageType('success');
        setMessage('Task deleted successfully!');
        fetchTasks();
        setTimeout(() => setMessage(''), 3000);
      } else {
        setMessageType('danger');
        setMessage(result.message);
      }
    }
  };

  const handleCompleteTask = async (id) => {
    const result = await taskService.markTaskAsCompleted(id, user.email);
    
    if (result.success) {
      setMessageType('success');
      setMessage('Task marked as completed!');
      fetchTasks();
      setTimeout(() => setMessage(''), 3000);
    } else {
      setMessageType('danger');
      setMessage(result.message);
    }
  };

  const handleEditTask = (task) => {
    setEditingTask(task);
    setShowTaskForm(true);
  };

  const handleCancelEdit = () => {
    setEditingTask(null);
    setShowTaskForm(false);
  };

  if (loading) {
    return (
      <div className="text-center">
        <h2>Loading...</h2>
      </div>
    );
  }

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>My Tasks</h1>
        <button
          className="btn btn-primary"
          onClick={() => setShowTaskForm(true)}
        >
          Add New Task
        </button>
      </div>

      {message && (
        <div className={`alert alert-${messageType}`}>
          {message}
        </div>
      )}

      {showTaskForm && (
        <TaskForm
          onSubmit={editingTask ? handleUpdateTask : handleCreateTask}
          onCancel={editingTask ? handleCancelEdit : () => setShowTaskForm(false)}
          task={editingTask}
          isEditing={!!editingTask}
        />
      )}

      <TaskList
        tasks={tasks}
        onEdit={handleEditTask}
        onDelete={handleDeleteTask}
        onComplete={handleCompleteTask}
      />
    </div>
  );
};

export default Dashboard;
