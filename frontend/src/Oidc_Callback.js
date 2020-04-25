import React, { useEffect } from 'react';
import AuthProvider from './authProvider';

function Oidc_Callback() {
  const { signinRedirectCallback } = React.useContext(AuthProvider.context);

  useEffect(() => {
    signinRedirectCallback();
  }, [signinRedirectCallback]);

  return <div>[Oidc_Callback] Loading...</div>;
}

export default Oidc_Callback;
