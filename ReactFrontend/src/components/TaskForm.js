import React, { useState, useEffect } from 'react';

const TaskForm = ({ onSubmit, onCancel, task, isEditing }) => {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    dificulty: 1,
    deadLine: ''
  });

  useEffect(() => {
    if (task) {
      setFormData({
        title: task.title || '',
        description: task.description || '',
        dificulty: task.dificulty || 1,
        deadLine: task.deadLine ? new Date(task.deadLine).toISOString().split('T')[0] : ''
      });
    }
  }, [task]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const taskData = {
      title: formData.title,
      description: formData.description,
      dificulty: parseInt(formData.dificulty),
      deadLine: new Date(formData.deadLine).toISOString()
    };

    if (isEditing) {
      onSubmit(task.id, taskData);
    } else {
      onSubmit(taskData);
    }
  };

  const getDifficultyLabel = (value) => {
    const labels = { 1: 'Very Easy', 2: 'Easy', 3: 'Medium', 4: 'Hard', 5: 'Very Hard' };
    return labels[value] || 'Unknown';
  };

  return (
    <div className="task-form">
      <h3>{isEditing ? 'Edit Task' : 'Create New Task'}</h3>

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="title">Title *</label>
          <input type="text" id="title" name="title" className="form-control" value={formData.title} onChange={handleChange} required placeholder="Enter task title" />
        </div>

        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea id="description" name="description" className="form-control" value={formData.description} onChange={handleChange} rows="3" placeholder="Enter task description (optional)" />
        </div>

        <div className="form-group">
          <label htmlFor="dificulty">Difficulty Level</label>
          <select id="dificulty" name="dificulty" className="form-control" value={formData.dificulty} onChange={handleChange}>
            <option value="1">1 - Very Easy</option>
            <option value="2">2 - Easy</option>
            <option value="3">3 - Medium</option>
            <option value="4">4 - Hard</option>
            <option value="5">5 - Very Hard</option>
          </select>
          <small className="text-muted">Current: {getDifficultyLabel(formData.dificulty)}</small>
        </div>

        <div className="form-group">
          <label htmlFor="deadLine">Deadline *</label>
          <input type="date" id="deadLine" name="deadLine" className="form-control" value={formData.deadLine} onChange={handleChange} required min={new Date().toISOString().split('T')[0]} />
        </div>

        <div className="task-actions">
          <button type="submit" className="btn btn-primary">{isEditing ? 'Update Task' : 'Create Task'}</button>
          <button type="button" className="btn btn-danger" onClick={onCancel} style={{ marginLeft: '10px' }}>Cancel</button>
        </div>
      </form>
    </div>
  );
};

export default TaskForm;
