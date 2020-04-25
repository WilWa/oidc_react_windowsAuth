import React from 'react';
import AuthService from './authService';

const authContext = React.createContext(null);

const authService = new AuthService();

export default function AuthProvider({ children }) {
  const [user, setUser] = React.useState({});
  const [ready, setReady] = React.useState(false);

  authService.Events.accessTokenExpired = () => {
    setUser(null);
  };

  React.useEffect(() => {
    const getUser = async () => {
      const user = await authService.getUser();
      console.log(user);
      setUser(user);
      setReady(true);
    };
    getUser();
  }, []);

  const signinRedirect = async () => {
    setReady(false);
    await authService.signinRedirect();
  };

  const signinRedirectCallback = async () => {
    const user = await authService.signInRedirectCallback();
    setUser(user);
    setReady(true);
  };

  const signinSilentCallback = async () => {
    await authService.signinSilentCallback();
    setUser(user);
  };

  const signoutRedirect = async () => {
    await authService.signoutRedirect();
  };

  const signoutRedirectCallback = async () => {
    await authService.signoutRedirectCallback();
    setUser(null);
    setReady(true);
  };

  return (
    <authContext.Provider
      value={{
        ready,
        user,
        signinRedirect,
        signinRedirectCallback,
        signinSilentCallback,
        signoutRedirect,
        signoutRedirectCallback,
      }}
    >
      {children}
    </authContext.Provider>
  );
}
AuthProvider.context = authContext;
