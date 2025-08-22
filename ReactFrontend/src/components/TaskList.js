import React from 'react';

const TaskList = ({ tasks, onEdit, onDelete, onComplete }) => {
  if (!tasks || tasks.length === 0) {
    return (
      <div className="text-center">
        <h3>No tasks found</h3>
        <p>Create your first task to get started!</p>
      </div>
    );
  }

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  const getDifficultyBadge = (difficulty) => {
    const difficultyClass = `difficulty-${difficulty}`;
    const labels = {
      1: 'Very Easy',
      2: 'Easy',
      3: 'Medium',
      4: 'Hard',
      5: 'Very Hard'
    };
    
    return (
      <span className={`difficulty-badge ${difficultyClass}`}>
        {difficulty} - {labels[difficulty]}
      </span>
    );
  };

  const isOverdue = (deadline) => {
    return new Date(deadline) < new Date() && !tasks.find(t => t.deadline === deadline)?.isCompleted;
  };

  return (
    <div className="task-list">
      {tasks.map((task) => (
        <div 
          key={task.id} 
          className={`task-item ${task.isCompleted ? 'completed' : ''}`}
        >
          <div className="task-header">
            <div>
              <h4 className="task-title">{task.title}</h4>
              <div className="task-meta">
                <span>Created: {formatDate(task.createdAt)}</span>
                {task.updatedAt && (
                  <span style={{ marginLeft: '15px' }}>
                    Updated: {formatDate(task.updatedAt)}
                  </span>
                )}
              </div>
            </div>
            <div style={{ textAlign: 'right' }}>
              {getDifficultyBadge(task.dificulty)}
              <div className="task-meta" style={{ marginTop: '5px' }}>
                Deadline: {formatDate(task.deadLine)}
                {isOverdue(task.deadLine) && (
                  <span style={{ color: '#dc3545', marginLeft: '10px' }}>
                    ⚠️ Overdue
                  </span>
                )}
              </div>
            </div>
          </div>

          {task.description && (
            <p style={{ margin: '10px 0', color: '#666' }}>
              {task.description}
            </p>
          )}

          {task.isCompleted && (
            <div style={{ 
              backgroundColor: '#d4edda', 
              color: '#155724', 
              padding: '8px', 
              borderRadius: '4px',
              margin: '10px 0'
            }}>
              ✅ Completed on {formatDate(task.completedAt)} by {task.completedBy}
            </div>
          )}

          <div className="task-actions">
            {!task.isCompleted && (
              <>
                <button
                  className="btn btn-success"
                  onClick={() => onComplete(task.id)}
                  title="Mark as completed"
                >
                  Complete
                </button>
                <button
                  className="btn btn-primary"
                  onClick={() => onEdit(task)}
                  title="Edit task"
                >
                  Edit
                </button>
              </>
            )}
            <button
              className="btn btn-danger"
              onClick={() => onDelete(task.id)}
              title="Delete task"
            >
              Delete
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};

export default TaskList;
