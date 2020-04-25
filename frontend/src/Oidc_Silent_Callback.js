import React from 'react';
import AuthProvider from './authProvider';

function Oidc_Silent_Callback() {
  const { signinSilentCallback } = React.useContext(AuthProvider.context);

  React.useEffect(() => {
    signinSilentCallback();
  }, [signinSilentCallback]);

  return <div>[Oidc_Silent_Callback] Loading...</div>;
}

export default Oidc_Silent_Callback;
