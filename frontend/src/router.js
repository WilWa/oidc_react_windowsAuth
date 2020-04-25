import React from 'react';
import { Route, Redirect, Switch } from 'react-router-dom';
import { PrivateRoute } from './PrivateRoute';
import Secure from './Secure';
import Unsecure from './Unsecure';
import Oidc_Callback from './Oidc_Callback';
import Oidc_Silent_Callback from './Oidc_Silent_Callback';
import Oidc_Logout_Callback from './Oidc_Logout_Callback';

const Router = () => (
  <Switch>
    <Redirect exact from="/" to="/unsecure" />
    <Route path="/unsecure" component={Unsecure} />
    <PrivateRoute path="/secure" component={Secure} />
    <Route path="/oidc_callback" component={Oidc_Callback} />
    <Route path="/oidc_silent_callback" component={Oidc_Silent_Callback} />
    <Route path="/oidc_logout_callback" component={Oidc_Logout_Callback} />
  </Switch>
);

export default Router;
