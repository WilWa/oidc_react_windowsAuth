import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import Router from './router';
import AuthProvider from './authProvider';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Router />
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
