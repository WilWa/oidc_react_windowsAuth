import { Log, UserManager } from 'oidc-client';

export default class AuthService {
  UserManager;
  SilentUser;
  Events = {
    accessTokenExpired: () => ({}),
  };

  constructor() {
    this.UserManager = new UserManager({
      accessTokenExpiringNotificationTime: 40,
      authority: 'https://localhost:44395/',
      client_id: 'poManagerFrontEnd',
      post_logout_redirect_uri: 'http://localhost:3000/oidc_logout_callback',
      redirect_uri: 'http://localhost:3000/oidc_callback',
      silent_redirect_uri: 'http://localhost:3000/oidc_silent_callback',
      response_mode: 'query',
      response_type: 'code',
      scope: 'openid profile poManagerApi',
    });
    Log.logger = console;
    Log.level = Log.DEBUG;
    this.UserManager.events.addAccessTokenExpired(() => {
      localStorage.clear();
      this.UserManager.clearStaleState();
      this.Events.accessTokenExpired();
    });
    this.UserManager.events.addAccessTokenExpiring(async () => {
      await this.signinSilent();
    });
  }

  getUser = async () => {
    try {
      return await this.UserManager.getUser();
    } catch (err) {
      console.log(`getUser ${err}`);
    }
  };

  signinRedirect = async () => {
    try {
      await this.UserManager.signinRedirect({ state: window.location.href });
    } catch (err) {
      console.log(`signinRedirect ${err}`);
    }
  };

  signInRedirectCallback = async () => {
    try {
      const user = await this.UserManager.signinRedirectCallback({
        state: window.location.href,
      });
      window.history.replaceState(
        {},
        window.document.title,
        window.location.origin + window.location.pathname
      );
      window.location = user.state || '/';
      return user;
    } catch (err) {
      console.log(`signInRedirectCallback ${err}`);
    }
  };

  signinSilent = async () => {
    try {
      console.log('called signinSilent');
      const user = await this.UserManager.signinSilent();
      this.SilentUser = user;
    } catch (err) {
      console.log(`signinSilent ${err}`);
    }
  };

  signinSilentCallback = async () => {
    try {
      await this.UserManager.signinSilentCallback();
      return this.SilentUser;
    } catch (err) {
      console.log(`signinSilentCallback ${err}`);
    }
  };

  signoutRedirect = async () => {
    try {
      await this.UserManager.signoutRedirect();
    } catch (err) {
      console.log(`signoutRedirect ${err}`);
    }
  };

  signoutRedirectCallback = async () => {
    try {
      await this.UserManager.signoutRedirectCallback();
      window.history.replaceState(
        {},
        window.document.title,
        window.location.origin + window.location.pathname
      );
      window.location = '/';
    } catch (err) {
      console.log(`signoutRedirectCallback ${err}`);
    }
  };
}
