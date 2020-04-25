import React from 'react';
import AuthProvider from './authProvider';

function SignInPage() {
  const { signinRedirect } = React.useContext(AuthProvider.context);

  React.useEffect(() => {
    signinRedirect();
  }, [signinRedirect]);

  return <div>[SignInPage] Loading...</div>;
}

export default SignInPage;
