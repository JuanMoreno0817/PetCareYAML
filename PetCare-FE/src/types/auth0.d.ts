import { User } from "@auth0/auth0-react";

export interface Auth0User extends User {
  email?: string;
  email_verified?: boolean;
  name?: string;
  nickname?: string;
  picture?: string;
  sub?: string;
  updated_at?: string;
}

export interface Auth0Context {
  user?: Auth0User;
  isAuthenticated: boolean;
  isLoading: boolean;
  loginWithRedirect: () => Promise<void>;
  logout: () => void;
  getAccessTokenSilently: () => Promise<string>;
}
