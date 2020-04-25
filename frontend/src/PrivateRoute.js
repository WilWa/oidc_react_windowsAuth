import React from 'react';
import { Route } from 'react-router-dom';
import AuthProvider from './authProvider';
import SignInPage from './SignInPage';
import Loading from './Loading';

export const PrivateRoute = ({ component, ...options }) => {
  const { ready, user } = React.useContext(AuthProvider.context);

  const finalComponent = ready
    ? !!user && !!user.access_token
      ? component
      : SignInPage
    : Loading;

  return !ready ? (
    <div>[PrivateRoute] Loading...</div>
  ) : (
    <Route {...options} component={finalComponent} />
  );
};
