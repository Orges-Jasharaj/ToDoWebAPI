import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navbar = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="navbar-content">
        <div className="navbar-brand">
          <Link to="/dashboard" style={{ color: 'white', textDecoration: 'none' }}>
            ToDo App
          </Link>
        </div>
        <ul className="navbar-nav">
          <li>
            <Link to="/dashboard">Dashboard</Link>
          </li>
          {user && (
            <li>
              <span style={{ color: '#ccc', padding: '0.5rem 1rem' }}>
                Welcome, {user.email}
              </span>
            </li>
          )}
          <li>
            <button 
              onClick={handleLogout}
              style={{ 
                background: 'none', 
                border: 'none', 
                color: 'white', 
                cursor: 'pointer',
                padding: '0.5rem 1rem',
                borderRadius: '4px',
                transition: 'background-color 0.3s'
              }}
              onMouseOver={(e) => e.target.style.backgroundColor = '#555'}
              onMouseOut={(e) => e.target.style.backgroundColor = 'transparent'}
            >
              Logout
            </button>
          </li>
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;
