import React, { useEffect } from 'react';
import AuthProvider from './authProvider';

function Oidc_Logout_Callback() {
  const { signoutRedirectCallback } = React.useContext(AuthProvider.context);

  useEffect(() => {
    signoutRedirectCallback();
  }, [signoutRedirectCallback]);

  return <div>[Oidc_Logout_Callback] Loading...</div>;
}

export default Oidc_Logout_Callback;
